using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class SearchSpell
    {
        public string id { get; set; }
        public string text { get; set; }
        public string Description { get; set; }
        public string CastingTime { get; set; }
        public string Components { get; set; }
        public string Range { get; set; }
        public string Save { get; set; }
        public string SpellResistance { get; set; }
        public string Target { get; set; }
    }
}