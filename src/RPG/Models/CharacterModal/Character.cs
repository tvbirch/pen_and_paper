using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Ajax.Utilities;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.Context;
using RPG.Models.CoreModal.DTO;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Alligments;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Conditions;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Lists;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.CharacterModal
{
    public class Character : ElementId
    {
        //Character stats
        public int Xp { get; set; }
        public Alligment Alligment { get; set; }
        public List<AbilityScore> Abilities { get; set; }
        public Race Race { get; set; }
        public HitPoints HitPoints { get; set; }
        public List<Language> Languages { get; set; }  

        public PhysicalAppearance PhysicalAppearance { get; set; }
        public List<ClassLevel> CurrentClasses { get; set; }
        public List<SkillRank> SkillRanks { get; set; }
        public Inventory Inventory { get; set; }

        public List<SpecialAbility> Feats { get; set; }

        public List<SpellSlot> SpellSlots { get; set; }

        [NotMapped]
        public List<Condition> Conditions { get; set; }


        //public SpellBook SpellBook;

        //public CarryingCapacity CarryingCapacity;
        public Round Round { get; set; }
        //public Initiativ Initiativ { get; set; }
        //public Grapple Grapple { get; set; }
        //public SpellResistance SpellResistance { get; set; }
        
        public Character()
        {
        }

        [NotMapped]
        private GetBonusDto _bonusDto { get; set; }

        public void Load()
        {
            _bonusDto = GetBonusDto();
        }

        private GetBonusDto GetBonusDto()
        {
            var dto = new GetBonusDto
            {
                Character = this,
                Round = Round,
                Abilities = Abilities,
                ActiveBonus = new List<BonusRef>(),
                PassiveBonus = new List<BonusRef>(),
                EquippedItems = GetEquippedItems(),
                AllItems = Inventory.BagItems,
                Classes = CurrentClasses,
                Feats = new List<SpecialAbility>(Feats),
            };

            //Conditions
            Round.EnrichActiveBonusDto(dto);
            //Feats
            dto.Feats.AddRange(Race.RacialAbilities);
            dto.Feats.AddRange(
                Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault())
                    .Select(x => x.Item.EnchanmentAbilities)
                    .SelectMany(x => x));

            dto.Feats.AddRange(
                Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault())
                .Select(x => x.EnchanmentAbilities ?? new List<SpecialAbility>() )
                    .SelectMany(x => x));

            foreach (var currentClass in CurrentClasses)
            {
                var currentClassAbis = currentClass.GetAbilitiesAtCurrentLevel();
                dto.Feats.AddRange(currentClassAbis);
            }
            //dto.Feats.AddRange(CurrentClasses.Select(x => x.GetAbilitiesAtCurrentLevel()).SelectMany( x=> x));


            //Passive bonuses
            Race.EnrichPassiveBonusDto(dto);
            PhysicalAppearance.EnrichPassiveBonusDto(dto);
            foreach (var specialAbility in dto.Feats)
            {
                specialAbility.EnrichPassiveBonusDto(dto);
            }

            var itemEquiped = Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault());
            foreach (var ownedItem in itemEquiped)
            {
                foreach (var bonuse in ownedItem.Item.EnchanmentsBonuses)
                {
                    dto.PassiveBonus.Add(new BonusRef(ownedItem.Item,bonuse));
                }
                if (ownedItem.EnchanmentsBonuses != null)
                {
                    foreach (var bonuse in ownedItem.EnchanmentsBonuses)
                    {
                        dto.PassiveBonus.Add(new BonusRef(new ElementId
                        {
                            Name = Name + "'s " + ownedItem.Item.Name
                        }, bonuse));
                    }
                }
                
            }
            //dto.PassiveBonus.AddRange(Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault()).Select(x=> x.Item.EnchanmentsBonuses).SelectMany(x => x));
            
            //Active bonuses
            Race.EnrichActiveBonusDto(dto);
            foreach (var specialAbility in dto.Feats)
            {
                specialAbility.EnrichActiveBonusDto(dto);
            }
            

            //Replaying actions
            foreach (var specialAbility in dto.Feats)
            {
                specialAbility.SetChildReferences();
            }

            dto.Feats = dto.Feats.DistinctBy(x => x.ID).ToList();
            return dto;
        }

        public List<OwnedItem> GetEquippedItems()
        {
            return Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault()).ToList();
        }

        public TimeChangeRemoval ChangeTime(ChangeTimeDto changeTimeDto)
        {
            TimeChangeRemoval actionsToRemove = Round.ChangeTime(new ChangeCharacterTimeDto
                {
                    TimeUnit = changeTimeDto.TimeLimitUnit,
                    Bonus = _bonusDto,
                    TargetHit = changeTimeDto.HitTarget
                });
            return actionsToRemove;
        }
        
        public int GetTotalLevel()
        {
            return CurrentClasses.Sum(x => x.Level);
        }
        public int GetNumberOfAttacks()
        {
            return GetBaseAttack().Count;
        }

        //public int? GetBaseAttackForAttack(int getAttacksTaken)
        //{
        //    var baseAttack = GetBaseAttack();
        //    var babForAttack = baseAttack.ElementAtOrDefault(getAttacksTaken);
        //    var babBonusesList = Tools.GetBonusesApplyingTo(Configuration.BabId, BonusApplyToType.BaseAttack, _bonusDto.Bonuses);
        //    var babBonus = babBonusesList.MaxBonuesSum(_bonusDto);

        //    return babForAttack - babBonus;
        //}

        #region Description
        public string GetClassesAndLevelString()
        {
            if (CurrentClasses == null || CurrentClasses.Count == 0)
            {
                return "";
            }

            return string.Join(" ,", CurrentClasses.Select(x => x.Class.Name + "/" + x.Level).ToArray());
        }

        public string GetRaceString()
        {
            if (Race == null)
            {
                return "";
            }
            return Race.Name;
        }

        public string GetAlignmentString()
        {
            if (Alligment == null)
            {
                return "";
            }
            return Alligment.Name;
        }

        public string GetDeityString()
        {
            return "TODO";
        }

        public string GetSizeString()
        {
            if (Race == null)
            {
                return "";
            }
            return Race.GetCurrentSizeString(_bonusDto);
        }

        public string GetAgeString()
        {
            if (PhysicalAppearance == null || PhysicalAppearance.Age == null)
            {
                return "";
            }
            return PhysicalAppearance.Age.CurrentAge.ToString();
        }

        public string GetGenderString()
        {
            if (PhysicalAppearance == null)
            {
                return "";
            }
            return PhysicalAppearance.Gender;
        }

        public string GetHeightString()
        {
            if (PhysicalAppearance == null || PhysicalAppearance.Height == null)
            {
                return "";
            }
            return PhysicalAppearance.Height.ToString();
        }

        public string GetWeightString()
        {
            if (PhysicalAppearance == null || PhysicalAppearance.Weight == null)
            {
                return "";
            }
            return PhysicalAppearance.Weight.ToString();
        }

        public string GetEyesString()
        {
            if (PhysicalAppearance == null)
            {
                return "";
            }
            return PhysicalAppearance.Eyes;
        }

        public string GetHairString()
        {
            if (PhysicalAppearance == null)
            {
                return "";
            }
            return PhysicalAppearance.Hair;
        }

        public string GetSkinString()
        {
            if (PhysicalAppearance == null)
            {
                return "";
            }
            return PhysicalAppearance.Skin;
        }
        #endregion

        #region actions
        public List<RoundAction> GetPossibleActions()
        {
            return Round.GetPossibleActions(_bonusDto);
        }
        #endregion

        #region abilityscores
        public List<CalculatedAbilityScores> GetAbilities()
        {
            var result = new List<CalculatedAbilityScores>();
            foreach (var ability in Abilities)
            {
                result.Add(new CalculatedAbilityScores
                {
                    Name = ability.Ability.Name,
                    Base = ability.GetBase(_bonusDto),
                    BaseMod = ability.GetBaseModifier(_bonusDto),
                    Current = ability.GetCurrent(_bonusDto),
                    CurrentMod = ability.GetCurrentModifier(_bonusDto)
                });
            }
            return result;
        }
        #endregion

        #region saves
        public List<Save> GetSavingThrows()
        {
            return Saves.GetSaves(_bonusDto);
        }
        #endregion

        #region hp
        public CalculatedString GetMaxHp()
        {
            return HitPoints.GetMaxHp(_bonusDto);
        }

        public CalculatedString GetCurrentHp()
        {
            return HitPoints.GetCurrentHp(_bonusDto);
        }

        public CalculatedString GetNonlethalDamage()
        {
            return HitPoints.GetNonLethal();
        }
        #endregion

        #region speed
        public string GetSpeed()
        {
            return Race.GetCurrentSpeed(_bonusDto).ToString();
        }
        #endregion

        #region AC
        public CalculatedString GetArmorClass()
        {
            return ArmorClass.GetArmorClass(_bonusDto);
        }

        public CalculatedString GetTouchArmorClass()
        {
            return ArmorClass.GetTouchArmorClass(_bonusDto);
        }

        public CalculatedString GetFlettFootedArmorClass()
        {
            return ArmorClass.GetFlatFootedArmorClass(_bonusDto);
        }
        #endregion

        #region dmgreduction
        public CalculatedString GetDamageReduction()
        {
            return DamageRecuction.GetDamageReduction(_bonusDto);
        }
        #endregion

        #region baseattack
        public string GetBaseAttackString()
        {
            var attacks = GetBaseAttack();
            var builder = new StringBuilder();
            foreach (var attack in attacks)
            {
                builder.AppendFormat("{0} / ", attack);
            }
            builder.Length = builder.Length - 3;
            return builder.ToString();
        }
        public string GetBaseAttackCalculationString()
        {
            return CurrentClasses.Select(x => x.GetBaseAttackBonus() + "("+x.Class.Name+")").ToList().Aggregate((current, next) => current + " + " + next);
        }

        public List<int> GetBaseAttack()
        {
            var totalBab = CurrentClasses.Sum(x => x.GetBaseAttackBonus());
            var attacks = new List<int>();
            for (var i = totalBab; i > 0; i -= 5)
            {
                attacks.Add(i);
            }
            if (_bonusDto != null && attacks.Any())
            {
                foreach (var bonusRef in _bonusDto.Bonuses)
                {
                    if (bonusRef.Bonues != null && 
                        bonusRef.Bonues.Type == BonusType.ExtraAttackAtFullBab)
                    {
                        if (!bonusRef.IsActive(_bonusDto))
                        {
                            continue;
                        }
                        var amount = 0;
                        if (bonusRef.Bonues != null && bonusRef.Bonues.BonusValue != null && bonusRef.Bonues.BonusValue.FixedValue.HasValue)
                        {
                            amount = bonusRef.Bonues.BonusValue.FixedValue.Value;
                        }
                        for (int i = 0; i < amount; i++)
                        {
                            attacks.Insert(0, attacks[0]);
                        }
                    }
                }
                
            }
            return attacks;
        }
        #endregion

        #region initiativ
        public string GetInitiativString()
        {
            return Initiativ.GetInitiativBonus(_bonusDto).ToString();
        }
        #endregion

        #region grapple
        public CalculatedString GetGrappleString()
        {
            return Grapple.GetGrappleBonus(_bonusDto);
        }
        #endregion

        public List<CalculatedSkill> GetSkills()
        {
            var calcedSkills = new List<CalculatedSkill>();

            var shieldPenelty = GetShieldPenelty(_bonusDto);

            foreach (var currentSkills in SkillRanks)
            {
                var skillAbi = _bonusDto.Abilities.First(y => y.Ability.ID == currentSkills.Skill.SkillModifier.ID).Ability;
                var result = new CalculatedSkill
                {
                    Ability = skillAbi.Name,
                    //AbilityBonues =
                    //    _bonusDto.Abilities.First(y => y.Ability.ID == currentSkills.Skill.SkillModifier.ID)
                    //        .GetCurrentModifier(_bonusDto),
                    SkillRanks = currentSkills.Ranks,
                    IsClassSkill = CurrentClasses.Any(y => y.Class.ClassSkills.Any(z => z.ID == currentSkills.Skill.ID)),
                    Name = currentSkills.Skill.Name,
                    UseUntrained = currentSkills.Skill.UseUntrained,
                };
                var skillBonuses = _bonusDto.Bonuses.Where(y => y.ShouldApplyTo(currentSkills, typeof(SkillRank)) && y.ShouldApplyToSubType(skillAbi.ID));

                result.Ranks = new CalculatedString();
                var abiScore = _bonusDto.Abilities.First(y => y.Ability.ID == currentSkills.Skill.SkillModifier.ID);
                result.Ranks.AddIfNotZero(abiScore.Ability.Name,null, abiScore.GetCurrentModifier(_bonusDto));
                result.Ranks.AddIfNotZero("Ranks", null, currentSkills.Ranks);
                result.Ranks.AddPartsByRef(skillBonuses.ToList(), _bonusDto);


                if (currentSkills.Skill.SynergiApplyTo != null && currentSkills.Skill.SynergiApplyTo.Count != 0)
                {
                    foreach (var skillSynergi in currentSkills.Skill.SynergiApplyTo)
                    {
                        var skillRanksForSynergi = SkillRanks.FirstOrDefault(x => x.Skill.ID == skillSynergi.SynergiFrom.ID);
                        if (skillRanksForSynergi != null && skillRanksForSynergi.Ranks >= 5)
                        {
                            result.Ranks.AddIfNotZero(string.Format("Synergi ({0})", skillRanksForSynergi.Skill.Name), skillSynergi.Condition, 2);
                        }
                    }
                }

                if (currentSkills.Skill.ArmorCheckPeneltyApply)
                {
                    foreach (var equippedItem in _bonusDto.EquippedItems)
                    {
                        var localcalcstr = equippedItem.GetArmorCheckPenelty(_bonusDto);
                        var totalCheckPenelty = Math.Min(localcalcstr.GetValueAsInt(),0);
                        if (currentSkills.Skill.ArmorCheckPeneltyApplyDouble)
                        {
                            totalCheckPenelty = totalCheckPenelty * 2;
                        }
                        result.Ranks.AddIfNotZero(string.Format("Armor check penelty({0})",equippedItem.Item.Name), null, totalCheckPenelty);
                    }
                }

                if (currentSkills.Skill.RequiresMovement)
                {
                    result.Ranks.AddIfNotZero("Shield proficiency penelty",null,shieldPenelty);
                }

                calcedSkills.Add(result);
            }

            return calcedSkills.Where(x => x.SkillRanks > 0 || x.UseUntrained).ToList();
        }

        #region items
        public List<BagItem> GetBagItems()
        {
            var items =
                Inventory.BagItems.Where(x => !x.IsEquipped.GetValueOrDefault())
                    .GroupBy(y => y.GetName())
                    .Select(z => new BagItem
                    {
                        Id = z.First().ID,
                        Name = z.First().Item.Name,
                        Description = z.First().Item.Description,
                        NumberOfItems = z.Count(),
                        Weight = new ImperialWeight(z.Sum(weight => weight.GetWeight(_bonusDto)))
                    }).ToList();
            return items;
        }

        public List<EquipedItem> GetEquipedItems()
        {
            var items =
                Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault())
                    .Select(z => new EquipedItem
                    {
                        Id = z.ID,
                        Name = z.Item.Name,
                        Description = z.Item.Description,
                        Slot = z.Item.RequiresSlots,
                    }).ToList();
            return items;
        }
        
        public List<ItemSlotRequirement> GetEmptySlots()
        {
            var itemGrantedSlots =
                Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault())
                    .Select(z => z.Item.HasSlots.Select(y => y.Requirement)).ToList();


            var slots = itemGrantedSlots.SelectMany(itemGrantedSlot => itemGrantedSlot).ToList();
            slots.Add(ItemSlotRequirement.Arms);
            slots.Add(ItemSlotRequirement.Body);
            slots.Add(ItemSlotRequirement.Face);
            slots.Add(ItemSlotRequirement.Feet);
            slots.Add(ItemSlotRequirement.Hands);
            slots.Add(ItemSlotRequirement.Head);
            slots.Add(ItemSlotRequirement.Misc);
            slots.Add(ItemSlotRequirement.Ring);
            slots.Add(ItemSlotRequirement.Ring);
            slots.Add(ItemSlotRequirement.Shoulder);
            slots.Add(ItemSlotRequirement.Throat);
            slots.Add(ItemSlotRequirement.Torso);
            slots.Add(ItemSlotRequirement.Waist);
            slots.Add(ItemSlotRequirement.WeaponHand);
            slots.Add(ItemSlotRequirement.WeaponHand);

            var slotsToRemove = Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault())
                .Select(z => z.Item.RequiresSlots.Select(y => y.Requirement))
                .ToList()
                .SelectMany(itemReqSlot => itemReqSlot)
                .ToList();

            foreach (var itemSlotRequirement in slotsToRemove)
            {
                if (itemSlotRequirement != ItemSlotRequirement.Misc)
                {
                    slots.Remove(itemSlotRequirement);
                }
            }

            return slots;
        }
        #endregion

        public CharacterLoad GetLoad()
        {
            var load = new CharacterLoad
            {
                CurrentLoad = (int) CarryingCapacity.GetCurrentLoad(_bonusDto),
                HeavyLoad = CarryingCapacity.GetHeavyLoad(_bonusDto),
                MediumLoad = CarryingCapacity.GetMediumLoad(_bonusDto),
                LightLoad = CarryingCapacity.GetLightLoad(_bonusDto),
            };

            load.IsLightLoad = load.CurrentLoad <= load.LightLoad;
            load.IsMediumLoad = load.CurrentLoad > load.LightLoad && load.CurrentLoad <= load.MediumLoad;
            load.IsHeavyLoad = load.CurrentLoad > load.LightLoad && load.CurrentLoad > load.MediumLoad;
            load.LiftOverHeadLoad= load.HeavyLoad;
            load.LiftOverGroundLoad = load.HeavyLoad * 2;
            load.PushOrDrag = load.HeavyLoad * 5;
            return load;
        }

        public List<ElementId> GetOtherAbilities()
        {
            var usableAbis = new List<ElementId>();
            foreach (var spb in _bonusDto.Feats)
            {
                if (spb.IsLimited()) continue;
                usableAbis.Add(new ElementId
                {
                    Name = spb.GetName(_bonusDto),
                    Description = spb.Description,
                    ID = spb.ID,
                });

            }
            return usableAbis;
        }
        public List<UsableAbility> GetUsableAbilities()
        {
            var usableAbis = new List<UsableAbility>();
            foreach (var spb in _bonusDto.Feats)
            {
                if (!spb.IsLimited()) continue;
                if (!spb.IsRequiredAbilityActive(_bonusDto)) continue;

                usableAbis.Add(new UsableAbility
                {
                    Name = spb.GetName(_bonusDto),
                    IsCurrentlyUseable = spb.IsCurrentlyUseable(_bonusDto),
                    IsActive = spb.IsActive(_bonusDto),
                    Description = spb.Description,
                    Id = spb.ID,
                });
                
            }
            return usableAbis;
        }

        public void TriggerAbility(Guid abilityId, int charges)
        {
            foreach (var spb in _bonusDto.Feats)
            {
                if (spb.ID == abilityId)
                {
                    spb.UseChargeIfPossible(charges, _bonusDto);  
                    return;
                }
            }
        }

        public void TakeDamage(bool normalDamage, DamageType damageType, int amount, bool ignorreDmgReduction = false)
        {
            if (normalDamage)
            {
                HitPoints.TakeDamage(damageType, amount, _bonusDto, ignorreDmgReduction);
            }
            else
            {
                HitPoints.TakeNonLethalDamage(damageType, amount, _bonusDto, ignorreDmgReduction);
            }
        }

        public void HealDamage(int amount)
        {
            HitPoints.Heal(amount,_bonusDto);
        }

        #region attack
        private IEnumerable<OwnedItem> GetAttackItems()
        {
            var attackItems = Inventory.BagItems.Where(x => x.IsEquipped.GetValueOrDefault() && x.CanAttack()).ToList();
            return attackItems;
        } 
        public OwnedItem GetPrimaryWeapon()
        {
            var weapons = GetAttackItems().Where(x => x.Item.RequiresSlots.Any(y => y.Requirement == ItemSlotRequirement.WeaponHand)).ToList();
            var firstWeaponHand = weapons.FirstOrDefault();
            return firstWeaponHand;
        }
        public OwnedItem GetOffHandWeapon()
        {
            var weapons = GetAttackItems().Where(x => x.Item.RequiresSlots.Any(y => y.Requirement == ItemSlotRequirement.WeaponHand)).ToList();
            var secoundWeaponHands = weapons.Skip(1).FirstOrDefault();
            return secoundWeaponHands;
        }

        public int GetCurrentMaxBaseAttack()
        {
            var bab = 0;
            var attackItems = GetAttackItems();
            var firstWeaponHand = GetPrimaryWeapon();
            var secoundWeaponHands = GetOffHandWeapon();

            var primaryWeaponId = firstWeaponHand == null ? (Guid?) null : firstWeaponHand.ID;
            var offHandWeaponId = secoundWeaponHands == null ? (Guid?) null : secoundWeaponHands.ID;

            foreach (var attackItem in attackItems)
            {
                var currentbab = Round.GetBaseAttackWithWeapon(attackItem.ID, GetBaseAttack(), primaryWeaponId, offHandWeaponId);
                bab = Math.Max(bab, currentbab.GetValueOrDefault());
            }
            return bab;
        }
        public List<AttackItem> GetAttacks()
        {
            var attacks = new List<AttackItem>();
            var attackItems = GetAttackItems();

            var firstWeaponHand = GetPrimaryWeapon();
            var secoundWeaponHands = GetOffHandWeapon();

            var primaryWeaponId = firstWeaponHand == null ? (Guid?) null : firstWeaponHand.ID;
            var offHandWeaponId = secoundWeaponHands == null ? (Guid?) null : secoundWeaponHands.ID;

            //Adding item attacks
            foreach (var equipedItem in attackItems)
            {
                var canCurrentlyAttack = Round.CanWeaponCurrentlyAttack(equipedItem.ID, GetBaseAttack(), primaryWeaponId, offHandWeaponId);
                
                attacks.Add(new AttackItem
                {
                    Id = equipedItem.ID,
                    Name = equipedItem.GetName(primaryWeaponId, offHandWeaponId),
                    ActionToUse = Round.GetActionToUseWeapon(equipedItem.ID, GetNumberOfAttacks(), primaryWeaponId, offHandWeaponId),
                    AttackBonus = equipedItem.GetAttackBonus(_bonusDto,
                        GetCurrentEquipmentPenelty(equipedItem, firstWeaponHand, secoundWeaponHands),
                        Round.GetBaseAttackWithWeapon(equipedItem.ID, GetBaseAttack(), primaryWeaponId, offHandWeaponId),
                        GetItemProficiencyPenelty(_bonusDto,equipedItem),
                        GetShieldPenelty(_bonusDto)),
                    Damage = equipedItem.GetDamageString(_bonusDto),
                    AttackType = equipedItem.GetAttackType(),
                    CriticalRange = equipedItem.GetCriticalRange(_bonusDto),
                    Range = equipedItem.GetRange(_bonusDto),
                    CanCurrentlyAttack = canCurrentlyAttack,
                });
            }

            //Adding other attacks
            return attacks.OrderByDescending(x => x.CanCurrentlyAttack).ToList();
        }

        private int GetShieldPenelty(GetBonusDto bonusDto)
        {
            var shields = bonusDto.EquippedItems.Where(x => x.Item != null && x.Item.Type == ItemType.Shield).ToList();
            var penelty = 0;
            foreach (var shield in shields)
            {
                if (shield.Item == null)
                {
                    continue;
                }
                if (shield.Item.RequiresAbility == null || shield.Item.RequiresAbility.Count == 0)
                {
                    continue;
                }
                var foundAbilities = bonusDto.Feats.Where(x => shield.Item.RequiresAbility.Select(y => y.SpecialAbilityGuid).Contains(x.ID))
                    .ToList();
                if (foundAbilities.Count != shield.Item.RequiresAbility.Count || foundAbilities.Any(x => !x.IsActive(bonusDto)))
                {
                    penelty += shield.GetArmorCheckPenelty(bonusDto).GetValueAsInt();
                }
            }
            return -penelty;
        }

        private int GetItemProficiencyPenelty(GetBonusDto bonusDto, OwnedItem equipedItem)
        {
            if (equipedItem == null || equipedItem.Item == null || equipedItem.Item.RequiresAbility == null || equipedItem.Item.RequiresAbility.Count == 0)
            {
                return 0;
            }
            if (equipedItem.Item.Type != ItemType.Weapon)
            {
                return 0;
            }

            var foundAbilities =
                bonusDto.Feats.Where(
                    x => equipedItem.Item.RequiresAbility.Select(y => y.SpecialAbilityGuid).Contains(x.ID))
                    .ToList();
            if (foundAbilities.Count != equipedItem.Item.RequiresAbility.Count)
            {
                return -4;
            }
            if (foundAbilities.Any(x => !x.IsActive(bonusDto)))
            {
                return -4;
            }
            return 0;
        }

        private int GetCurrentEquipmentPenelty(OwnedItem currentItem, OwnedItem firstWeaponHand, OwnedItem secoundWeaponHand)
        {
            var penelty = 0;
            var numberOfWeapons = firstWeaponHand == null ? 0 : secoundWeaponHand == null ? 1 : 2;
            var isOffHandLight = secoundWeaponHand == null || secoundWeaponHand.Item.IsLightWeapon;

            //Handle two weapon fighting
            //Handle natural weapons. After Attacking with weapons, you can attack with natural attack at -5.


            if (numberOfWeapons > 1)
            {
                if (currentItem.ID == secoundWeaponHand.ID)
                {
                    penelty = -10;
                }
                else if (currentItem.ID == firstWeaponHand.ID)
                {
                    penelty = -6;
                }
                /*else if (currentItem.Item.RequiresSlots.TrueForAll(x => x.Requirement != ItemSlotRequirement.WeaponHand)) // not enough. Could be graft. Need property "natural weapon"
                {
                    //assuming third naturalAttack
                    penelty = ?;
                    return penelty;
                }*/
            }
            if (penelty != 0 && isOffHandLight)
            {
                penelty += 2;
            }

            return penelty;
        }

        public void AttackWithItem(Guid weaponId, int? nrOfEnemies)
        {
            var firstWeaponHand = GetPrimaryWeapon();
            var secoundWeaponHands = GetOffHandWeapon();

            var primaryWeaponId = firstWeaponHand == null ? (Guid?)null : firstWeaponHand.ID;
            var offHandWeaponId = secoundWeaponHands == null ? (Guid?)null : secoundWeaponHands.ID;

            var actionToUse = Round.GetActionToUseWeapon(weaponId, GetNumberOfAttacks(), primaryWeaponId, offHandWeaponId);
            if (!actionToUse.HasValue)
            {
                return;
            }
            if (nrOfEnemies.HasValue && nrOfEnemies != 0)
            {
                var onHitFeats = _bonusDto.Feats.Where(x => x.Limit != null && x.Limit.Amount != null &&
                    x.Limit.Amount.ActionRequired == RoundAction.AutoOnHit).ToList();
                foreach (var specialAbility in onHitFeats)
                {
                    for (int i = 0; i < nrOfEnemies.Value; i++)
                    {
                        //TODO: 100 should be amount om dmg given
                        specialAbility.UseChargeIfPossible(100, _bonusDto);
                    }
                }
            }

            Round.TakeAction(actionToUse.Value, weaponId);
        }
        #endregion

        #region spells
        public int GetCasterLevelForClass(Guid id)
        {
            var classBase = _bonusDto.Classes.First(x => x.Class.ID == id);
            return classBase.GetCasterLevel(_bonusDto);
        }

        public int GetBonusSpellsPrLevel(int level, Ability abilityUsedForCasting)
        {
            if (level == 0)
            {
                return 0;
            }
            var abilityMod = Abilities.First(x => x.Ability.ID == abilityUsedForCasting.ID).GetCurrentModifier(_bonusDto);
            var effectivModifier = abilityMod - level + 1;
            if (effectivModifier < 0)
            {
                return 0;
            }
            var bonusSpells = Math.Ceiling((double)effectivModifier/4.0);
            var asInt = (int) bonusSpells;
            return asInt;
        }

        public int GetSpellDcPrLevel(int level, Ability abilityUsedForCasting)
        {
            //TODO check feats for extra casterlevel, and present in paranteses
            var abilityMod = Abilities.First(x => x.Ability.ID == abilityUsedForCasting.ID).GetCurrentModifier(_bonusDto);
            return 10 + level + abilityMod;
        }
        public SpellInSlot[] GetSpellsSlots(int spellLevel, Guid classId)
        {
            var nrOfSlots = GetNumberOfSpellSlotsForClass(spellLevel, classId);
            var slotsList = new SpellInSlot[nrOfSlots];
            if (SpellSlots == null)
            {
                return slotsList;
            }
            var spellSlotsForClass = SpellSlots.Where(x => x.SpellChosenForClass.ID == classId).ToList();
            var spellForCurrentLevel = spellSlotsForClass.Where(x => x.Spell.CasterRequirements.Any(y => y.CasterClass.ID == classId && y.CasterLevel == spellLevel)).ToList();
            for (int i = 0 ; i < spellForCurrentLevel.Count; ++i)
            {
                slotsList[i] = new SpellInSlot
                {
                    Components = spellForCurrentLevel[i].Spell.GetComponentsString(),
                    Duration = "TODO",
                    Id = spellForCurrentLevel[i].ID,
                    Name = spellForCurrentLevel[i].Spell.Name,
                    Range = spellForCurrentLevel[i].Spell.GetRangeString(),
                    SR = spellForCurrentLevel[i].Spell.GetSpellResistanceString(),
                    Save = spellForCurrentLevel[i].Spell.GetSave(),
                    School = spellForCurrentLevel[i].Spell.School.Name,
                    TargetEffectArea = spellForCurrentLevel[i].Spell.GetTarget(),
                    Time = spellForCurrentLevel[i].Spell.CastingTime.ToString(),
                    SlotSpend = spellForCurrentLevel[i].Used,
                    CurrentlyUsable = !spellForCurrentLevel[i].Used && spellForCurrentLevel[i].IsUsableInCurrentRound(Round.GetPossibleActions(_bonusDto))
                };
            }
            return slotsList;
        }

        public int GetNumberOfSpellSlotsForClass(int spellLevel, Guid classId)
        {
            var casterLevel = GetCasterLevelForClass(classId);
            var classBase = CurrentClasses.First(x => x.Class.ID == classId);
            return classBase.Class.SpellPrDayDictonary[casterLevel][spellLevel] + GetBonusSpellsPrLevel(spellLevel, classBase.Class.AbilityUsedForCasting);
        }

        #endregion

        #region Condtions
        public void AddCondtion(Guid conditionId)
        {
            var condition = Conditions.FirstOrDefault(x => x.ID == conditionId);
            if (condition != null)
            {
                Round.ApplyCondition(condition,null);
            }
        }

        public void RemoveCondtion(Guid conditionId)
        {
            Round.ManullyRemoveCondition(conditionId);
        }
        #endregion
    }
}
