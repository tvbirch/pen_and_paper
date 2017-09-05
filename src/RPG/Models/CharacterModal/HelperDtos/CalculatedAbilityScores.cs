using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class CalculatedAbilityScores
    {
        public string Name { get; set; }
        public int BaseMod { get; set; }
        public CalculatedString Base { get; set; }
        public int CurrentMod { get; set; }
        public CalculatedString Current { get; set; }
        //public int BaseMod { get; set; }
        //public int Base { get; set; }
        //public int CurrentMod { get; set; }
        //public int Current { get; set; }
    }
}