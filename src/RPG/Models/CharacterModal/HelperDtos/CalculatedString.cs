using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RPG.Models.CoreModal.DTO;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class CalculatedString
    {
        public CalculatedString()
        {
            Parts = new List<KeyValuePair<string, DiceRoll>>();
            Circumstances = new List<KeyValuePair<string, DiceRoll>>();
        }
        
        public List<KeyValuePair<string, DiceRoll>> Parts { get; set; }
        public List<KeyValuePair<string, DiceRoll>> Circumstances { get; set; }


        public void AddIfNotZero(string name, string condition, DiceRoll value)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                AddIfNotZero(name, value);
            }
            else
            {
                AddCircumstancesIfNotZero(name,condition,value);
            }
        }
        private void AddIfNotZero(string name, DiceRoll value)
        {
            if (value.GetFixedAmount() != 0 || (value.Dice != null && value.Dice.Any()))
            {
                Parts.Add(new KeyValuePair<string, DiceRoll>(name, value));
            }
        }
        private void AddCircumstancesIfNotZero(string name, string condition, DiceRoll value)
        {
            if (value.GetFixedAmount() != 0 || (value.Dice != null && value.Dice.Any()))
            {
                var description = string.Format("Condition: '{0}' ({1})", condition, name);
                Circumstances.Add(new KeyValuePair<string, DiceRoll>(description, value));
            }
        }
        public void AddIfNotZero(string name, string condition, int value)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                AddIfNotZero(name, value);
            }
            else
            {
                AddCircumstancesIfNotZero(name, condition, value);
            }
        }

        private void AddIfNotZero(string name, int value)
        {
            if (value != 0)
            {
                Parts.Add(new KeyValuePair<string, DiceRoll>(name, new DiceRoll
                {
                    FixedAmount = value,
                    Dice = new List<Die>()
                }));
            }
        }
        private void AddCircumstancesIfNotZero(string name, string condition, int value)
        {
            if (value != 0)
            {
                var description = string.Format("Condition: '{0}' ({1})", condition, name);
                Circumstances.Add(new KeyValuePair<string, DiceRoll>(description, new DiceRoll
                {
                    FixedAmount = value,
                    Dice = new List<Die>()
                }));
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
                        AddIfNotZero(bonuesValue.Parent.Name,bonuesValue.GetCondition(), new DiceRoll
                        {
                            FixedAmount = bonuesValue.GetBonus(dto).GetFixedAmount()
                        });
                    }
                }
                else
                {
                    var bonuesValues = grp.Select(x => new
                    {
                        Name = x.Parent.Name,
                        Value = x.GetBonus(dto).GetFixedAmount(),
                        Condition = x.Bonues.GetCondition()
                    }).ToList();
                    var min = bonuesValues.OrderByDescending(x => x.Value).First();
                    var max = bonuesValues.OrderBy(x => x.Value).First();
                    if (min.Value < 0 && max.Value > 0)
                    {
                        AddIfNotZero(max.Name,max.Condition, new DiceRoll
                        {
                            FixedAmount = max.Value
                        });
                        AddIfNotZero(min.Name,min.Condition, new DiceRoll
                        {
                            FixedAmount = min.Value
                        });
                        //value += max + min;
                    }
                    else if (min.Value < 0 && max.Value < 0)
                    {
                        AddIfNotZero(min.Name, min.Condition, new DiceRoll
                        {
                            FixedAmount = min.Value
                        });
                        //value += min;
                    }
                    //prefer bonus without conditions
                    else if (string.IsNullOrWhiteSpace(max.Condition))
                    {
                        AddIfNotZero(max.Name, max.Condition, new DiceRoll
                        {
                            FixedAmount = max.Value
                        });
                        //value += max;
                    }
                    else
                    {
                        AddIfNotZero(min.Name, min.Condition, new DiceRoll
                        {
                            FixedAmount = min.Value
                        });
                        //value += max;
                    }
                }
            }
        }
        public int GetValueAsInt()
        {
            return Parts.Sum(x => x.Value.GetFixedAmount());
        }

        public string GetValue()
        {
            var partsSum = new DiceRoll();
            var conditionSum = new DiceRoll();
            foreach (var diceRoll in Parts)
            {
                partsSum += diceRoll.Value;
                conditionSum += diceRoll.Value;
            }

            var partStr = partsSum.ToString();
            partStr = string.IsNullOrWhiteSpace(partStr) ? "0" : partStr;
            if (!Circumstances.Any())
            {
                return string.Format("{0}", partStr);
            }

            foreach (var diceRoll in Circumstances)
            {
                conditionSum += diceRoll.Value;
            }

            var conditionSumStr = conditionSum.ToString();
            conditionSumStr = string.IsNullOrWhiteSpace(conditionSumStr) ? "0" : conditionSumStr;
            return string.Format("{0} ({1})", partStr, conditionSumStr);
        }

        public string GetCalculation()
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var part in Parts)
            {
                var partStr = part.Value.ToString();
                if (!partStr.StartsWith("-") && !first)
                {
                    partStr = "+" + partStr;
                }
                builder.AppendFormat("{0}({1})", partStr, part.Key);
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
                var partStr = part.Value.ToString();
                if (!partStr.StartsWith("-") && !first)
                {
                    partStr = "+" + partStr;
                }
                builder.AppendFormat("{0}({1})", partStr, part.Key);
                first = false;
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            return GetCalculation();
        }
    }
}