using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;

namespace RPG.Models.CharacterModal.Parts
{
    public class ClassLevel : GameId
    {
        public ClassBase Class { get; set; }
        public int Level { get; set; }
        
        //public int GetCasterLevel()
        //{
        //    if (Class.Spells == null)
        //    {
        //        return 0;
        //    }
        //    var highestLevelCaster = Class.Spells.SpellPrDay.Where(x => x.Level <= Level).OrderByDescending(x => x.Level).FirstOrDefault();
        //    if (highestLevelCaster == null)
        //    {
        //        return 0;
        //    }
        //    var highestSpellPossible = highestLevelCaster.NumberOfSpells.OrderByDescending(x => x.SpellLevel).FirstOrDefault();
        //    if (highestSpellPossible == null) return 0;
        //    return highestSpellPossible.SpellLevel;
        //}

        public int GetBaseAttackBonus()
        {
            var bab = 0;
            switch (Class.BaseAttackBaseSaveBonus)
            {
                case BaseBonusRate.Good:
                    bab = Level;
                    break;
                case BaseBonusRate.Average:
                    bab = ((int)Math.Floor(3.0m * ((decimal)Level) / (decimal)4));
                    break;
                case BaseBonusRate.Poor:
                    bab = (int)Math.Floor(((decimal)Level / 2.0m));
                    break;
                default:
                    throw new NotSupportedException("Unsupported bonus rate for base attack");
            }
            return bab;
        }

        public List<SpecialAbility> GetAbilitiesAtCurrentLevel()
        {
            return Class.GetAbilitiesAtCurrentLevel(Level);
        }

        public void EnrichPassiveBonusDto(GetBonusDto dto)
        {
            var abilities = Class.GetAbilitiesAtCurrentLevel(Level);
            foreach (var specialAbility in abilities)
            {
                specialAbility.EnrichPassiveBonusDto(dto);
            }
        }

        public void EnrichActiveBonusDto(GetBonusDto dto)
        {
            var abilities = Class.GetAbilitiesAtCurrentLevel(Level);
            foreach (var specialAbility in abilities)
            {
                specialAbility.EnrichActiveBonusDto(dto);
            }
        }

        public int GetCasterLevel(GetBonusDto bonusDto)
        {
            if (!Class.IsCaster())
            {
                return 0;
            }
            var casterLevelBase = Level;
            var casterLevelBonues = 0;
            if (Class.ArcaneCaster)
            {
                casterLevelBonues +=
                    bonusDto.Bonuses.Where(x => x.ShouldApplyToSubType(Configuration.ExistingCasterLevelArcaneId))
                        .Sum(x => x.GetBonus(bonusDto).GetFixedAmount());
            } 
            if (Class.DivineCaster)
            {
                casterLevelBonues +=
                    bonusDto.Bonuses.Where(x => x.ShouldApplyToSubType(Configuration.ExistingCasterLevelDivineId))
                        .Sum(x => x.GetBonus(bonusDto).GetFixedAmount());
            }


            return casterLevelBase + casterLevelBonues;
        }
    }
}