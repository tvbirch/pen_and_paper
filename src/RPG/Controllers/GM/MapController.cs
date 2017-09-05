using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using RPG.Models.ControllerDto.GM;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.GmModal.World;

namespace RPG.Controllers.GM
{
    public class MapController : ControllerBase
    {
        // GET: Map
        public ActionResult Index(Guid? map)
        {
            if (!map.HasValue)
            {
                map = Context.GetQueryable<Map>().Select(x => x.ID).FirstOrDefault();
            }
            return View("GM/Map", new MapDto(Context, map));
        }

        [HttpPost]
        public ActionResult Save(MapDto map)
        {
            Map mapToUpdate;
            if (map.SelectedItem.ID == Guid.Empty)
            {
                mapToUpdate = map.SelectedItem;
            }
            else
            {
                mapToUpdate = Context.Get<Map>(map.SelectedItem.ID);
            }
            mapToUpdate.Description = map.SelectedItem.Description;
            mapToUpdate.Name = map.SelectedItem.Name;
            mapToUpdate.Width = map.SelectedItem.Width;
            mapToUpdate.Height = map.SelectedItem.Height;
            mapToUpdate.MinZoom = map.SelectedItem.MinZoom;
            mapToUpdate.MaxZoom = map.SelectedItem.MaxZoom;
            mapToUpdate.DistanceScale = map.SelectedItem.DistanceScale;

            if (map.Data != null)
            {
                mapToUpdate.Parts = new List<MapPart>();
                using (var zip = new ZipFile(map.Data.FileName))
                {
                    foreach (var zipEntry in zip.Entries)
                    {
                        if (zipEntry.IsDirectory)
                        {
                            continue;
                        }
                        using (var memStream = new MemoryStream())
                        {
                            zipEntry.Extract(memStream);
                            var data = memStream.ToArray();
                            var nameParts = zipEntry.FileName.Split('/');
                            var z = nameParts[nameParts.Length - 3];
                            var x = nameParts[nameParts.Length - 2];
                            var y = nameParts[nameParts.Length - 1];
                            var fileType = y.Split('.').Last();
                            y = y.Split('.').First();
                            mapToUpdate.Parts.Add(new MapPart
                            {
                                Data = data,
                                X = int.Parse(x),
                                Y = int.Parse(y),
                                Z = int.Parse(z),
                                FileType = fileType,
                            });
                        }
                    }
                }
            }
            
            Context.CreateOrUpdate(mapToUpdate);
            return RedirectToAction("Index", "Map", new { id = mapToUpdate.ID });
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                Context.Delete<Map>(id.Value);
            }
            return Index(null);
        }

        public ActionResult CreateMapIndex(Guid? map)
        {
            return View("GM/CreateMap", new MapDto(Context, map));
        }

        [OutputCache(Duration = 3600)]
        public FileResult GetMapPart(Guid map, int z, int x, int y)
        {
            var result = Context.GetQueryable<MapPart>().FirstOrDefault(mp => mp.Map.ID == map && mp.Z == z && mp.X == x && mp.Y == y);
            if (result == null)
            {
                return null;
            }
            return new FileStreamResult(new MemoryStream(result.Data), "image/"+result.FileType);
        }

    }
}