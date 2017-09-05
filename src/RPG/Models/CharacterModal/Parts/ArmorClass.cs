using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CoreModal.DTO;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;


namespace RPG.Models.CharacterModal.Parts
{
    //Tjek at alle MaxBonusList vælger max bonus + alle de - bonuser der er. F.eks. +6 armro bonus for armor. -2 armor bonus fra rage, skal kun tælles som 4.
    public static class ArmorClass
    {
        //public AbilityScore Dex { get; private set; }
        //public Size Size { get; private set; }
        //public MaxBonusList<Bonus> Bonuses { get; private set; }
        //private List<ItemSlot> _equipedArmorItems;

        //public ArmorClass(Size size, AbilityScore dex)
        //{
        //    Size = size;
        //    Dex = dex;
        //    Bonuses = new MaxBonusList<Bonus>();
        //    _equipedArmorItems = new List<ItemSlot>();
        //}

        //public int GetAc(GetBonusDto bonusDto)
        //{
        //    var maxAc = GetMaxArmorBonus(bonusDto);
        //    var maxDex = Math.Min(Dex.GetCurrentModifier(bonusDto), GetMaxDexBonus());
        //    var size = Size.GetAttackAndAcModifier(bonusDto);
        //    return 10 + maxAc + maxDex + size;
        //}

        //private int GetMaxArmorBonus(GetBonusDto bonusDto, List<BonusType> typeLimit = null)
        //{
        //    var localBonusList = new MaxBonusList<Bonus>();
        //    localBonusList.AddRange(Bonuses);
        //    foreach (var armorItem in _equipedArmorItems)
        //    {
        //        if (armorItem.Item == null)
        //        {
        //            continue;
        //        }
        //        if (armorItem.Item.Item.Type == ItemType.Armor)
        //        {
        //            //localBonusList.Add(new Bonus(Guid.NewGuid(), "ac",null, "", "ac", new BonusToAdd(armorItem.Item.GetAc(), null, null, null, null, null), BonusType.ArmorBonus, null));
        //            localBonusList.Add(new Bonus
        //            {
        //                ID = Guid.NewGuid(),
        //                ApplyTo = new BonusApplyTo
        //                {
        //                    ApplyToType = BonusApplyToType.Ac,
        //                    ApplyToGuid = Configuration.AcId
        //                },
        //                BonusValue = new BonusToAdd
        //                {
        //                    FixedValue = armorItem.Item.GetAc(),
        //                },
        //                Description = null,
        //                Name = null,
        //                ParentAbility = null,
        //                Type = BonusType.ArmorBonus
        //            });
        //        }
        //        else if (armorItem.Item.Item.Type == ItemType.Shield)
        //        {
        //            //localBonusList.Add(new Bonus(Guid.NewGuid(), "ac", null, "", "ac",new BonusToAdd(armorItem.Item.AC.GetValueOrDefault(), null, null, null, null, null), BonusType.ShieldBonus, null));
        //            localBonusList.Add(new Bonus
        //            {
        //                ID = Guid.NewGuid(),
        //                ApplyTo = new BonusApplyTo
        //                {
        //                    ApplyToType = BonusApplyToType.Ac,
        //                    ApplyToGuid = Configuration.AcId
        //                },
        //                BonusValue = new BonusToAdd
        //                {
        //                    FixedValue = armorItem.Item.GetAc(),
        //                },
        //                Description = null,
        //                Name = null,
        //                ParentAbility = null,
        //                Type = BonusType.ShieldBonus
        //            });
        //        }
        //        else
        //        {
        //            if (armorItem.Item.Item.Type == ItemType.Other)
        //            {
        //                //localBonusList.Add(new Bonus(Guid.NewGuid(), "ac", null, "", "ac", new BonusToAdd(armorItem.Item.GetAc(), null, null, null, null, null), BonusType.ArmorBonus, null));
        //                localBonusList.Add(new Bonus
        //                {
        //                    ID = Guid.NewGuid(),
        //                    ApplyTo = new BonusApplyTo
        //                    {
        //                        ApplyToType = BonusApplyToType.Ac,
        //                        ApplyToGuid = Configuration.AcId
        //                    },
        //                    BonusValue = new BonusToAdd
        //                    {
        //                        FixedValue = armorItem.Item.GetAc(),
        //                    },
        //                    Description = null,
        //                    Name = null,
        //                    ParentAbility = null,
        //                    Type = BonusType.ArmorBonus
        //                });
        //            }
        //        }
        //    }
        //    return localBonusList.GetMaxValue(bonusDto, typeLimit, true);
        //}

        
        //public int GetArmorCheckPenetly()
        //{
        //    var armorCheckPenelty = 0;

        //    foreach (var itemSlot in _equipedArmorItems)
        //    {
        //        if (itemSlot.IsEmptySlot())
        //        {
        //            continue;
        //        }
        //        if (itemSlot.Item.Item.ArmorCheckPenelty.HasValue)
        //        {
        //            armorCheckPenelty += itemSlot.Item.Item.ArmorCheckPenelty.GetValueOrDefault();
        //        }
        //    }
        //    return armorCheckPenelty;
        //}

        public static CalculatedString GetArmorClass(GetBonusDto bonusDto)
        {
            var maxAc = GetMaxArmorBonus(bonusDto);
            var maxDex = Math.Min(bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiDexId).GetCurrentModifier(bonusDto), GetMaxDexBonus(bonusDto));
            var other = MagicBonueses(bonusDto);
            var size = bonusDto.Character.Race.GetAttackAndAcModifier(bonusDto);

            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("Base", null, 10);
            calcStr.AddIfNotZero("Dexterity", null, maxDex);
            calcStr.AddPartsByRef(maxAc,bonusDto);
            calcStr.AddPartsByRef(other,bonusDto);
            calcStr.AddIfNotZero("Size", null, size);

            return calcStr;
            //return 10 + maxAc + maxDex + size + other;
        }

        private static int GetMaxDexBonus(GetBonusDto bonusDto)
        {
            var maxDexBonus = int.MaxValue;

            foreach (var itemSlot in bonusDto.EquippedItems)
            {
                var localMaxDex = itemSlot.GetMaxDexBonus(bonusDto);
                if (localMaxDex != null &&
                    localMaxDex.GetValueAsInt() < maxDexBonus)
                {
                    maxDexBonus = localMaxDex.GetValueAsInt();
                }
            }
            return maxDexBonus;
        }

        private static List<BonusRef> GetMaxArmorBonus(GetBonusDto bonusDto)
        {
            var armorBonuses = new List<BonusRef>();
            var armor = bonusDto.EquippedItems.Select(x => x.GetArmorAcBonus(bonusDto)).OrderByDescending(x => x.MaxBonuesSum(bonusDto)).FirstOrDefault();
            var shield = bonusDto.EquippedItems.Select(x => x.GetShieldAcBonus(bonusDto)).OrderByDescending(x => x.MaxBonuesSum(bonusDto)).FirstOrDefault();

            if (armor != null)
            {
                armorBonuses.AddRange(armor);
            }
            if (shield != null)
            {
                armorBonuses.AddRange(shield);
            }

            return armorBonuses;
        }

        private static List<BonusRef> MagicBonueses(GetBonusDto bonusDto)
        {
            var other = bonusDto.Bonuses.Where(x => x.ShouldApplyTo(new GameId
            {
                ID = Configuration.AcId,
            }, typeof (ArmorClass)) && x.ShouldApplyToSubType(null)).MaxBonusRefList(bonusDto);
            return other;
        }

        public static CalculatedString GetTouchArmorClass(GetBonusDto bonusDto)
        {
            var touchBonues = bonusDto.Bonuses.Where(x => x.ShouldApplyTo(new GameId
            {
                ID = Configuration.AcId,
            }, typeof (ArmorClass)) && x.ShouldApplyToSubType(Configuration.AcTouchId)).MaxBonusRefList(bonusDto);


            var maxDex = Math.Min(bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiDexId).GetCurrentModifier(bonusDto), GetMaxDexBonus(bonusDto));
            var other = MagicBonueses(bonusDto);
            var size = bonusDto.Character.Race.GetAttackAndAcModifier(bonusDto);

            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("Base", null, 10);
            calcStr.AddIfNotZero("Dexterity", null, maxDex);
            calcStr.AddPartsByRef(touchBonues, bonusDto);
            calcStr.AddPartsByRef(other, bonusDto);
            calcStr.AddIfNotZero("Size", null, size);

            return calcStr;
        }

        public static CalculatedString GetFlatFootedArmorClass(GetBonusDto bonusDto)
        {
            var flatBonus = bonusDto.Bonuses.Where(x => x.ShouldApplyTo(new GameId
            {
                ID = Configuration.AcId,
            }, typeof(ArmorClass)) && x.ShouldApplyToSubType(Configuration.AcFlatFootedId)).MaxBonusRefList(bonusDto);



            var maxAc = GetMaxArmorBonus(bonusDto);
            var other = MagicBonueses(bonusDto);
            var size = bonusDto.Character.Race.GetAttackAndAcModifier(bonusDto);

            var calcStr = new CalculatedString();
            calcStr.AddIfNotZero("Base", null, 10);
            calcStr.AddPartsByRef(flatBonus,bonusDto);
            calcStr.AddPartsByRef(maxAc, bonusDto);
            calcStr.AddPartsByRef(other, bonusDto);
            calcStr.AddIfNotZero("Size", null, size);

            return calcStr;


        }
    }
}
