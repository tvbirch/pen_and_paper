using System;
using System.ComponentModel.DataAnnotations.Schema;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Dice;

namespace RPG.Models.RulebookModal.BaseTypes.Damages
{
    public class Damage : GameId
    {
        public DiceRoll Amount { get; set; }
        public DamageType Type { get; set; }
    }
}
