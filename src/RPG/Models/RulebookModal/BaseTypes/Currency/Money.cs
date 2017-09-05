using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace RPG.Models.RulebookModal.BaseTypes.Currency
{
    [ComplexType]
    public class Money
    {
        public int? CopperPrice { get; set; }

        public override string ToString()
        {
            var isNegativ = CopperPrice.GetValueOrDefault() < 0;
            var absCopper = Math.Abs(CopperPrice.GetValueOrDefault());

            var copper = absCopper% 10;
            var gold = (absCopper / 10) % 10;
            var platin = (absCopper / 100);

            var price = "";
            if (platin != 0)
            {
                price += platin +"pp";
            }
            if (platin != 0 || gold != 0)
            {
                if (price != "")
                {
                    price += " ";
                }
                price += gold + "gp";
            }
            if (platin != 0 || gold != 0 || copper != 0)
            {
                if (price != "")
                {
                    price += " ";
                }
                price += copper + "cp";
            }

            if (isNegativ)
            {
                price = "-" + price;
            }
            return price;
        }

        public void AddFromString(string money)
        {
            if (string.IsNullOrWhiteSpace(money))
            {
                return;
            }
            if (!CopperPrice.HasValue)
            {
                CopperPrice = 0;
            }
            var copperToAdd = StringToCopper(money);
            CopperPrice += copperToAdd;
        }
        public void RemoveFromString(string money)
        {
            if (string.IsNullOrWhiteSpace(money))
            {
                return;
            }
            if (!CopperPrice.HasValue)
            {
                CopperPrice = 0;
            }
            var copperToAdd = StringToCopper(money);
            CopperPrice -= copperToAdd;
        }

        private static int StringToCopper(string money)
        {
            var copperToAdd = 0;

            money = Regex.Replace(money.TrimEnd(new char[] {' ', '\r', '\n', '\t'}), "[^cgp0123456789]", string.Empty,
                RegexOptions.IgnoreCase);
            var remaingString = "";
            var ppParts = money.Split(new string[] {"pp"}, StringSplitOptions.RemoveEmptyEntries);
            if (ppParts.Length > 1)
            {
                copperToAdd += int.Parse(ppParts[0])*100;
                remaingString = ppParts[1];
            }
            else if (ppParts.Length == 1 && (!ppParts[0].Contains('c') && !ppParts[0].Contains('g')))
            {
                copperToAdd += int.Parse(ppParts[0]) * 100;
                remaingString = string.Empty;
            }
            else
            {
                remaingString = ppParts.LastOrDefault() ?? "";
            }

            var gpParts = remaingString.Split(new string[] {"gp"}, StringSplitOptions.RemoveEmptyEntries);
            if (gpParts.Length > 1)
            {
                copperToAdd += int.Parse(gpParts[0])*10;
                remaingString = gpParts[1];
            }
            else if (gpParts.Length == 1 && (!gpParts[0].Contains('c')))
            {
                copperToAdd += int.Parse(gpParts[0]) * 10;
                remaingString = string.Empty;
            }
            else
            {
                remaingString = gpParts.LastOrDefault() ?? "";
            }
            var cpParts = remaingString.Split(new string[] {"cp"}, StringSplitOptions.RemoveEmptyEntries);
            if (cpParts.Length >= 1)
            {
                copperToAdd += int.Parse(cpParts[0]);
            }
            return copperToAdd;
        }
    }
}
