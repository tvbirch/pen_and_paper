using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.Ajax.Utilities;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.RulebookModal.BaseTypes.Bonuses
{
    public class Bonus: ElementId, IRoundable
    {
        public BonusApplyTo ApplyTo { get; set; }
        public BonusToAdd BonusValue { get; set; }
        public BonusType Type { get; set; }
        public bool CanTakeAsEnchantment { get; set; }

        [NotMapped]
        public SpecialAbility ParentAbility { get; set; }

        public virtual ICollection<Race> BonuesGrantedByRaces { get; set; }
        public virtual ICollection<ItemBase> BonuesGrantedByItems { get; set; }
        public virtual ICollection<OwnedItem> BonuesGrantedByOwnedItems { get; set; }

        public virtual ICollection<MaterialBonuses> MaterialBonus { get; set; }


        public virtual DiceRoll GetBonus(GetBonusDto bonusDto)
        {
            if (ParentAbility != null && ParentAbility.IsActive(bonusDto) && ParentAbility.IsChargeItem())
            {
                if (ParentAbility.Limit != null && ParentAbility.Limit.Amount != null && 
                    (ParentAbility.Limit.Amount.ActionRequired == RoundAction.AutoOnHit ||
                    ParentAbility.Limit.Amount.ActionRequired == RoundAction.AutoOnTakeDamage))
                {
                    return BonusValue.GetBonus(bonusDto); 
                }
                var activeAbi = bonusDto.Round.ActivatedAbilities.First(x => x.AbilityId == ParentAbility.ID);
                var charge = ParentAbility.BonusFromCharges.First(x => x.NumberOfChargesForBonus == activeAbi.Charges);
                return charge.Bonus;
            }
            if (ParentAbility != null && ParentAbility.IsActive(bonusDto) && ParentAbility.IsTradeOff())
            {
                var activeAbi = bonusDto.Round.ActivatedAbilities.First(x => x.AbilityId == ParentAbility.ID);

                var toTrade = 0;
                if (ParentAbility.Limit.Amount.TradeWith == this)
                {
                    toTrade = -activeAbi.Charges;
                }
                else
                {
                    toTrade = (int)(activeAbi.Charges * activeAbi.Multiplier);
                }

                return new DiceRoll
                {
                    FixedAmount = toTrade
                };
            }


            return BonusValue.GetBonus(bonusDto);
        }

        public virtual bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<AbilityScore> abilities)
        {
            /*if (LimitUse != null)
            {
                LimitUse.ChangeTime(currentSpecialAbilities,unit, abilities);
                if (LimitUse.Duration != null && LimitUse.Amount == null && LimitUse.LimitUnit == null)
                {
                    //TODO: hvis noget om bonser fucker op er det sikkert denne antagelse
                    //Det må være en bonus applyed at en spell. Fjerner denne.
                    return true;
                }
            }*/
            return false;
        }

        /// <summary>
        /// Override type should be used on all classes retrived using entityframework.
        /// </summary>
        /// <param name="idAble"></param>
        /// <param name="overrideType"></param>
        /// <returns></returns>
        public bool ShouldApplyTo(GameId idAble, Type overrideType = null)
        {
            if (idAble == null)
            {
                return false;
            }
            var type = overrideType ?? idAble.GetType();
            if (type == typeof(AbilityScore))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Ability && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == ((AbilityScore)idAble).Ability.ID);
            }
            if (type == typeof(ArmorClass))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Ac && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(ClassLevel))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Class && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == ((ClassLevel)idAble).Class.ID);
            }
            if (type == typeof(DamageRecuction))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.DamageRecuction && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(Grapple))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Grapple && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(HitPoints))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.HitPoints && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(ItemBase))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Item && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(Initiativ))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Initiativ && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(Race))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Race && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(Round))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Round && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(Save)) // change to savescore
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Save && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            if (type == typeof(SkillRank))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Skill && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == ((SkillRank)idAble).Skill.ID);
            }
            if (type == typeof(Spell))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.Spell && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == ((Spell)idAble).ID);
            }
            if (type == typeof(SpellResistance))
            {
                return ApplyTo.ApplyToType == BonusApplyToType.SpellResistance && (ApplyTo.ApplyToAll || ApplyTo.ApplyToGuid == idAble.ID);
            }
            throw new NotImplementedException();
        }

        public bool ShouldApplyToSubType(Guid? subtype)
        {
            return !ApplyTo.ApplyToSubtypeGuid.HasValue || ApplyTo.ApplyToSubtypeGuid.Value == subtype.GetValueOrDefault();
        }
        /*public bool IsChargesPossible(int charge, Round round, List<AbilityScore> abilities, List<ClassBase> classes)
        {
            return LimitUse != null && LimitUse.IsChargesPossible(charge, round,abilities,classes);
        }

        public void UseCharge(int charge, Round round, List<AbilityScore> abilities, List<ClassBase> classes)
        {
            LimitUse.UseChagesIfPossible(charge, round, ApplyToKey, abilities,classes);
        }

        public void ResetLimit(TimeLimitUnit time)
        {
            if (LimitUse == null) return;
            LimitUse.Reset(time);
        }*/

        public bool IsOfType(DamageType type)
        {
            return BonusValue != null && (BonusValue.Against == type ||
                (type == DamageType.Bludgeoning && BonusValue.Against == DamageType.AllWeapons) ||
                (type == DamageType.Piercing && BonusValue.Against == DamageType.AllWeapons) ||
                (type == DamageType.Slashing && BonusValue.Against == DamageType.AllWeapons)) ||
                type == DamageType.AllWeapons;
        }

        public bool IsActive(GetBonusDto bonus)
        {
            if (ParentAbility != null)
            {
                return ParentAbility.IsActive(bonus);
            }
            return true;
        }

        public bool IsTemporary()
        {
            if (ParentAbility != null)
            {
                return ParentAbility.Limit != null && ParentAbility.Limit.Duration != null;
            }
            return false;
        }

        public string GetCondition()
        {
            if (ApplyTo != null)
            {
                return ApplyTo.ApplyToCondition;
            }
            return null;
        }
    }
}
