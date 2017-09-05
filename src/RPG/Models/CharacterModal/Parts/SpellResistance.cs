using System.Collections.Generic;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;


namespace RPG.Models.CharacterModal.Parts
{
    public class SpellResistance : GameId
    {
        public MaxBonusList<Bonus> Bonuses { get; private set; }

        public SpellResistance()
        {
            ID = Configuration.SpellResistanceId;
        }

        public int GetBonusValue(GetBonusDto bonusDto)
        {
            return Bonuses.GetMaxValue(bonusDto);
        }

        public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<Ability> abilities)
        {
            return false;
        }

        public void AddBonues(List<Bonus> bonues)
        {
            foreach (var bonus in bonues)
            {
                if (bonus.ShouldApplyTo(this))
                {
                    Bonuses.Add(bonus);
                }
            }
        }
        public void RemoveBonues(List<Bonus> bonues)
        {
            foreach (var bonus in bonues)
            {
                Bonuses.RemoveAll(x => x.ID == bonus.ID);
            }
        }
    }
}
