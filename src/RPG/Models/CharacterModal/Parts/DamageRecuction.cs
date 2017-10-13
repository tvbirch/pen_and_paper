using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CoreModal.DTO;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;

namespace RPG.Models.CharacterModal.Parts
{
    public static class DamageRecuction 
    {
        public static CalculatedString GetDamageReduction(GetBonusDto bonusDto)
        {
            //TODO: support different types of dmg reduction.
            var damageReductionBonues = GetDamageReductionBonues(bonusDto);
            var max = damageReductionBonues.OrderByDescending(x => x.GetBonus(bonusDto).GetFixedAmount()).ToList();

            var calcStr = new CalculatedString(); 
            if (max.Count != 0)
            {
                damageReductionBonues = new List<BonusRef>();
                damageReductionBonues.Add(max.First());
                calcStr.AddPartsByRef(damageReductionBonues, bonusDto);
            }
            //TODO: Crystals til våben og armor.
            return calcStr;
        }

        private static List<BonusRef> GetDamageReductionBonues(GetBonusDto bonusDto)
        {
            var damageReductionBonues = bonusDto.Bonuses.Where(x => x.Bonues.ShouldApplyTo(new GameId
            {
                ID = Configuration.DamageRecuctionId,
            }, typeof (DamageRecuction)) && x.ShouldApplyToSubType(null)).ToList();
            var damageReductionBonusFromItems = bonusDto.EquippedItems.Select(x => x.GetDamageReductionBonus(bonusDto)).SelectMany(x => x).ToList();
            damageReductionBonues.AddRange(damageReductionBonusFromItems);
            return damageReductionBonues;
        }

        public static int SubtractReduction(int dmg, DamageType type, GetBonusDto bonusDto)
        {
            var reduction = GetReductionOfType(type, bonusDto);
            var dmgAfterReduction =  Math.Max(0, dmg - reduction);
            return dmgAfterReduction;
        }

        private static int GetReductionOfType(DamageType type, GetBonusDto bonusDto)
        {
            var localBonues = GetDamageReductionBonues(bonusDto);
            localBonues = localBonues.Where(x => x.Bonues.IsOfType(type)).ToList();
            if(localBonues.Count == 0)
            {
                return 0;
            }

            var max = localBonues.Max(x => x.GetBonus(bonusDto).GetFixedAmount());

            return Math.Max(0, max);
        }
    }
}
