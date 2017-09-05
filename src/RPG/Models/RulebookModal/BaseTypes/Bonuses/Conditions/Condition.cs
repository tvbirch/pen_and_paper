using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;


namespace RPG.Models.RulebookModal.BaseTypes.Bonuses.Conditions
{
    public class Condition
    {
        public ConditionPretens ConditionPretensForBonus { get; private set; }
        
        public List<SpecialAbility> SpecialAbillitiesForBonus { get; private set; }
        public Ability AbillitiesForBonus { get; private set; }
        public bool? TrapsBonus { get; set; }

        public Condition(List<SpecialAbility> specialAbillitiesForBonus, ConditionPretens conditionPretens)
        {
            ConditionPretensForBonus = conditionPretens;
            SpecialAbillitiesForBonus = specialAbillitiesForBonus;
        }

        public Condition(Ability abilityScore, ConditionPretens conditionPretens)
        {
            ConditionPretensForBonus = conditionPretens;
            AbillitiesForBonus = abilityScore;
        }
        public Condition(bool traps, ConditionPretens conditionPretens)
        {
            ConditionPretensForBonus = conditionPretens;
            TrapsBonus = traps;
        }

        private Condition(List<SpecialAbility> specialAbillitiesForBonus, Ability abilityScore, bool? traps, ConditionPretens conditionPretens)
        {
            ConditionPretensForBonus = conditionPretens;
            SpecialAbillitiesForBonus = specialAbillitiesForBonus;
            AbillitiesForBonus = abilityScore;
            TrapsBonus = traps;
        }

        public bool IsConditionMeet(GetBonusDto bonusDto)
        {
            if (SpecialAbillitiesForBonus == null && AbillitiesForBonus == null && TrapsBonus == null) return false;
            if (TrapsBonus.HasValue)
            {
                return TrapsBonus.Value;
            }
            
            if (AbillitiesForBonus != null)
            {
                var modifier = bonusDto.Abilities.First(x => x.Ability.ID == AbillitiesForBonus.ID).GetCurrent(bonusDto);
                switch (ConditionPretensForBonus)
                {
                    case ConditionPretens.LargerThenZero:
                        return modifier.GetValueAsInt() > 0;
                    default:
                        throw new NotImplementedException();
                }
            }
            
            return true;
        }

    }
}
