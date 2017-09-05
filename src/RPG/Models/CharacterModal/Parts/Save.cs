using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;

namespace RPG.Models.CharacterModal.Parts
{
    [NotMapped]
    public class Save : GameId
    {
        public SaveType SaveType {get; set;}
        public string Name { get; set; }
        public string Description { get; set; }

        public CalculatedString SaveBonus { get; set; }

        public void AddBaseSave(string name, int level, BaseBonusRate rate)
        {
            switch (rate)
            {
                case BaseBonusRate.Good:
                    SaveBonus.AddIfNotZero(name, null, (level + 1 + 3) / 2);
                    return;
                case BaseBonusRate.Average:
                    throw new NotSupportedException("Unsupported bonus rate for savet");
                case BaseBonusRate.Poor:
                    SaveBonus.AddIfNotZero(name, null, (level) / 3);
                    return;
                default:
                    throw new NotSupportedException("Unsupported bonus rate for savet");
            }
        }
        public void SetBonuesToSave(GetBonusDto bonusDto)
        {
            var abilityScore = bonusDto.Abilities.First(x => x.Ability.ID == SaveType.AbilityModifier.ID);
            SaveBonus.AddIfNotZero(abilityScore.Ability.Name,null,abilityScore.GetCurrentModifier(bonusDto));
            var bonusToApplyToThis = bonusDto.Bonuses.Where(a => a.ShouldApplyTo(this) && a.ShouldApplyToSubType(null)).ToList();
            SaveBonus.AddPartsByRef(bonusToApplyToThis, bonusDto);
        }
        //public int BaseSave { get; set; }
        //public int AbilityScore { get; set; }
        //public int TempBonus { get; set; }
        //public int MiscBonus { get; set; }

        //public int TotalBonus
        //{
        //    get { return BaseSave + AbilityScore + TempBonus + MiscBonus; }
        //}

        //public void AddBaseSave(int level, BaseBonusRate rate)
        //{
        //    switch (rate)
        //    {
        //        case BaseBonusRate.Good:
        //            BaseSave += (level + 1 + 3) / 2;
        //            return;
        //        case BaseBonusRate.Average:
        //            throw new NotSupportedException("Unsupported bonus rate for savet");
        //        case BaseBonusRate.Poor:
        //            BaseSave += (level) / 3;
        //            return;
        //        default:
        //            throw new NotSupportedException("Unsupported bonus rate for savet");
        //    }
        //}

        //public void SetBonuesToSave(GetBonusDto bonusDto)
        //{
        //    AbilityScore = bonusDto.Abilities.First(x => x.Ability.ID == SaveType.AbilityModifier.ID).GetCurrentModifier(bonusDto);
        //    foreach (var passive in bonusDto.PassiveBonus)
        //    {
        //        if (passive.ShouldApplyTo(this) && passive.ShouldApplyToSubType(null))
        //        {
        //            MiscBonus += passive.GetBonus(bonusDto).GetFixedAmount();
        //        }
        //    }
        //    foreach (var active in bonusDto.ActiveBonus)
        //    {
        //        if (active.ShouldApplyTo(this) && active.ShouldApplyToSubType(null))
        //        {
        //            TempBonus += active.GetBonus(bonusDto).GetFixedAmount();
        //        }
        //    }

        //}
    }
}
