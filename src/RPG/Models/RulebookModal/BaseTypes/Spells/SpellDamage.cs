using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellDamage : GameId
    {
        public List<DamageByCasterLevel> ByCasterLevel { get; set; }
        public Damage Damage { get; set; }
        
        public DiceRoll GetDamage(int casterLevel)
        {
            if (Damage != null && Damage.Amount != null)
            {
                return Damage.Amount;
            }
            else
            {
                return ByCasterLevel.Where(x => x.AvailableAtLevel <= casterLevel).OrderByDescending(x => x.AvailableAtLevel).First().Damage.Amount;
            }
        }
    }
}
