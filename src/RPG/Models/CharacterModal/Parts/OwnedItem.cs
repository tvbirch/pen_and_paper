using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CoreModal.DTO;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.CharacterModal.Parts
{
    public class OwnedItem : GameId
    {
        public ItemBase Item { get; set; }
        public int? OwnerEnchamtmentBonues { get; set; }
        public bool? OwnerMasterWorked { get; set; }
        public ItemMaterial OwnerMaterial { get; set; }
        public bool? IsEquipped { get; set; }
        public List<SpecialAbility> EnchanmentAbilities { get; set; }
        public List<Bonus> EnchanmentsBonuses { get; set; }

        private IEnumerable<BonusRef> GetItemBonuses(GetBonusDto bonusDto)
        {
            var localList = new List<BonusRef>();
            localList.AddRange(bonusDto.Bonuses.Where(x => x.ShouldApplyTo(Item, typeof(ItemBase))));
            if (OwnerMaterial != null)
            {
                foreach (var materialBonusese in OwnerMaterial.MaterialBonues)
                {
                    if (materialBonusese.ApplyToItemType == Item.Type)
                    {
                        if (materialBonusese.RequiredAbility.HasValue)
                        {
                            var itemRequiresAbility = Item != null && Item.RequiresAbility != null &&
                                                      Item.RequiresAbility.Select(x => x.SpecialAbilityGuid)
                                                          .Contains(materialBonusese.RequiredAbility.Value);
                            if (!itemRequiresAbility)
                            {
                                continue;
                            }
                        }
                        localList.AddRange(materialBonusese.Bonuses.Select(x => new BonusRef(materialBonusese,x)));
                    }
                }
            } 
            else if (Item.Material != null)
            {
                foreach (var materialBonusese in Item.Material.MaterialBonues)
                {
                    if (materialBonusese.ApplyToItemType == Item.Type)
                    {
                        if (materialBonusese.RequiredAbility.HasValue)
                        {
                            var itemRequiresAbility = Item != null && Item.RequiresAbility != null &&
                                                      Item.RequiresAbility.Select(x => x.SpecialAbilityGuid)
                                                          .Contains(materialBonusese.RequiredAbility.Value);
                            if (!itemRequiresAbility)
                            {
                                continue;
                            }
                        }
                        localList.AddRange(materialBonusese.Bonuses.Select(x => new BonusRef(materialBonusese, x)));
                    }
                }
            }
            return localList;
        }

        public CalculatedString GetArmorCheckPenelty(GetBonusDto bonusDto)
        {
            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("Base", null, -Item.ArmorCheckPenelty.GetValueOrDefault());
            if (IsMasterWorked() && (Item.Type == ItemType.Armor || Item.Type == ItemType.Shield))
            {
                calcStr.AddIfNotZero("Masterworked", null, 1);
            }

            var bonuses =
                GetItemBonuses(bonusDto)
                    .Where(x => x.ShouldApplyToSubType(Configuration.ArmorCheckPeneltyReduction))
                    .MaxBonusRefList(bonusDto);
            
            calcStr.AddPartsByRef(bonuses,bonusDto);

            return calcStr;
        }

        private List<BonusRef> GetArmorAcBonusList(GetBonusDto bonusDto)
        {
            var armor = new List<BonusRef>();
            if (Item.Type == ItemType.Armor)
            {
                armor.Add(new BonusRef(Item, new Bonus
                {
                    BonusValue = new BonusToAdd
                    {
                        FixedValue = Item.AC.GetValueOrDefault(),
                    },
                    Type = BonusType.ArmorBonus
                }));
                armor.Add(new BonusRef(Item, new Bonus
                {
                    BonusValue = new BonusToAdd
                    {
                        FixedValue = Item.EnchanmentBonus.GetValueOrDefault(),
                    },
                    Type = BonusType.EnhancementBonus
                }));
            }
            armor.AddRange(GetItemBonuses(bonusDto).Where(x => x.Type == BonusType.ArmorBonus && x.ShouldApplyToSubType(Configuration.ExtraAC)).MaxBonusRefList(bonusDto));
            return armor;
        }


        private List<BonusRef> GetShieldAcBonusList(GetBonusDto bonusDto)
        {
            var armor = new List<BonusRef>();
            if (Item.Type == ItemType.Shield)
            {
                armor.Add(new BonusRef(Item, new Bonus
                {
                    BonusValue = new BonusToAdd
                    {
                        FixedValue = Item.AC.GetValueOrDefault(),
                    },
                    Type = BonusType.ShieldBonus
                }));
                armor.Add(new BonusRef(Item, new Bonus
                {
                    BonusValue = new BonusToAdd
                    {
                        FixedValue = Item.EnchanmentBonus.GetValueOrDefault(),
                    },
                    Type = BonusType.EnhancementBonus
                }));
            }
            armor.AddRange(GetItemBonuses(bonusDto).Where(x => x.Type == BonusType.ShieldBonus && x.ShouldApplyToSubType(Configuration.ExtraAC)).MaxBonusRefList(bonusDto));
            return armor;
        }

        public List<BonusRef> GetArmorAcBonus(GetBonusDto bonusDto)
        {
            var itemAcBonus = GetArmorAcBonusList(bonusDto);
            if (Item.Type == ItemType.Armor)
            {
                var currentEnchantmentBonus = itemAcBonus.FirstOrDefault(x => x.Bonues.Type == BonusType.EnhancementBonus);
                if (currentEnchantmentBonus != null)
                {
                    currentEnchantmentBonus.Bonues.BonusValue.FixedValue += OwnerEnchamtmentBonues.GetValueOrDefault();
                }
                else
                {
                    itemAcBonus.Add(new BonusRef(Item,new Bonus
                    {
                        Type = BonusType.EnhancementBonus,
                        BonusValue = new BonusToAdd
                        {
                            FixedValue = OwnerEnchamtmentBonues.GetValueOrDefault()
                        }
                    }));
                }
            }

            return itemAcBonus;
        }

        public List<BonusRef> GetShieldAcBonus(GetBonusDto bonusDto)
        {
            var itemAcBonus = GetShieldAcBonusList(bonusDto);
            if (Item.Type == ItemType.Shield)
            {
                var currentEnchantmentBonus = itemAcBonus.FirstOrDefault(x => x.Bonues.Type == BonusType.EnhancementBonus);
                if (currentEnchantmentBonus != null)
                {
                    currentEnchantmentBonus.Bonues.BonusValue.FixedValue += OwnerEnchamtmentBonues.GetValueOrDefault();
                }
                else
                {
                    itemAcBonus.Add(new BonusRef(Item, new Bonus
                    {
                        Type = BonusType.EnhancementBonus,
                        BonusValue = new BonusToAdd
                        {
                            FixedValue = OwnerEnchamtmentBonues.GetValueOrDefault()
                        }
                    }));
                }
            }
            return itemAcBonus;
        }

        public double GetWeight(GetBonusDto bonusDto)
        {
            var weightBonus = GetItemBonuses(bonusDto).Where(x => x.ShouldApplyToSubType(Configuration.WeightReductionInProcentage)).MaxBonusRefList(bonusDto);
            var calcStr = new CalculatedString();
            calcStr.AddPartsByRef(weightBonus,bonusDto);
            var procentReduction = calcStr.GetValueAsInt();

            return Item.Weight.Lb / 100.0 * (100-procentReduction);
        }

        public bool CanAttack()
        {
            return Item.CanAttack;
        }

        public string GetName()
        {
            var baseName = Item.Name;
            var totalEnchantmentBonues = GetEnchantmentBonus();
            var materialName = OwnerMaterial != null
                ? OwnerMaterial.Name
                : Item.Material != null ? Item.Material.Name : null;
            var masterWorked = IsMasterWorked();

            var finalName = "";
            if (masterWorked)
            {
                finalName += "Masterworked ";
            }
            if (materialName != null)
            {
                finalName += materialName + " ";
            }
            finalName += baseName;
            if (totalEnchantmentBonues != 0)
            {
                finalName += " +" + totalEnchantmentBonues;
            }
            return finalName;
        }

        public string GetName(Guid? primaryWeaponId, Guid? offHandWeaponId)
        {
            var name = GetName();
            if (ID == primaryWeaponId)
            {
                name += ", main hand";
            }
            else if (ID == offHandWeaponId)
            {
                name += ", off-hand";
            }
            return name;
        }

        private bool IsMasterWorked()
        {
            return OwnerMasterWorked.GetValueOrDefault() || Item.Masterworked;
        }

        private int GetEnchantmentBonus()
        {
            return OwnerEnchamtmentBonues.GetValueOrDefault() + Item.EnchanmentBonus.GetValueOrDefault();
        }

        public CalculatedString GetAttackBonus(GetBonusDto bonusDto, int currentEquipmentPenelty, int? currentBaseAttackWithItem, int proficiencyPenelty, int shieldPenelty)
        {
            var totalEnchantmentBonues = GetEnchantmentBonus();
            var masterWorkedBonues = IsMasterWorked() && Item.Type == ItemType.Weapon ? 1 : 0;

            var abilityBonues = 0;
            if (Item.UseItemsOwnAbilistyScore.HasValue)
            {
                abilityBonues = Item.UseItemsOwnAbilistyScore.Value;
            } 
            else
            {
                if (Item.IsRanged)
                {
                    abilityBonues = bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiDexId)
                            .GetCurrentModifier(bonusDto);
                }
                else
                {
                    abilityBonues = bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiStrId)
                            .GetCurrentModifier(bonusDto);
                }
            }
            
            var size = bonusDto.Character.Race.GetAttackAndAcModifier(bonusDto);

            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("Base Attack",null, currentBaseAttackWithItem.GetValueOrDefault());
            calcStr.AddIfNotZero(Item.IsRanged ? "Dexterity" : "Strength", null, abilityBonues);
            if (totalEnchantmentBonues == 0)
            {
                calcStr.AddIfNotZero("Masterworked", null, masterWorkedBonues);
            }
            calcStr.AddIfNotZero("Enchantment Bonus", null, totalEnchantmentBonues);
            calcStr.AddIfNotZero("Equipment Penelty", null, currentEquipmentPenelty);
            calcStr.AddIfNotZero("Base Attack trade", null, Tools.GetBonusesApplyingTo(Configuration.BabId, BonusApplyToType.BaseAttack, bonusDto.Bonuses).MaxBonuesSum(bonusDto));
            calcStr.AddIfNotZero("Proficiency Penelty",null,proficiencyPenelty);
            calcStr.AddIfNotZero("Shield Penelty", null, shieldPenelty);
            calcStr.AddIfNotZero("Size", null, size);

            calcStr.AddPartsByRef(GetItemBonuses(bonusDto).Where(x => x.ShouldApplyToSubType(Configuration.AttackBonues)).ToList(),bonusDto);

            return calcStr;
        }

        public CalculatedString GetDamageString(GetBonusDto bonusDto)
        {
            //SetCorrectMaterial(bonusDto);
            var isTwoHanded = Item.RequiresSlots.Select(x => x.Requirement).Count(x => x == ItemSlotRequirement.WeaponHand) == 2;
            var damage = Item.Damage;
            if (damage == null || damage.Amount == null)
            {
                return null;
            }
            var calcStr = new CalculatedString();

            var bonusList = GetItemBonuses(bonusDto).Where(x => x.ShouldApplyToSubType(Configuration.Damage) ||
                                                                (Item.Type == ItemType.Armor && x.ShouldApplyToSubType(Configuration.DamageArmor)) ||
                                                                (Item.Type == ItemType.Shield && x.ShouldApplyToSubType(Configuration.DamageShield)) ||
                                                                (Item.Type == ItemType.Weapon && x.ShouldApplyToSubType(Configuration.DamageWeapon)) ||
                                                                (Item.Type == ItemType.Other && x.ShouldApplyToSubType(Configuration.DamageMisc)) ||
                                                                (Item.Type == ItemType.Unarmed && x.ShouldApplyToSubType(Configuration.DamageUnarmed))
            ).ToList();
            
            calcStr.AddIfNotZero(Item.Name, null, damage.Amount);
            
            var enchantmentBonus = GetEnchantmentBonus();
            if (enchantmentBonus != 0)
            {
                calcStr.AddIfNotZero("Enchantment Bonus", null, new DiceRoll
                {
                    FixedAmount = enchantmentBonus
                });
            }

            if (Item.UseItemsOwnAbilistyScore.HasValue)
            {
                calcStr.AddIfNotZero("Item Ability Score",null, new DiceRoll
                {
                    FixedAmount = Item.UseItemsOwnAbilistyScore
                });
            }
            else
            {
                if (!Item.IsRanged)
                {
                    var abiMod = bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiStrId)
                            .GetCurrentModifier(bonusDto);
                    if (Item.IsOneAndAHalfHanded && bonusDto.EquippedItems.Sum(x => x.Item.RequiresSlots.Count(y => y.Requirement == ItemSlotRequirement.WeaponHand)) == 1)
                    {
                        calcStr.AddIfNotZero("1.5 x Strength",null, new DiceRoll
                        {
                            FixedAmount = (int)((double)abiMod * 1.5)
                        });
                    }
                    else if (isTwoHanded)
                    {
                        calcStr.AddIfNotZero("1.5 x Strength",null, new DiceRoll
                        {
                            FixedAmount = (int)((double)abiMod * 1.5)
                        });
                    }
                    else
                    {
                        calcStr.AddIfNotZero("Strength",null, new DiceRoll
                        {
                            FixedAmount = abiMod
                        });
                    }
                }    
            }

            foreach (var bonus in bonusList)
            {
                 if (bonus.Bonues != null && bonus.Bonues.ParentAbility != null &&
                            bonus.Bonues.ParentAbility.Limit != null &&
                            bonus.Bonues.ParentAbility.Limit.Amount.TradeDoubleIfThw &&
                            isTwoHanded)
                {
                    var b = bonus.GetBonus(bonusDto);
                    b.FixedAmount *= 2;
                    calcStr.AddIfNotZero(bonus.Parent.Name, bonus.GetCondition(), b);
                }
                else
                {
                    calcStr.AddIfNotZero(bonus.Parent.Name, bonus.GetCondition(), bonus.GetBonus(bonusDto));
                }   
            }
            return calcStr;
        }

        public string GetAttackType()
        {
            var damage = Item.Damage;
            if (damage == null)
            {
                return "";
            }
            return damage.Type.ToString();
        }

        public string GetCriticalRange(GetBonusDto bonusDto)
        {
            var critRange = 0;
            if (Item.CritRange.HasValue)
            {
                critRange += Item.CritRange.GetValueOrDefault();
            }
            
            var calcStr = new CalculatedString();
            calcStr.AddPartsByRef(GetItemBonuses(bonusDto).Where(x => x.ShouldApplyToSubType(Configuration.CritRange)).ToList(), bonusDto);
            if (calcStr.GetValueAsInt() != 0)
            {
                critRange *= calcStr.GetValueAsInt();
            }
            string critStr = null;
            if (critRange == 0)
            {
                critStr = "20";
            }
            else
            {
                critStr = (20 - critRange).ToString() + " - 20";
            }
            if (Item.CriticalMultiplier.HasValue)
            {
                critStr += " x" + Item.CriticalMultiplier;
            }

            return critStr;
        }

        public string GetRange(GetBonusDto bonusDto)
        {
            //SetCorrectMaterial(bonusDto);
            var range = 0;
            if (Item.RangeIncrement.HasValue)
            {
                range += Item.RangeIncrement.GetValueOrDefault();
            }
            
            var calcStr = new CalculatedString();
            calcStr.AddPartsByRef(GetItemBonuses(bonusDto).Where(x => x.ShouldApplyToSubType(Configuration.RangeIncrement)).ToList(), bonusDto);
            if (calcStr.GetValueAsInt() != 0)
            {
                range += calcStr.GetValueAsInt();
            }

            if (range == 0)
            {
                return "";
            }
            return range + "ft";
        }


        public CalculatedString GetMaxDexBonus(GetBonusDto bonusDto)
        {
            var calcStr = new CalculatedString();
            if (!Item.MaxDexBonus.HasValue)
            {
                return null;
            }

            calcStr.AddIfNotZero("Base", null, Item.MaxDexBonus.Value);
            calcStr.AddPartsByRef(GetItemBonuses(bonusDto).Where(x => x.ShouldApplyToSubType(Configuration.MaxDexBonus)).ToList(),bonusDto);

            return calcStr;
        }

        public List<BonusRef> GetDamageReductionBonus(GetBonusDto bonusDto)
        {
            var bonuses = GetItemBonuses(bonusDto).Where(x => x.ShouldApplyTo(new GameId{ID = Configuration.DamageRecuctionId}, typeof(DamageRecuction))).ToList();
            return bonuses;
        }

        public override string ToString()
        {
            return Item == null ? "N/A" : Item.Name;
        }
    }
}