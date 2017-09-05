using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.GmModal.World
{
    public class Location : ElementId
    {
        public LocationType LocationType { get; set; }
        public Map Map { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public int? Layer { get; set; }
        public string Population { get; set; }
        public string Governance { get; set; }
        public string ProsperityAndCrime { get; set; }
        public string Races { get; set; }
        public string Military { get; set; }
        public string SourceOfIncome { get; set; }
        public string SourceOfFood { get; set; }
        public string GroupsAndFactions { get; set; }
        public string FeaturesAndLandmarks { get; set; }
        public string NotableLocations { get; set; }


        public ICollection<NPC> NPCs { get; set; }
    }
}