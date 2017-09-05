using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.GmModal.World;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.ControllerDto.GM
{
    public class LocationDto
    {
        public LocationDto() { }

        public LocationDto(ContextManager context, Guid? selectedLocation)
        {
            if (selectedLocation.HasValue)
            {
                SelectedItem = context.Get<Location>(selectedLocation.Value);
                if (SelectedItem.LocationType != null)
                {
                    LocationTypeId = SelectedItem.LocationType.ID;
                }
                if (SelectedItem.Map != null)
                {
                    MapId = SelectedItem.Map.ID;
                }
                History = context.GetHistory(typeof(Location), selectedLocation.Value);

            }
            else
            {
                History = new List<History>();
            }
            Locations = context.Context.Locations.ToList().Cast<ElementId>().ToList();
            Locations.Insert(0, new ElementId { ID = Guid.Empty, Name = "N/A" });

            LocationTypes = context.Context.LocationTypes.OrderBy(x => x.Name).ToList().Cast<ElementId>().ToList();
            LocationTypes.Insert(0, new ElementId { ID = Guid.Empty, Name = "--Non--" });

            Maps = context.Context.Maps.OrderBy(x => x.Name).ToList().Cast<ElementId>().ToList();
            Maps.Insert(0, new ElementId { ID = Guid.Empty, Name = "--Non--" });


        }

        public Location SelectedItem { get; set; }
        public Guid? LocationTypeId { get; set; }
        public Guid? MapId { get; set; }
        public List<ElementId> Locations { get; set; }
        public List<ElementId> LocationTypes { get; set; }
        public List<ElementId> Maps { get; set; }
        public List<History> History { get; set; }
    }
}