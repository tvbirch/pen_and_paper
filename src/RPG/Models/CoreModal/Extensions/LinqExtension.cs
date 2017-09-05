using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;

namespace RPG.Models.CoreModal.Extensions
{
    public static class LinqExtension
    {
        public static int MaxBonuesSum(this IEnumerable<Bonus> bonues, GetBonusDto dto)
        {
            int value = 0;
            var grps = bonues.GroupBy(x => x.Type);
            foreach (var grp in grps)
            {
                var bonuesValues = grp.Select(x => x.GetBonus(dto)).ToList();
                var stacks = DoesStack(grp.Key);
                if (stacks)
                {
                    value = bonuesValues.Select(x => x.GetFixedAmount()).Sum();
                }
                else
                {
                    var min = bonuesValues.Select(x => x.GetFixedAmount()).Min();
                    var max = bonuesValues.Select(x => x.GetFixedAmount()).Max();
                    if (min < 0 && max > 0)
                    {
                        value += max + min;
                    }
                    else if (min < 0 && max < 0)
                    {
                        value += min;
                    }
                    else
                    {
                        value += max;
                    }
                }
            }
            return value;
        }
        public static int MaxBonuesSum(this IEnumerable<BonusRef> bonues, GetBonusDto dto)
        {
            int value = 0;
            var grps = bonues.GroupBy(x => x.Type);
            foreach (var grp in grps)
            {
                var bonuesValues = grp.Select(x => x.GetBonus(dto)).ToList();
                var stacks = DoesStack(grp.Key);
                if (stacks)
                {
                    value = bonuesValues.Select(x => x.GetFixedAmount()).Sum();
                }
                else
                {
                    var min = bonuesValues.Select(x => x.GetFixedAmount()).Min();
                    var max = bonuesValues.Select(x => x.GetFixedAmount()).Max();
                    if (min < 0 && max > 0)
                    {
                        value += max + min;
                    }
                    else if (min < 0 && max < 0)
                    {
                        value += min;
                    }
                    else
                    {
                        value += max;
                    }
                }
            }
            return value;
        }

        public static List<BonusRef> MaxBonusRefList(this IEnumerable<BonusRef> bonusRefs, GetBonusDto dto, bool forceNonStack = false)
        {
            var list = new List<BonusRef>();
            var bonuesList = bonusRefs as IList<BonusRef> ?? bonusRefs.ToList();
            if (bonusRefs == null || !bonuesList.Any())
            {
                return list;
            }
            var grps = bonuesList.GroupBy(x => x.Type);
            foreach (var grp in grps)
            {
                var stacks = DoesStack(grp.Key) && !forceNonStack;
                if (stacks)
                {
                    //value = bonuesValues.Sum();
                    //var bonuesValues = grp.Select(x => x.GetBonus(dto).GetValueOrDefault()).ToList();
                    list.AddRange(grp);
                }
                else
                {
                    var bonuesValues = grp.Select(x => new
                    {
                        Name = x.Parent.Name,
                        Value = x.GetBonus(dto),
                        org = x
                    }).ToList();
                    var min = bonuesValues.OrderByDescending(x => x.Value.GetFixedAmount()).First();
                    var max = bonuesValues.OrderBy(x => x.Value.GetFixedAmount()).First();
                    if (min.Value.GetFixedAmount() < 0 && max.Value.GetFixedAmount() > 0)
                    {
                        list.Add(min.org);
                        list.Add(max.org);
                        //value += max + min;
                    }
                    else if (min.Value.GetFixedAmount() < 0 && max.Value.GetFixedAmount() < 0)
                    {
                        list.Add(min.org);
                        //value += min;
                    }
                    else
                    {
                        list.Add(max.org);
                        //value += max;
                    }
                }
            }
            return list;
        }


        public static bool DoesStack(BonusType key)
        {
            return key == BonusType.DivineBonus || 
                key == BonusType.DodgeBonus ||
                key == BonusType.InsightBonus ||
                key == BonusType.NaturalBonus ||
                key == BonusType.SacredModifier;

        }
    }
}