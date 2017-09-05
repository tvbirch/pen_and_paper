using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.CoreModal.Comparer;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Rounds;


namespace RPG.Models.RulebookModal
{
    public static class Tools
    {
        public static List<Bonus> GetBonusesApplyingTo(Guid applyTo, BonusApplyToType type, List<Bonus> bonuses)
        {
            return
                bonuses.Where(
                    x =>
                        x.ApplyTo != null && x.ApplyTo.ApplyToType == type &&
                        (x.ApplyTo.ApplyToAll || x.ApplyTo.ApplyToGuid == applyTo)).ToList();

        }

        public static List<BonusRef> GetBonusesApplyingTo(Guid applyTo, BonusApplyToType type, List<BonusRef> bonuses)
        {
            return
                bonuses.Where(
                    x =>
                        x.Bonues.ApplyTo != null && x.Bonues.ApplyTo.ApplyToType == type &&
                        (x.Bonues.ApplyTo.ApplyToAll || x.Bonues.ApplyTo.ApplyToGuid == applyTo)).ToList();

        }

        private static SelectList EnumASelectList(Type enumType, bool addSpacesToSentence = true, bool orderByText = true, bool orderByValue = false, string addNullOptionWithText = null, bool duplicateWeaponHand = false)
        {
            Array enumValues = Enum.GetValues(enumType);
            var list = new List<SelectListItem>();
            foreach (var enumValue in enumValues)
            {
                var enumText = Enum.GetName(enumType, enumValue);
                list.Add(new SelectListItem
                {
                    Text = addSpacesToSentence ? AddSpacesToSentence(enumText, false) : enumText,
                    Value = ((int)enumValue).ToString()
                });
                if (duplicateWeaponHand && enumText == "WeaponHand")
                {
                    list.Add(new SelectListItem
                    {
                        Text = addSpacesToSentence ? AddSpacesToSentence(enumText, false) : enumText + "2",
                        Value = "-1"
                    });
                }
            }
            if (!string.IsNullOrEmpty(addNullOptionWithText))
            {
                list.Add(new SelectListItem
                {
                    Text = addNullOptionWithText,
                    Value = null,
                });
            }
            if (orderByText)
            {
                list = list.OrderBy(x => x.Text).ToList();
            }
            if (orderByValue)
            {
                list = list.OrderBy(x => x.Value, new SemiNumericComparer()).ToList();
            }
            return new SelectList(list, "Value", "Text");
        }
        private static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
        public static SelectList DamageTypeAsSelectList()
        {
            return EnumASelectList(typeof(DamageType),false,false);
        }
        public static SelectList BonusTypeAsSelectList()
        {
            return EnumASelectList(typeof(BonusType));
        }

        public static SelectList BonusApplyToTypeAsSelectList()
        {
            return EnumASelectList(typeof(BonusApplyToType));
        }

        public static SelectList SizeCategoryAsSelectList()
        {
            return EnumASelectList(typeof(SizeCategory),true,false,true);
        }

        public static SelectList RoundActionAsSelectList()
        {
            return EnumASelectList(typeof(RoundAction), true, false, true, "Allways active");
        }

        public static SelectList TimeLimitUnitAsSelectList()
        {
            return EnumASelectList(typeof(TimeLimitUnit), true, false, true, "Never resets");
        }

        public static SelectList DurationUnitAsSelectList()
        {
            return EnumASelectList(typeof(DurationUnit), true, false, true, "No duration");
        }

        public static SelectList ItemTypeAsSelectList()
        {
            return EnumASelectList(typeof(ItemType),true,false,true);
        }

        public static SelectList ItemSlotRequirementAsSelectList()
        {
            return EnumASelectList(typeof(ItemSlotRequirement),duplicateWeaponHand:true);
        }

        public static SelectList HitDieAsSelectList()
        {
            return EnumASelectList(typeof(DieType),false,false,true);
        }

        public static SelectList BaseBonusRateAsSelectList()
        {
            return EnumASelectList(typeof(BaseBonusRate));
        }

        public static SelectList AgeCategoryAsSelectList()
        {
            return EnumASelectList(typeof(AgeCategory));
        }

        public static SelectList CastingTimeAsSelectList()
        {
            return EnumASelectList(typeof(CastingTime));
        }

        public static SelectList SpellRangeAsSelectList()
        {
            return EnumASelectList(typeof(SpellBaseRange),true,true,false,"Custom");
        }

        public static SelectList AutoApplyLimitAsSelectList()
        {
            return EnumASelectList(typeof(AutoApplyLimit), true, true, false);
        }
    }
}
