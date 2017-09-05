using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Alligments;

namespace RPG.Models.GmModal.World
{
    public class NPC : ElementId
    {
        public string Profession { get; set; }
        public string Race { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string VoiceMannersPersonality { get; set; }
        public string RelationToParty { get; set; }
        public string CombatStatistics { get; set; }
        public string Affiliations { get; set; }
        public Alligment Alligment { get; set; }
        public Location Location { get; set; }
    }
}