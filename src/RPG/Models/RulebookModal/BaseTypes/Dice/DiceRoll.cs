using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Dice
{
    public enum MathOperator
    {
        Minus = 0,
        Plus = 1,
    }
    public class DiceRoll : GameId
    {
        public int? FixedAmount { get; set; }
        public List<Die> Dice { get; set; }

        public void AddFixedAmount(int fixedAmount)
        {
            if (FixedAmount == null)
            {
                FixedAmount = 0;
            }
            FixedAmount += fixedAmount;
        }

        public void AddDie(Die die)
        {
            if (Dice == null)
            {
                Dice = new List<Die>();
            }
            Dice.Add(die);
        }

        public void AddDice(List<Die> dice)
        {
            if (Dice == null)
            {
                Dice = new List<Die>();
            }
            Dice.AddRange(dice);
        }
        private int GetFixed()
        {
            return FixedAmount.GetValueOrDefault();
        }

        public List<Die> GetDice()
        {
            return Dice;
        }

        public DiceRoll Clone()
        {
            var newDiceRoll = new DiceRoll
            {
                Dice = new List<Die>(),
                FixedAmount = FixedAmount,
            };
            foreach (var die in Dice)
            {
                newDiceRoll.AddDie(new Die
                {
                    DieType = die.DieType,
                    NumberOfDice = die.NumberOfDice,
                    Operator = die.Operator
                });
            }
            return newDiceRoll;
        }

        public static DiceRoll operator +(DiceRoll d1, DiceRoll d2)
        {
            var dice = d1.GetDice();
            if (dice == null)
            {
                dice = new List<Die>();
            
            }
            if (d2.Dice != null)
            {
                dice.AddRange(d2.GetDice());    
            }
            
            var diceAdded = Die.ConcatDice(dice);
            return new DiceRoll
            {
                Dice = diceAdded,
                FixedAmount = d1.GetFixed() + d2.GetFixed()
            };
        }


        public int GetFixedAmount()
        {
            return FixedAmount.GetValueOrDefault();
        }

        public override string ToString()
        {
            if (Dice == null)
            {
                Dice = new List<Die>();
            }
            var str = Dice.OrderByDescending(x => x.DieType).Aggregate("", (current, die) => current + (die.Operator.ToString() + die.ToString()));
            //removing + if its in the start of the string
            if (Dice.Any())
            {
                if (str.StartsWith("+"))
                {
                    str = str.Substring(1);
                }
            }
            if (FixedAmount > 0)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    str += FixedAmount;
                }
                else
                {
                    str += "+" + FixedAmount;
                }
            }
            else if (FixedAmount < 0)
            {
                str += FixedAmount;
            }
            
            return str;
        }

        public static DiceRoll Parse(string damageString)
        {
            if (string.IsNullOrWhiteSpace(damageString))
            {
                return null;
            }
            var newDiceRoll = new DiceRoll
            {
                Dice = new List<Die>(),
                FixedAmount = 0
            };
            var rgx = new Regex("[^dD0-9-+]");
            damageString = rgx.Replace(damageString, "");
            var currentPart = "";
            MathOperator? currentOperator = MathOperator.Plus;
            MathOperator? nextOperator = null;

            //Checking first char for operator, we assume + if there is none.
            if (damageString[0] == '+' || damageString[0] == '-')
            {
                if (damageString[0] == '-')
                {
                    currentOperator = MathOperator.Minus;
                }
                damageString = damageString.Substring(1);
            }

            for (var i = 0; i < damageString.Length; i++)
            {
                var currentChar = damageString[i];
                if (currentChar == '+')
                {
                    nextOperator = MathOperator.Plus;
                }
                else if (currentChar == '-')
                {
                    nextOperator = MathOperator.Minus;
                }
                else
                {
                    currentPart += currentChar;
                }

                if (nextOperator != null || i == damageString.Length - 1)
                {
                    //Adding part
                    if (currentPart.Contains("d"))
                    {
                        //Adding dice
                        var parts = currentPart.ToLower().Split('d');
                        newDiceRoll.Dice.Add(new Die
                        {
                            Operator = currentOperator.Value,
                            NumberOfDice = int.Parse(parts[0]),
                            DieType = int.Parse(parts[1])
                        });
                    }
                    else
                    {
                        //Changing fixed value
                        var currentValue = int.Parse(currentPart);
                        switch (currentOperator)
                        {
                            case MathOperator.Minus:
                                newDiceRoll.FixedAmount -= currentValue;
                                break;
                            case MathOperator.Plus:
                                newDiceRoll.FixedAmount += currentValue;
                                break;
                            default:
                                throw new NotImplementedException("Mathoperator not supported");
                        }
                    }

                    //resetting
                    currentOperator = nextOperator;
                    nextOperator = null;
                    currentPart = "";
                }
            }
            //grouping dice
            newDiceRoll.Dice = Die.ConcatDice(newDiceRoll.Dice);
            return newDiceRoll;
        }
    }
}
