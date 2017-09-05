using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.GmModal.World
{
    public class LocationType : ElementId
    {
        public ICollection<Location> Locations { get; set; }
    }
}