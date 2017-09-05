using System;

namespace RPG.Models.CoreModal.Extensions
{
    public static class StringExtension
    {
        public static Guid? ToGuid(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return (Guid?)null;
            }
            else
            {
                return Guid.Parse(str);
            }
        }
        public static int? ToInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return (int?)null;
            }
            else
            {
                return int.Parse(str);
            }
        }

        public static decimal? ToDecimal(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return (int?)null;
            }
            else
            {
                return decimal.Parse(str);
            }
        }
    }
}
