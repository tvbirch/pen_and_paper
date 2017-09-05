/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.CoreModal.DTO;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class CalculatedString
    {
        public CalculatedString()
        {
            Parts = new List<KeyValuePair<string, int>>();
            Circumstances = new List<KeyValuePair<string, int>>();
        }
        public CalculatedString(GetBonusDto bonusDto, AbilityScore abilityScore) : this()
        {
            AddIfNotZero(abilityScore.Ability.Name, abilityScore.GetCurrentModifier(bonusDto));
        }
        public CalculatedString(GetBonusDto bonusDto, AbilityScore abilityScore, List<BonusRef> bonusRefs)
            : this(bonusDto, abilityScore)
        {
            AddPartsByRef(bonusRefs, bonusDto);
        }
        public CalculatedString(GetBonusDto bonusDto, AbilityScore abilityScore, int ranks, List<BonusRef> bonusRefs) : this(bonusDto,abilityScore)
        {
            AddIfNotZero("Ranks", ranks);
            AddPartsByRef(bonusRefs, bonusDto);
        }
        public void AddIfNotZero(string name, int value)
        {
            if (value != 0)
            {
                Parts.Add(new KeyValuePair<string, int>(name, value));
            }
        }
        public void AddCircumstancesIfNotZero(string name, string condition, int value)
        {
            if (value != 0)
            {
                var description = string.Format("Condition: '{0}' ({1})", condition, name);
                Circumstances.Add(new KeyValuePair<string, int>(description, value));
            }
        }

        public void AddPartsByRef(List<BonusRef> bonusRefs, GetBonusDto dto)
        {
            if (bonusRefs == null || bonusRefs.Count == 0)
            {
                return;
            }
            var grps = bonusRefs.GroupBy(x => x.Type);
            foreach (var grp in grps)
            {
                var stacks = LinqExtension.DoesStack(grp.Key);
                if (stacks)
                {
                    //value = bonuesValues.Sum();
                    //var bonuesValues = grp.Select(x => x.GetBonus(dto).GetValueOrDefault()).ToList();
                    foreach (var bonuesValue in grp)
                    {
                        AddIfNotZero(bonuesValue.Parent.Name,bonuesValue.GetBonus(dto).GetValueOrDefault());
                    }
                }
                else
                {
                    var bonuesValues = grp.Select(x => new
                    {
                        Name = x.Parent.Name,
                        Value = x.GetBonus(dto).GetValueOrDefault(),
                    }).ToList();
                    var min = bonuesValues.OrderByDescending(x => x.Value).First();
                    var max = bonuesValues.OrderBy(x => x.Value).First();
                    if (min.Value < 0 && max.Value > 0)
                    {
                        AddIfNotZero(max.Name, max.Value);
                        AddIfNotZero(min.Name, min.Value);
                        //value += max + min;
                    }
                    else if (min.Value < 0 && max.Value < 0)
                    {
                        AddIfNotZero(min.Name, min.Value);
                        //value += min;
                    }
                    else
                    {
                        AddIfNotZero(max.Name, max.Value);
                        //value += max;
                    }
                }
            }
        }
        public List<KeyValuePair<string, int>> Parts { get; set; }
        public List<KeyValuePair<string, int>> Circumstances { get; set; }
        

        public int GetValueAsInt()
        {
            return Parts.Sum(x => x.Value);
        }
        public string GetValue()
        {
            var partsSum = Parts.Sum(x => x.Value);
            if (Circumstances.Count == 0)
            {
                return partsSum.ToString();
            }
            var conditionSum = Circumstances.Sum(x => x.Value) + partsSum;
            return string.Format("{0} ({1})", partsSum, conditionSum);
        }

        public string GetCalculation()
        {
            var builder = new StringBuilder();

            var first = true;
            foreach (var part in Parts)
            {

                if (!first)
                {
                    if (part.Value >= 0)
                    {
                        builder.Append(" + ");
                    }
                }
                builder.AppendFormat("{0}({1})", part.Value.ToString().Replace("-"," - "), part.Key);
                first = false;
            }

            first = true;
            foreach (var part in Circumstances)
            {
                if (first)
                {
                    builder.AppendLine("<br>");
                    builder.AppendLine("Circumstantial bonueses:");
                    builder.AppendLine("<br>");
                }

                if (!first)
                {
                    if (part.Value < 0)
                    {
                        builder.Append(" - ");
                    }
                    else
                    {
                        builder.Append(" + ");
                    }
                }
                if (first && part.Value < 0)
                {
                    builder.Append("-");
                }

                builder.AppendFormat("{0}({1})", part.Value, part.Key);
                first = false;
            }
            return builder.ToString();
        }
    }

}*/