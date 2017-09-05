using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using RPG.Models.CharacterModal;
using RPG.Models.ControllerDto.GM;
using RPG.Models.ControllerDto.GM.Helper;
using RPG.Models.GmModal.World;

namespace RPG.Controllers.GM
{
    public class HistoryController : ControllerBase
    {
        public void SetViewBag()
        {
            var chars = new List<JsonAtDto>();
            chars.AddRange(Context.Context.Characters.Select(x => new JsonAtDto { Name = x.Name, Type = typeof(Character).Name, Id = x.ID }));
            chars.AddRange(Context.Context.NPCs.Select(x => new JsonAtDto { Name = x.Name, Type = typeof(NPC).Name, Id = x.ID }));
            SetLinks(chars);
            ViewBag.Characters = new JavaScriptSerializer().Serialize(chars.OrderBy(x => x.Name));

            var locations = new List<JsonAtDto>();
            locations.AddRange(Context.Context.Locations.Select(x => new JsonAtDto { Name = x.Name, Type = typeof(Location).Name, Id = x.ID }));
            SetLinks(locations);
            ViewBag.Locations = new JavaScriptSerializer().Serialize(locations.OrderBy(x => x.Name));

        }
        private void SetLinks(List<JsonAtDto> data)
        {
            foreach (var jsonAtDto in data)
            {
                if (jsonAtDto.Type == typeof(Character).Name)
                {
                    jsonAtDto.Link = UrlHelper.GenerateUrl(null, "Index", "CharacterOverview", new RouteValueDictionary(new { id = jsonAtDto.Id }), 
                    RouteTable.Routes, this.ControllerContext.RequestContext, false);
                }
                else if (jsonAtDto.Type == typeof(NPC).Name)
                {
                    jsonAtDto.Link = UrlHelper.GenerateUrl(null, "Index", "NpcOverview", new RouteValueDictionary(new { id = jsonAtDto.Id }),
                        RouteTable.Routes, this.ControllerContext.RequestContext, false);
                }
                else if (jsonAtDto.Type == typeof(Location).Name)
                {
                    jsonAtDto.Link = UrlHelper.GenerateUrl(null, "Index", "Location", new RouteValueDictionary(new { id = jsonAtDto.Id }),
                        RouteTable.Routes, this.ControllerContext.RequestContext, false);
                }
            }
        }

        // GET: History
        public ActionResult Index(Guid? id)
        {
            SetViewBag();
            return View("GM/History", new HistoryDto(Context, id));
        }
        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                Context.Delete<History>(id.Value);
            }
            SetViewBag();
            return View("GM/History", new HistoryDto(Context, null));
        }
        [ValidateInput(false)]
        public ActionResult SaveHistory(Guid? id, string history)
        {
            History historyToUpdate;
            if (id.HasValue)
            {
                historyToUpdate = Context.Get<History>(id.Value);
            }
            else
            {
                historyToUpdate = new History();
                historyToUpdate.Created = DateTime.Now;
            }
            historyToUpdate.Description = history;
            Context.CreateOrUpdate(historyToUpdate);
            //<p hidden="">Character:3107a056-8868-e711-bb8f-9b6742426148</p>
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}