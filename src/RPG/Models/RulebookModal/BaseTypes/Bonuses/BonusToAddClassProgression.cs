using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Dice;

namespace RPG.Models.RulebookModal.BaseTypes.Bonuses
{
    public class BonusToAddClassProgression : GameId
    {
        public ClassBase ClassProgression { get; set; }
        public int AtLevel { get; set; }
        public DiceRoll Bonues { get; set; }
    }
}