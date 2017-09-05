using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;

namespace RPG.Models.RulebookModal.BaseTypes.Time
{
    public class UsableAmountClassProgression : GameId
    {
        public ClassBase ClassProgression { get; set; }
        public int AtLevel { get; set; }
    }
}