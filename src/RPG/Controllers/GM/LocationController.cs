using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.ControllerDto.GM;
using RPG.Models.GmModal.World;

namespace RPG.Controllers.GM
{
    public class LocationController : ControllerBase
    {
        // GET: Location
        public ActionResult Index(Guid? id)
        {
            var dto = new LocationDto(Context,id);
            return View("GM/Location",dto);
        }

        public ActionResult Save(LocationDto data)
        {
            Location locationToUpdate;
            if (data.SelectedItem.ID != Guid.Empty)
            {
                locationToUpdate = Context.Get<Location>(data.SelectedItem.ID);
            }
            else
            {
                locationToUpdate = new Location();
            }

            if (data.LocationTypeId.HasValue && data.LocationTypeId != Guid.Empty)
            {
                locationToUpdate.LocationType = Context.Get<LocationType>(data.LocationTypeId.Value);
            }
            else
            {
                locationToUpdate.LocationType = null;
            }

            if (data.MapId.HasValue && data.MapId != Guid.Empty)
            {
                locationToUpdate.Map = Context.Get<Map>(data.MapId.Value);
            }
            else
            {
                locationToUpdate.Map = null;
            }

            locationToUpdate.Name = data.SelectedItem.Name;
            locationToUpdate.Description = data.SelectedItem.Description;
            locationToUpdate.Lat = data.SelectedItem.Lat;
            locationToUpdate.Lon = data.SelectedItem.Lon;
            locationToUpdate.Layer = data.SelectedItem.Layer;

            locationToUpdate.FeaturesAndLandmarks = data.SelectedItem.FeaturesAndLandmarks;
            locationToUpdate.Governance = data.SelectedItem.Governance;
            locationToUpdate.Military = data.SelectedItem.Military;
            locationToUpdate.NotableLocations = data.SelectedItem.NotableLocations;
            locationToUpdate.ProsperityAndCrime = data.SelectedItem.ProsperityAndCrime;
            locationToUpdate.Races = data.SelectedItem.Races;
            locationToUpdate.SourceOfFood = data.SelectedItem.SourceOfFood;
            locationToUpdate.SourceOfIncome = data.SelectedItem.SourceOfIncome;
            locationToUpdate.GroupsAndFactions = data.SelectedItem.GroupsAndFactions;
            locationToUpdate.Population = data.SelectedItem.Population;


            Context.CreateOrUpdate(locationToUpdate);
            return RedirectToAction("Index", "Location", new { id = locationToUpdate.ID });
            //return View("GM/Location", dto);
        }

        public ActionResult Delete(Guid? id)
        {
            var dto = new LocationDto(Context, null);
            return View("GM/Location", dto);
        }
    }
}