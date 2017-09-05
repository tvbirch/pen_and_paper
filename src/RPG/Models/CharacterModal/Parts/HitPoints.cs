using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;
using RPG.Models.RulebookModal.Rounds;


namespace RPG.Models.CharacterModal.Parts
{
    public class HitPoints : GameId
    {
        public int BaseHp { get; set; }
        
        public List<DamageTaken> Damage { get; set; }

        public List<DamageTaken> NonLethalDamage { get; set; }
        
        
        /*

        public string GetWounds()
        {
            var wounds = string.Empty;
            foreach (var wound in wounds)
            {
                wounds += wound + ",";
            }
            if (!string.IsNullOrEmpty(wounds))
            {
                wounds = wounds.Substring(0, wounds.Length - 1);
            }
            return wounds;
        }

       

        public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<AbilityScore> abilities)
        {
            Bonuses.ChangeTime(currentSpecialAbilities,unit, abilities);
            DamageRecuction.ChangeTime(currentSpecialAbilities,unit);
            return false;
        }

        public void AddBonues(List<Bonus> bonues)
        {
            foreach (var bonus in bonues)
            {
                if (bonus.ShouldApplyTo(this))
                {
                    Bonuses.Add(bonus);
                }
            }
            DamageRecuction.AddBonues(bonues);
        }
        public void RemoveBonues(List<Bonus> bonues)
        {
            if (bonues == null)
            {
                return;
            }
            foreach (var bonus in bonues)
            {
                Bonuses.RemoveAll(x => x.ID == bonus.ID);
            }
        }

        public int GetDamageReduction(GetBonusDto bonusDto)
        {
            return DamageRecuction.Bonuses.GetMaxValue(bonusDto);
        }

        public void Heal(int hp)
        {
            DamageTaken = Math.Max(0, DamageTaken - hp);
            NonLethalDamageTaken = Math.Max(0, NonLethalDamageTaken - hp);
        }

        public void TakeDamage(int dmg, DamageType type, GetBonusDto bonusDto)
        {
            DamageTaken += DamageRecuction.SubtractReduction(dmg, type, bonusDto);
        }

        public void TakeNonLethalDamage(int dmg, DamageType type, GetBonusDto bonusDto)
        {
            NonLethalDamageTaken += DamageRecuction.SubtractReduction(dmg, type, bonusDto);
        }*/

        public CalculatedString GetMaxHp(GetBonusDto bonusDto)
        {
            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("Base HP", null, BaseHp);
            var abi = bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiConId).GetCurrentModifier(bonusDto);
            var lvl = bonusDto.Classes.Sum(x => x.Level);

            calcStr.AddIfNotZero(string.Format("Constitution ({0}) * Level ({1})", abi, lvl), null, abi * lvl);
            return calcStr;

            //return BaseHp + (bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiConId).GetCurrentModifier(bonusDto) * bonusDto.Classes.Sum(x => x.Level));
        }
        public CalculatedString GetCurrentHp(GetBonusDto bonusDto)
        {
            if (Damage == null)
            {
                Damage = new List<DamageTaken>();
            }
            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("HP", null, GetMaxHp(bonusDto).GetValueAsInt());
            foreach (var damageTaken in Damage)
            {
                if (damageTaken.Amount < 0)
                {
                    calcStr.AddIfNotZero("DMG", null, damageTaken.Amount);
                }
                else
                {
                    calcStr.AddIfNotZero("Heal", null, damageTaken.Amount);
                }
            }
            return calcStr;
            //return GetMaxHp(bonusDto).GetValueAsInt() - Damage.Sum(x => x.Amount);
        }
        public CalculatedString GetNonLethal()
        {
            if (NonLethalDamage == null)
            {
                NonLethalDamage = new List<DamageTaken>();
            }
            var calcStr = new CalculatedString();
            foreach (var damageTaken in NonLethalDamage)
            {
                if (damageTaken.Amount < 0)
                {
                    calcStr.AddIfNotZero("DMG", null, damageTaken.Amount);
                }
                else
                {
                    calcStr.AddIfNotZero("Heal", null, damageTaken.Amount);
                }
            }
            return calcStr;
            //return NonLethalDamage.Sum(x => x.Amount);
        }

        public void TakeDamage(DamageType damageType, int amount, GetBonusDto bonus, bool ignorreDmgReduction)
        {
            var onHitFeats = bonus.Feats.Where( x => x.Limit != null && x.Limit.Amount != null &&
                    x.Limit.Amount.ActionRequired == RoundAction.AutoOnTakeDamage).ToList();
            foreach (var specialAbility in onHitFeats)
            {
                specialAbility.UseChargeIfPossible(amount, bonus);
            }

            var realDamge = ignorreDmgReduction ? amount : DamageRecuction.SubtractReduction(amount, damageType, bonus);
            if (realDamge <= 0)
            {
                return;
            }
            if (Damage == null)
            {
                Damage = new List<DamageTaken>();
            }
            Damage.Add(new DamageTaken
            {
                Amount = -realDamge
            });
        }
        public void TakeNonLethalDamage(DamageType damageType, int amount, GetBonusDto bonus, bool ignorreDmgReduction)
        {
            var realDamge = ignorreDmgReduction ? amount : DamageRecuction.SubtractReduction(amount, damageType, bonus);
            if (realDamge <= 0)
            {
                return;
            }
            if (NonLethalDamage == null)
            {
                NonLethalDamage = new List<DamageTaken>();
            }
            NonLethalDamage.Add(new DamageTaken
            {
                Amount = realDamge
            });
        }

        public List<DamageTaken> DamgeToDelete(GetBonusDto bonusDto)
        {
            var toRemove = new List<DamageTaken>();
            var maxHp = GetMaxHp(bonusDto).GetValueAsInt();
            var currentHp = GetCurrentHp(bonusDto).GetValueAsInt();
            if (maxHp >= currentHp)
            {
                toRemove.AddRange(Damage);
            }
            var currentNonLethal = GetNonLethal().GetValueAsInt();
            if (currentNonLethal <= 0)
            {
                toRemove.AddRange(NonLethalDamage);
            }

            return toRemove;
        }
        public void Heal(int amount, GetBonusDto bonusDto)
        {
            var maxHp = GetMaxHp(bonusDto).GetValueAsInt();
            var currentHp = GetCurrentHp(bonusDto).GetValueAsInt();
            var maxAmountToHeal = Math.Min(maxHp - currentHp, amount);


            var currentNonLethal = GetNonLethal().GetValueAsInt();
            var maxAmountToHealNl = currentNonLethal - Math.Max(0,currentNonLethal - amount);

            if (maxAmountToHeal != 0)
            {
                Damage.Add(new DamageTaken
                {
                    Amount = maxAmountToHeal
                });
            }
            if (maxAmountToHealNl != 0)
            {
                NonLethalDamage.Add(new DamageTaken
                {
                    Amount = -maxAmountToHealNl
                });
            }
        }

        public int GetDamageTaken()
        {
            return Damage == null ? 0 : Damage.Sum(x => x.Amount);
        }
    }
}
