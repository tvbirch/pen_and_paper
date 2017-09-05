using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;

namespace RPG.Models.RulebookModal.BaseTypes.Bonuses
{
    public class BonusToAdd : GameId
    {
        public DamageType? Against { get; set; }

        public int? FixedValue { get; set; }
        public Ability AbilityModifyer { get; set; }

        public List<BonusToAddClassProgression> ClassProgression { get; set; }

        public DiceRoll Dice { get; set; }
        
        public DiceRoll GetBonus(GetBonusDto bonusDto)
        {
            var diceRoll = new DiceRoll();
            var totalBonus = FixedValue.GetValueOrDefault();
            if (AbilityModifyer != null)
            {
                totalBonus += bonusDto.Abilities.First(x => x.Ability.ID == AbilityModifyer.ID).GetCurrentModifier(bonusDto);
            }
            else if (ClassProgression != null && ClassProgression.Any())
            {
                var progressions = ClassProgression.Where(x => x.AtLevel <= bonusDto.Classes.FirstOrDefault(y => y.Class.ID == x.ClassProgression.ID)?.Level).OrderByDescending(x => x.AtLevel).FirstOrDefault();
                if (progressions != null)
                {
                    diceRoll += progressions.Bonues;
                }
            }
            diceRoll.AddFixedAmount(totalBonus);
            if (Dice != null)
                diceRoll.AddDice(Dice.GetDice());

            return diceRoll;
        }

        public void SetFixedBonus(DiceRoll bonusFromCharge)
        {
            FixedValue = bonusFromCharge.GetFixedAmount();
            Dice = new DiceRoll
            {
                FixedAmount = 0, 
                Dice = bonusFromCharge.GetDice()
            };
        }

    }
}
