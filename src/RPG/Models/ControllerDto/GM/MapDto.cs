using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using RPG.Models.Context;
using RPG.Models.ControllerDto.GM.Helper;
using RPG.Models.GmModal.World;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.ControllerDto.GM
{
    public class MapDto
    {
        public MapDto() { }
        public MapDto(ContextManager context, Guid? selectedMap)
        {
            if (selectedMap.HasValue)
            {
                SelectedItem = context.Get<Map>(selectedMap.Value);
                var locations = context.Context.Locations
                    .Where(x => x.Map.ID == selectedMap.Value && x.Lat != null && x.Lon != null && x.LocationType != null).Include(x => x.LocationType).ToList();

                var mapMetaData = new List<MapLocationDto>();
                foreach (var loc in locations)
                {
                    mapMetaData.Add(new MapLocationDto(loc));
                }
                
                MapMetaData = JsonConvert.SerializeObject(mapMetaData);
            }
            Maps = new List<ElementId>(context.GetAll<Map>().OrderBy(x => x.Name));
        }
        public Map SelectedItem { get; set; }
        public HttpPostedFileBase Data { get; set; }
        public List<ElementId> Maps { get; set; }
        public string MapMetaData { get; set; }
    }
}