using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.CoreModal.DTO;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Conditions;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.RulebookModal.BaseTypes.SpecialAbilities
{
    public class SpecialAbility: ElementId
    {
        public List<Bonus> Bonuses { get; set; }
        public UsableLimit Limit { get; set; }
        public List<BonusFromCharges> BonusFromCharges { get; set; }
        public bool CanTakeAsFeat { get; set; }

        public Guid? RequiresSpecialAbilityActive { get; set; }
        public int? RequiredNumberOfCharges { get; set; }

        public Condition ApplyConditionOnActivate { get; set; }
        public Condition ApplyConditionOnDeactivate { get; set; }

        public virtual ICollection<ItemBase> ItemBase { get; set; }
        public virtual ICollection<OwnedItem> OwnedItemBase { get; set; }
        public virtual ICollection<Race> Races { get; set; }
        public virtual ICollection<Character> TakenBy { get; set; }
        public bool CanTakeAsEnchantment { get; set; }

        public void SetChildReferences()
        {
            if (Bonuses != null)
            {
                foreach (var bonuse in Bonuses)
                {
                    bonuse.ParentAbility = this;
                }
                if (Limit != null && Limit.Amount != null && Limit.Amount.TradeWith != null)
                {
                    Limit.Amount.TradeWith.ParentAbility = this;
                }
            }
        }

        public bool IsLimited()
        {
            if (Limit == null || (
                Limit.Amount != null && 
                Limit.Amount.FixedAmount == null && 
                Limit.Amount.Ability == null && 
                Limit.Amount.ClassProgression.Count == 0 && 
                Limit.Duration != null && 
                Limit.Duration.Amount == null && 
                Limit.Duration.DurationAbilityModifier == null))
            {
                return false;
            }
            return true;
        }

        public bool IsChargeItem()
        {
            return BonusFromCharges != null && BonusFromCharges.Count > 0;
        }
        
        public bool UseChargeIfPossible(int charge, GetBonusDto bonues)
        {
            if (Limit != null)
            {
                if (!Limit.IsChargesPossible(charge, bonues, ID))
                {
                    return false;
                }
                Limit.UseChagesIfPossible(charge, bonues,ID);
                if (ApplyConditionOnActivate != null)
                {
                    bonues.Round.ApplyCondition(ApplyConditionOnActivate,ID);
                }
                
                //var bonuse = BonusFromCharges.Where(x => x.NumberOfChargesForBonus <= charge)
                //    .OrderByDescending(x => x.NumberOfChargesForBonus)
                //    .FirstOrDefault();
                //if (bonuse != null)
                //{
                //    foreach (var bonus in Bonuses)
                //    {
                //        bonus.BonusValue.SetFixedBonus(bonuse.Bonus);
                //    }    
                //}
                
            }
            return true;
        }

        public bool IsActive(GetBonusDto bonues, bool? targetHit = null)
        {
            if (Limit != null && IsLimited())
            {
                return Limit.GetIsActive(bonues, ID, targetHit);
            }
            if (RequiresSpecialAbilityActive != null)
            {
                return IsRequiredAbilityActive(bonues);
            }
            return true;
        }

        public bool IsRequiredAbilityActive(GetBonusDto bonues)
        {
            if (!RequiresSpecialAbilityActive.HasValue)
            {
                return true;
            }

            var active =
                bonues.Round.ActivatedAbilities.Where(x => x.AbilityId == RequiresSpecialAbilityActive)
                    .Select(x => x.Charges)
                    .Sum();
            if (RequiredNumberOfCharges.HasValue)
            {
                return RequiredNumberOfCharges.Value <= active;
            }
            else
            {
                return active > 0;
            }
        }

        public List<RoundActivateAbilities> ChangeTime(ChangeCharacterTimeDto changeCharacterTimeDto)
        {
            if (Bonuses != null)
            {
                if (Limit != null)
                {
                    var isNoLongerActive = Limit.ChangeTime(changeCharacterTimeDto.TimeUnit, changeCharacterTimeDto.Bonus, ID);
                    if (isNoLongerActive)
                    {
                        var toInaktivate = changeCharacterTimeDto.Bonus.Round.ActivatedAbilities.Where(x => x.AbilityId == ID && x.IsActive).ToList();
                        foreach (var activateAbilitiese in toInaktivate)
                        {
                            activateAbilitiese.IsActive = false;
                        }
                        if (ApplyConditionOnDeactivate != null)
                        {
                            changeCharacterTimeDto.Bonus.Round.ApplyCondition(ApplyConditionOnDeactivate, ID);
                        }
                    }
                    var isActive = IsActive(changeCharacterTimeDto.Bonus, changeCharacterTimeDto.TargetHit);
                    if ((int)Limit.Duration.Unit.GetValueOrDefault() == (int)changeCharacterTimeDto.TimeUnit && isActive)
                    {
                        var hpDmgBonuses = Bonuses.Where(x => x.ShouldApplyTo(new GameId
                        {
                            ID = Configuration.HitPointsDmg
                        }, typeof(HitPoints))).ToList();
                        if (hpDmgBonuses.Any())
                        {
                            var dmgToTake = hpDmgBonuses.MaxBonuesSum(changeCharacterTimeDto.Bonus);
                            if (Limit.Amount != null)
                            {
                                dmgToTake = Limit.Amount.LimitAutoChargesIfNecessary(dmgToTake, changeCharacterTimeDto.Bonus);
                            }
                            changeCharacterTimeDto.Bonus.Character.TakeDamage(true, DamageType.AllWeapons, dmgToTake, true);
                        }

                        var hpDmgNlBonuses = Bonuses.Where(x => x.ShouldApplyTo(new GameId
                        {
                            ID = Configuration.HitPointsDmgNl
                        }, typeof(HitPoints))).ToList();
                        if (hpDmgNlBonuses.Any())
                        {
                            var dmgToTake = hpDmgNlBonuses.MaxBonuesSum(changeCharacterTimeDto.Bonus);
                            if (Limit.Amount != null)
                            {
                                dmgToTake = Limit.Amount.LimitAutoChargesIfNecessary(dmgToTake, changeCharacterTimeDto.Bonus);
                            }
                            changeCharacterTimeDto.Bonus.Character.TakeDamage(false, DamageType.AllWeapons, dmgToTake, true);
                        }

                        var hpHealBonuses = Bonuses.Where(x => x.ShouldApplyTo(new GameId
                        {
                            ID = Configuration.HitPointsHeal
                        }, typeof(HitPoints))).ToList();
                        if (hpHealBonuses.Any())
                        {
                            var dmgToHeal = hpHealBonuses.MaxBonuesSum(changeCharacterTimeDto.Bonus);
                            if (Limit.Amount != null)
                            {
                                dmgToHeal = Limit.Amount.LimitAutoChargesIfNecessary(dmgToHeal, changeCharacterTimeDto.Bonus);
                            }
                            changeCharacterTimeDto.Bonus.Character.HealDamage(dmgToHeal);
                        }
                    }

                    if ((int)changeCharacterTimeDto.TimeUnit >= (int)Limit.Amount.ResetUseTimeLimtUnit.GetValueOrDefault())
                    {
                        return changeCharacterTimeDto.Bonus.Round.ActivatedAbilities.Where(x => x.AbilityId == ID).ToList();
                    }
                }
            }
            return new List<RoundActivateAbilities>();
        }

        public string GetLimitAmount(GetBonusDto bonues)
        {
            if (!IsLimited()) return string.Empty;
            if (Limit != null)
            {
                return Limit.ToString(bonues,ID);
            }
            return null;
        }

        public int? GetLimitAmountAsInt(GetBonusDto bonues)
        {
            if (!IsLimited()) return null;
            if (Limit != null)
            {
                return Limit.Amount.GetAmount(bonues,ID);
            }
            return null;
        }

        public RoundAction? GetActionToUse(GetBonusDto bonues)
        {
            if (IsActive(bonues)) return null;
            /*var limits = GetLimit();
            foreach (var limit in limits)
            {
                if (limit.Amount != null)
                {
                    return limit.Amount.ActionRequired;
                }
            }*/
            if (Limit != null && Limit.Amount != null)
            {
                return Limit.Amount.ActionRequired;
            }
            return null;
        }

        public void EnrichPassiveBonusDto(GetBonusDto dto)
        {
            if (IsLimited())
            {
                return;
            }

            if (!IsRequiredAbilityActive(dto))
            {
                return;
            }
            if (Bonuses != null)
            {
                foreach (var bonuse in Bonuses)
                {
                    dto.PassiveBonus.Add(new BonusRef(this,bonuse));
                }
            }
        }

        public void EnrichActiveBonusDto(GetBonusDto dto)
        {
            if (!IsLimited())
            {
                return;
            }
            if (IsActive(dto) && Bonuses != null)
            {
                foreach (var bonuse in Bonuses)
                {
                    dto.ActiveBonus.Add(new BonusRef(this, bonuse));
                }
                if (Limit != null && Limit.Amount != null && Limit.Amount.TradeWith != null)
                {
                    dto.PassiveBonus.Add(new BonusRef(this, Limit.Amount.TradeWith));
                }
                //dto.ActiveBonus.AddRange(Bonuses);
            }
        }

        public string GetName(GetBonusDto bonues)
        {
            if (!IsLimited())
            {
                return Name;
            }
            var name = Name + " " + GetLimitAmount(bonues);

            if (IsActive(bonues))
            {
                name += " (" + Limit.GetRoundsLeft(bonues, ID)+"R)";
            }


            return name;
        }

        public bool IsCurrentlyUseable(GetBonusDto bonuses)
        {
            if (IsLimited())
            {
                if (!Limit.IsChargesPossible(1, bonuses, ID))
                {
                    return false;
                }

                var isCharge = IsChargeItem();
                var isTrade = IsTradeOff();
                var limitLeft = GetLimitAmountAsInt(bonuses);

                return !IsActive(bonuses) || (isCharge && limitLeft > 0) || (isTrade && limitLeft > 0);
            }
            return false;
        }

        public bool IsTradeOff()
        {
            if (Limit != null && Limit.Amount != null && Limit.Amount.TradeWith != null)
            {
                return true;
            }
            return false;
        }
    }
}
