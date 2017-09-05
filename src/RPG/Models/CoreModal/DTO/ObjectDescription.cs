using System;

namespace RPG.Models.CoreModal.DTO
{
    public class ObjectDescription
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }


        public static int SortAsAbilities(ObjectDescription obj)
        {
            if (obj == null)
            {
                return int.MaxValue;
            }
            return AbiToInt(obj.Name);
        }

        private static int AbiToInt(string name)
        {
            var toSwitch = name.ToLower().Substring(0, Math.Min(3, name.Length));
            switch (toSwitch)
            {
                case "str":
                    return 1;
                case "dex":
                    return 2;
                case "con":
                    return 3;
                case "int":
                    return 4;
                case "wis":
                    return 5;
                case "cha":
                    return 6;
                default:
                    return 7;
            }
        }
    }


}