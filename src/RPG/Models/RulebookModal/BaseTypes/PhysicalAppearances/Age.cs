using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Races;


namespace RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances
{
    public enum AgeCategory
    {
        Young,
        MiddleAge,
        Old,
        Venerable,
    }
    [ComplexType]
    public class Age
    {
        public int CurrentAge { get; set; }
        
        
        //public AgeCategory AgeCategory { get; private set; }
        
        //public List<Bonus> AgeBonuses { get; set; }

        //public Age(List<Ability> abilities, Race race, int age)
        //{
        //    AgeCategory = race.AgeCategoryAtAge.Where(x => x.Age <= age).OrderByDescending(x => x.Age).First().Category;

        //    var physicalModifier = GetPhysicalModifier();
        //    var mentalModifier = GetMentalModifier();
        //    /*var str = Tools.GetAbilityFromKey(abilities, "abi_strength");
        //    var dex = Tools.GetAbilityFromKey(abilities, "abi_dexterity");
        //    var con = Tools.GetAbilityFromKey(abilities, "abi_constitution");
        //    var intel = Tools.GetAbilityFromKey(abilities, "abi_intelligence");
        //    var wis = Tools.GetAbilityFromKey(abilities, "abi_wisdom");
        //    var cha = Tools.GetAbilityFromKey(abilities, "abi_charisma");
        //    str.Bonus.Add(new Bonus(Guid.NewGuid(), "Age",null,"", "abi_strength", new BonusToAdd(physicalModifier,null,null,null,null,null), BonusType.AgeModifier,null));
        //    dex.Bonus.Add(new Bonus(Guid.NewGuid(), "Age", null, "", "abi_dexterity", new BonusToAdd(physicalModifier, null, null, null, null, null), BonusType.AgeModifier, null));
        //    con.Bonus.Add(new Bonus(Guid.NewGuid(), "Age", null, "", "abi_constitution", new BonusToAdd(physicalModifier, null, null, null, null, null), BonusType.AgeModifier, null));
        //    intel.Bonus.Add(new Bonus(Guid.NewGuid(), "Age", null, "", "abi_intelligence", new BonusToAdd(mentalModifier, null, null, null, null, null), BonusType.AgeModifier, null));
        //    wis.Bonus.Add(new Bonus(Guid.NewGuid(), "Age", null, "", "abi_wisdom", new BonusToAdd(mentalModifier, null, null, null, null, null), BonusType.AgeModifier, null));
        //    cha.Bonus.Add(new Bonus(Guid.NewGuid(), "Age", null, "", "abi_charisma", new BonusToAdd(mentalModifier, null, null, null, null, null), BonusType.AgeModifier, null));
        //    */

        //}
        public List<Bonus> GetAgeBonuses(Race race, List<AbilityScore> abilities)
        {
            var bonues = new List<Bonus>();
            var category = GetCurrentAgeCategory(race);
            foreach (var ability in abilities)
            {
                var currentBonus = new Bonus
                {
                    ApplyTo = new BonusApplyTo
                    {
                        ApplyToGuid = ability.Ability.ID,
                        ApplyToAll = false,
                        ApplyToType = BonusApplyToType.Ability
                    },
                    BonusValue = new BonusToAdd
                    {
                        FixedValue = 0,
                    }
                };
                if (ability.Ability.IsPhycicalStat)
                {
                    currentBonus.BonusValue.FixedValue = GetPhysicalModifier(category);
                }
                else
                {
                    currentBonus.BonusValue.FixedValue = GetMentalModifier(category);
                }
                bonues.Add(currentBonus);
            }

            return bonues;
        }

        public AgeCategory GetCurrentAgeCategory(Race race)
        {
            return race.GetAgeCategoryFromAge(CurrentAge);
        }

        private int GetPhysicalModifier(AgeCategory age)
        {
            switch (age)
            {
                case AgeCategory.Young:
                    return 0;
                case AgeCategory.MiddleAge:
                    return -1;
                case AgeCategory.Old:
                    return -3;
                case AgeCategory.Venerable:
                    return -6;
                default:
                    throw new NotSupportedException("Age not supported");
            }
        }

        private int GetMentalModifier(AgeCategory age)
        {
            switch (age)
            {
                case AgeCategory.Young:
                    return 0;
                case AgeCategory.MiddleAge:
                    return 1;
                case AgeCategory.Old:
                    return 2;
                case AgeCategory.Venerable:
                    return 3;
                default:
                    throw new NotSupportedException("Age not supported");
            }
        }
    }
}
