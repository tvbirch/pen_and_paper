using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;

namespace RPG.Models.RulebookModal.BaseTypes.Races
{
    public enum SizeCategory
    {
        Fine = -4,
        Diminutive = -3,
        Tiny = -2,
        Small = -1,
        Medium = 0,
        Large = 1,
        Huge = 2,
        Gargantuan = 3,
        Colossal = 4,
    }

    public class Race : ElementId
    {
        public SizeCategory Size { get; set; }
        public List<Bonus> RaceBonuses { get; set; }
        public List<Language> Languages { get; set; }
        public List<Language> BonusLanguages { get; set; }
        public List<ClassBase> FavoredClasses { get; set; }
        public List<AgeCategoryAtAge> AgeCategoryAtAge { get; set; }
        public List<SpecialAbility> RacialAbilities { get; set; }
        public int BaseSpeed { get; set; }

        public virtual ICollection<Character> Characters { get; set; }
        
        public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<Ability> abilities)
        {
            return false;
        }

        public int GetBaseSpeed()
        {
            return BaseSpeed;
        }

        public AgeCategory GetAgeCategoryFromAge(int age)
        {
            var category =AgeCategoryAtAge.Where(x => x.Age <= age).OrderByDescending(y => y.Age).FirstOrDefault();
            if (category == null)
            {
                return AgeCategory.Young;
            }
            return category.Category;
        }
        public SizeCategory GetCurrentSize(GetBonusDto dto)
        {
            var intSize = (int)Size;
            intSize +=
                dto.Bonuses.Where(
                    x => x.ShouldApplyTo(this, typeof (Race)) && x.ShouldApplyToSubType(Configuration.SizeId))
                    .Sum(x => x.GetBonus(dto).GetFixedAmount());
            var size = (SizeCategory)intSize;
            return size;
        }
        public string GetCurrentSizeString(GetBonusDto dto)
        {
            var size = GetCurrentSize(dto);
            return size.ToString();
        }

        public void EnrichPassiveBonusDto(GetBonusDto dto)
        {
            if (RaceBonuses != null)
            {
                foreach (var bonuse in RaceBonuses)
                {
                    dto.PassiveBonus.Add(new BonusRef(this, bonuse));
                }
                //dto.PassiveBonus.AddRange(RaceBonuses);
            }
            if (RacialAbilities != null)
            {
                foreach (var ability in RacialAbilities)
                {
                    ability.EnrichPassiveBonusDto(dto);
                }    
            }
            
        }

        public void EnrichActiveBonusDto(GetBonusDto dto)
        {
            if (RacialAbilities != null)
            {
                foreach (var ability in RacialAbilities)
                {
                    ability.EnrichActiveBonusDto(dto);
                }
            }
        }

        public int GetCurrentSpeed(GetBonusDto bonusDto)
        {
            return BaseSpeed +
                   bonusDto.Bonuses.Where(
                       x => x.ShouldApplyTo(this, typeof (Race)) && x.ShouldApplyToSubType(Configuration.SpeedId))
                       .Select(y => y.GetBonus(bonusDto).GetFixedAmount())
                       .Sum();
        }

        #region sizespecificstuff
        public int GetAttackAndAcModifier(GetBonusDto bonusDto)
        {
            var currentSize = (int)GetCurrentSize(bonusDto);
            if (currentSize == 0) return 0;
            switch ((SizeCategory)currentSize)
            {
                case SizeCategory.Fine:
                    return 8;
                case SizeCategory.Diminutive:
                    return 4;
                case SizeCategory.Tiny:
                    return 2;
                case SizeCategory.Small:
                    return 1;
                case SizeCategory.Medium:
                    return 0;
                case SizeCategory.Large:
                    return -1;
                case SizeCategory.Huge:
                    return -2;
                case SizeCategory.Gargantuan:
                    return -4;
                case SizeCategory.Colossal:
                    return -8;
            }
            throw new IndexOutOfRangeException("Size not handled");
        }

        public int GetSpecialAttackModifier(GetBonusDto bonusDto)
        {
            var currentSize = (int)GetCurrentSize(bonusDto);
            if (currentSize == 0) return 0;
            return 4 * currentSize;
        }
        public int GetHideModifier(GetBonusDto bonusDto)
        {
            var currentSize = (int)GetCurrentSize(bonusDto);
            if (currentSize == 0) return 0;
            return -4 * currentSize;
        }
        public double GetBipedCarryingCapacityMulitplier(GetBonusDto dto)
        {
            switch (GetCurrentSize(dto))
            {
                case SizeCategory.Fine:
                    return 0.125;
                case SizeCategory.Diminutive:
                    return 0.25;
                case SizeCategory.Tiny:
                    return 0.5;
                case SizeCategory.Small:
                    return 0.75;
                case SizeCategory.Medium:
                    return 1.0;
                case SizeCategory.Large:
                    return 2.0;
                case SizeCategory.Huge:
                    return 4.0;
                case SizeCategory.Gargantuan:
                    return 8.0;
                case SizeCategory.Colossal:
                    return 16.0;
            }
            throw new IndexOutOfRangeException("Size not handled");
        }
        public double GetQuadrupedCarryingCapacityMulitplier(GetBonusDto dto)
        {
            switch (GetCurrentSize(dto))
            {
                case SizeCategory.Fine:
                    return 0.25;
                case SizeCategory.Diminutive:
                    return 0.5;
                case SizeCategory.Tiny:
                    return 0.75;
                case SizeCategory.Small:
                    return 1.0;
                case SizeCategory.Medium:
                    return 1.5;
                case SizeCategory.Large:
                    return 3.0;
                case SizeCategory.Huge:
                    return 6.0;
                case SizeCategory.Gargantuan:
                    return 12.0;
                case SizeCategory.Colossal:
                    return 24.0;
            }
            throw new IndexOutOfRangeException("Size not handled");
        }
        #endregion
    }
}
