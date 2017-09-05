using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;

namespace RPG.Models.RulebookModal.BaseTypes.Saves
{
    public enum BaseBonusRate
    {
        Good = 3,
        Average = 2,
        Poor = 1,
    }
    public class SaveRate : GameId
    {
        public ClassBase Class { get; set; }
        public SaveType Save { get; set; }
        public BaseBonusRate Rate { get; set; }
    }
}