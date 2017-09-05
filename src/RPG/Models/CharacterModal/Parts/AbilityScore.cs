using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;

namespace RPG.Models.CharacterModal.Parts
{
    public class AbilityScore : GameId
    {
        public Ability Ability { get; set; }
        public int? BaseValue { get; set; }
        /*
        public int GetScore(GetBonusDto bonusDto)
        {
            if (BaseValue.HasValue && !Bonus.Any())
            {
                return BaseValue.Value;
            }
            else if (BaseValue.HasValue)
            {
                var localBonus = new MaxBonusList<Bonus>();
                localBonus.AddRange(Bonus.Where(x => !x.IsTemporary()));

                return BaseValue.Value + localBonus.GetMaxValue(bonusDto);
            }
            return 0;
        }

        public int GetCurrent(GetBonusDto bonusDto)
        {
            return (GetScore(bonusDto) - 10) / 2;
        }
        public int? GetTempScore(GetBonusDto bonusDto)
        {
            if (Bonus.Any(x => x.IsTemporary() && x.IsActive()))
            {
                return (GetScore(bonusDto) + Bonus.Where(x => x.IsTemporary()).Sum(x => x.GetBonus(bonusDto)));
            }
            return null;
        }

        public int? GetTempModifier(GetBonusDto bonusDto)
        {
            var tempScore = GetTempScore(bonusDto);
            if (!tempScore.HasValue)
            {
                return null;
            }
            return (tempScore.Value - 10) / 2;
        }

        public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<AbilityScore> abilities)
        {
            Bonus.ChangeTime(currentSpecialAbilities, unit, abilities);
            return false;
        }
        
        public void AddBonues(List<Bonus> bonues)
        {
            foreach (var bonus in bonues)
            {
                if (bonus.ShouldApplyTo(Ability))
                {
                    Bonus.Add(bonus);
                }
            }
        }

        public void RemoveBonues(List<Bonus> bonues)
        {
            if (bonues == null)
            {
                return;
            }
            foreach (var bonus in bonues)
            {
                Bonus.RemoveAll(x => x.ID == bonus.ID);
            }
        }

        public int GetCurrentModifier(GetBonusDto bonusDto)
        {
            var tempBonus = GetTempModifier(bonusDto);
            if (tempBonus.HasValue)
            {
                return tempBonus.Value;
            }
            return GetCurrent(bonusDto);
        }
        public int GetScoreValue(GetBonusDto bonusDto)
        {
            var tempValue = GetTempScore(bonusDto);
            if (tempValue.HasValue)
            {
                return tempValue.Value;
            }
            return GetScore(bonusDto);
        }*/

        public CalculatedString GetCurrent(GetBonusDto bonusDto)
        {
            var activeBonuses = Tools.GetBonusesApplyingTo(Ability.ID, BonusApplyToType.Ability, bonusDto.ActiveBonus.ToList());
            var passiveBonuses = Tools.GetBonusesApplyingTo(Ability.ID, BonusApplyToType.Ability, bonusDto.PassiveBonus.ToList());
            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero(Ability.Name,null, BaseValue.GetValueOrDefault());

            calcStr.AddPartsByRef(passiveBonuses, bonusDto);
            calcStr.AddPartsByRef(activeBonuses,bonusDto);
            return calcStr;
        }

        public CalculatedString GetBase(GetBonusDto bonusDto)
        {
            var passiveBonuses = Tools.GetBonusesApplyingTo(Ability.ID, BonusApplyToType.Ability, bonusDto.PassiveBonus.ToList());
            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero(Ability.Name, null, BaseValue.GetValueOrDefault());

            calcStr.AddPartsByRef(passiveBonuses, bonusDto);
            return calcStr;
        }

        public int GetCurrentModifier(GetBonusDto bonusDto)
        {
            var activeBonuses = Tools.GetBonusesApplyingTo(Ability.ID, BonusApplyToType.Ability, bonusDto.ActiveBonus.ToList());
            var passiveBonuses = Tools.GetBonusesApplyingTo(Ability.ID, BonusApplyToType.Ability, bonusDto.PassiveBonus.ToList());
            //var activeBonusValue = activeBonuses.Sum(x => x.GetBonus(bonusDto).GetFixedAmount());
            //var passiveBonusValue = passiveBonuses.Sum(x => x.GetBonus(bonusDto).GetFixedAmount());

            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("Base", null, BaseValue.GetValueOrDefault());
            calcStr.AddPartsByRef(activeBonuses,bonusDto);
            calcStr.AddPartsByRef(passiveBonuses,bonusDto);
            var newBase = calcStr.GetValueAsInt();

            return (newBase - 10) / 2;
        }
        public int GetBaseModifier(GetBonusDto bonusDto)
        {
            var passiveBonuses = Tools.GetBonusesApplyingTo(Ability.ID, BonusApplyToType.Ability, bonusDto.PassiveBonus.ToList());
            //var passiveBonusValue = passiveBonuses.Sum(x => x.GetBonus(bonusDto).GetFixedAmount());

            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("Base", null, BaseValue.GetValueOrDefault());
            calcStr.AddPartsByRef(passiveBonuses, bonusDto);
            var newBase = calcStr.GetValueAsInt();

            return (newBase - 10) / 2;
        }
    }
}