using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;


namespace RPG.Models.RulebookModal.Lists
{
    public class MaxBonusList<T> : RoundableList<T> where T : Bonus, IRoundable
    {
        public bool ChangeUnit(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<AbilityScore> abilities)
        {
            if (Count != 0)
            {
                for (var i = Count -1 ; i >= 0; i--)
                {
                    var remove = this[i].ChangeTime(currentSpecialAbilities,unit, abilities);
                    if(remove)
                    {
                        this.RemoveAt(i);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the max bonus possible. This includes all stacable bonues + the highest non stacable.
        /// </summary>
        /// <param name="currentSpecialAbilities"></param>
        /// <returns></returns>
        public int GetMaxValue(GetBonusDto bonusDto, List<BonusType> notIncludeBonusTypes = null, bool includePenelties = false)
        {
            if (Count == 0) return 0;
            if (notIncludeBonusTypes == null)
            {
                var grouped = this.GroupBy(b => b.Type);
                return GetSumedBonus(bonusDto, grouped);
            }
            else
            {
                var grouped = this.Where(x => !notIncludeBonusTypes.Contains(x.Type))
                        .GroupBy(b => b.Type);
                return GetSumedBonus(bonusDto, grouped);
                
            }
        }

        private int GetSumedBonus(GetBonusDto bonusDto, IEnumerable<IGrouping<BonusType, T>> grouped)
        {
            var sum = 0;
            foreach (var grp in grouped)
            {
                if (!grp.Any())
                {
                    continue;
                }
                if (grp.Count() == 1)
                {
                    sum += grp.First().GetBonus(bonusDto).GetFixedAmount();
                }
                else
                {
                    var orderBonuses = grp.OrderByDescending((x => x.GetBonus(bonusDto).GetFixedAmount()));
                    var first = orderBonuses.First().GetBonus(bonusDto).GetFixedAmount();
                    var last = orderBonuses.Last().GetBonus(bonusDto).GetFixedAmount();
                    sum += first;
                    //If both bonuses and penelties exist, apply also largest penelty.
                    if (first > 0  && last < 0)
                    {
                        sum += last;
                    }
                }
            }

            return sum;
        }
    }
}
