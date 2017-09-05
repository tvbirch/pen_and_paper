using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class SpellInSlot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string School { get; set; }
        public string Components { get; set; }
        public string Time { get; set; }
        public string Range { get; set; }
        public string TargetEffectArea { get; set; }
        public string Duration { get; set; }
        public string Save { get; set; }
        public string SR { get; set; }

        public bool SlotSpend { get; set; }
        public bool CurrentlyUsable { get; set; }
    }
}