using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Dice;

namespace RPG.Models.RulebookModal.BaseTypes.SpecialAbilities
{
    public class BonusFromCharges : GameId
    {
        public int NumberOfChargesForBonus { get; set; }
        public DiceRoll Bonus { get; set; }
    }
}