using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Dice
{
    public class Die : GameId
    {
        public MathOperator? Operator { get; set; }
        public int? NumberOfDice { get; set; }
        public int? DieType { get; set; }

        public static List<Die> ConcatDice(List<Die> dice)
        {
            var finalList = new List<Die>();
            var grouped = dice.Select(x => new Die
            {
                DieType = x.DieType,
                NumberOfDice = x.NumberOfDice,
                Operator = x.Operator.HasValue ? x.Operator.Value : MathOperator.Plus,
                ID = x.ID
            }).GroupBy(d => new {d.DieType, d.Operator});
            var summed = grouped.Select(diegrp => new Die
            {
                NumberOfDice = diegrp.Sum(x => x.NumberOfDice), 
                DieType = diegrp.Key.DieType,
                Operator = diegrp.Key.Operator
            }).ToList();

            var summedGrouped = summed.GroupBy(x => x.DieType);
            foreach (var grp in summedGrouped)
            {
                var currentDie = new Die
                {
                    NumberOfDice = 0,
                };
                foreach (var die in grp)
                {
                    currentDie.DieType = die.DieType;
                    switch (die.Operator)
                    {
                        case MathOperator.Plus:
                            currentDie.NumberOfDice += die.NumberOfDice;
                            break;
                        case MathOperator.Minus:
                            currentDie.NumberOfDice -= die.NumberOfDice;
                            break;
                        default:
                            throw new NotImplementedException("Operator not supported.");
                    }
                }
                finalList.Add(currentDie);
            }
            return finalList;
        }

        public override string ToString()
        {
            return string.Format("{0}d{1}", NumberOfDice, DieType);
        }

        public static string ToDiceString(List<Die> dice)
        {
            var builder = new StringBuilder();
            var newDice = ConcatDice(dice);
            foreach (var die in newDice)
            {
                builder.AppendFormat("{0} + ", die.ToString());
            }
            if (builder.Length < 3)
                return string.Empty;
            builder.Length = builder.Length - 3;
            return builder.ToString();
        }
    }
}
