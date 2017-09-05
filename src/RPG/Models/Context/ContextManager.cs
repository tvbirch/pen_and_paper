using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.GmModal.World;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Alligments;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Conditions;
using RPG.Models.RulebookModal.BaseTypes.Currency;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Rounds;
using WebGrease.Css.Extensions;

namespace RPG.Models.Context
{
    public class ContextManager
    {
        public GameContext Context { get; private set; }

        public ContextManager()
        {
            Context = new GameContext();
        }

        public void Delete<T>(Guid id) where T : GameId
        {
            DeleteNoSave<T>(id);
            Context.SaveChanges();       
        }
        private void DeleteNoSave<T>(Guid id) where T : GameId
        {
            var existing = Get<T>(id);
            if (existing != null)
            {
                if (existing.GetType() == typeof(Bonus))
                {
                    var bonues = existing as Bonus;
                    if (bonues != null)
                    {
                        if (bonues.BonusValue != null)
                        {
                            DeleteNoSave<BonusToAdd>(bonues.BonusValue.ID);
                        }
                    }
                }
                if (existing.GetType() == typeof(BonusToAdd))
                {
                    var bonues = existing as BonusToAdd;
                    if (bonues != null)
                    {
                        if (bonues.ClassProgression != null)
                        {
                            bonues.ClassProgression.ForEach(x => DeleteNoSave<BonusToAddClassProgression>(x.ID));
                        }
                        if (bonues.Dice != null)
                        {
                            DeleteNoSave<DiceRoll>(bonues.Dice.ID);
                        }
                    }
                }
                if (existing.GetType() == typeof(Map))
                {
                    var map = existing as Map;
                    if (map != null)
                    {
                        if (map.Parts != null)
                        {
                            map.Parts.ToList().ForEach(x => DeleteNoSave<MapPart>(x.ID));
                        }
                    }
                }
                if (existing.GetType() == typeof(Race))
                {
                    var race = existing as Race;
                    if (race != null)
                    {
                        if (race.AgeCategoryAtAge != null)
                        {
                            race.AgeCategoryAtAge.ForEach(x => DeleteNoSave<AgeCategoryAtAge>(x.ID));
                        }
                    }
                }

                if (existing.GetType() == typeof(Skill))
                {
                    var skill = existing as Skill;
                    if (skill != null)
                    {
                        if (skill.SynergiApplyTo != null)
                        {
                            skill.SynergiApplyTo.ForEach(x => DeleteNoSave<SkillSynergi>(x.ID));
                        }
                        if (skill.SynergiFrom != null)
                        {
                            skill.SynergiFrom.ForEach(x => DeleteNoSave<SkillSynergi>(x.ID));
                        }
                    }
                }

                Context.Set<T>().Remove(existing);
            }
        }

        public List<T> GetList<T>(Guid[] ids) where T : GameId
        {
            var result = new List<T>();
            if (ids == null)
            {
                return result;
            }
            foreach (var id in ids)
            {
                result.Add(Get<T>(id));
            }
            return result;
        }

        public T Get<T>(Guid id) where T : GameId
        {
            if (typeof(T) == typeof(AbilityByClassLevel))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as AbilityByClassLevel;
                typedT.Ability = Get<SpecialAbility>(typedT.Ability.ID);
                return typedT as T;
            }
            if (typeof(T) == typeof(Bonus))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as Bonus;
                typedT.BonusValue = Get<BonusToAdd>(typedT.BonusValue.ID);
                return typedT as T;
            }

            if (typeof(T) == typeof(BonusFromCharges))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                return item as T;
            }
            if (typeof(T) == typeof(BonusToAdd))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as BonusToAdd;
                typedT.ClassProgression = GetList<BonusToAddClassProgression>(typedT.ClassProgression.Select(x=> x.ID).ToArray());
                return typedT as T;
            }

            if (typeof(T) == typeof(Character))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as Character;

                if (typedT.Abilities == null)
                {
                    typedT.Abilities = new List<AbilityScore>(); 
                    //Context.SaveChanges();
                }
                typedT.Abilities = GetList<AbilityScore>(typedT.Abilities.Select(x => x.ID).ToArray());

                if (typedT.CurrentClasses == null)
                {
                    typedT.CurrentClasses = new List<ClassLevel>();
                    //Context.SaveChanges();
                }
                typedT.CurrentClasses = GetList<ClassLevel>(typedT.CurrentClasses.Select(x => x.ID).ToArray());

                if (typedT.Feats == null)
                {
                    typedT.Feats = new List<SpecialAbility>();
                    //Context.SaveChanges();
                }
                typedT.Feats = GetList<SpecialAbility>(typedT.Feats.Select(x => x.ID).ToArray());

                if (typedT.Inventory == null)
                {
                    typedT.Inventory = new Inventory();
                    //Context.SaveChanges();
                }
                if (typedT.Inventory.BagItems == null)
                {
                    typedT.Inventory.BagItems = new List<OwnedItem>();
                    //Context.SaveChanges();
                }
                typedT.Inventory.BagItems = GetList<OwnedItem>(typedT.Inventory.BagItems.Select(x => x.ID).ToArray());

                typedT.Race = Get<Race>(typedT.Race.ID);

                if (typedT.Round == null)
                {
                    typedT.Round = new Round();
                    //Context.SaveChanges();
                }
                else
                {
                    typedT.Round = Get<Round>(typedT.Round.ID);
                }
                

                if (typedT.SkillRanks == null)
                {
                    typedT.SkillRanks = new List<SkillRank>();
                    //Context.SaveChanges();
                }
                typedT.SkillRanks = GetList<SkillRank>(typedT.SkillRanks.Select(x => x.ID).ToArray());
                if (typedT.SpellSlots == null)
                {
                    typedT.SpellSlots = new List<SpellSlot>();
                    //Context.SaveChanges();
                }
                typedT.SpellSlots = GetList<SpellSlot>(typedT.SpellSlots.Select(x => x.ID).ToArray());

                return typedT as T;
            }

            if (typeof(T) == typeof(ClassBase))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as ClassBase;
                typedT.SaveBonusRate = GetList<SaveRate>(typedT.SaveBonusRate.Select(x => x.ID).ToArray());
                typedT.ClassAbilities = GetList<AbilityByClassLevel>(typedT.ClassAbilities.Select(x => x.ID).ToArray());
                typedT.ClassSkills = GetList<Skill>(typedT.ClassSkills.Select(x => x.ID).ToArray());
                
                return typedT as T;
            }

            if (typeof(T) == typeof(ClassLevel))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as ClassLevel;
                typedT.Class = Get<ClassBase>(typedT.Class.ID);
                return typedT as T;
            }

            if (typeof(T) == typeof(Condition))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as Condition;
                typedT.Bonuses = GetList<Bonus>(typedT.Bonuses.Select(x => x.ID).ToArray());
                if (typedT.IfAlreadyActiveApplyCondition != null)
                    typedT.IfAlreadyActiveApplyCondition = Get<Condition>(typedT.IfAlreadyActiveApplyCondition.ID);
                return typedT as T;
            }

            if (typeof(T) == typeof(ItemBase))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as ItemBase;
                if (typedT.Material != null)
                    typedT.Material = Get<ItemMaterial>(typedT.Material.ID);
                typedT.EnchanmentAbilities = GetList<SpecialAbility>(typedT.EnchanmentAbilities.Select(x => x.ID).ToArray());
                typedT.EnchanmentsBonuses = GetList<Bonus>(typedT.EnchanmentsBonuses.Select(x => x.ID).ToArray());

                return typedT as T;
            }

            if (typeof(T) == typeof(ItemMaterial))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as ItemMaterial;
                typedT.MaterialBonues = GetList<MaterialBonuses>(typedT.MaterialBonues.Select(x => x.ID).ToArray());

                return typedT as T;
            }

            if (typeof(T) == typeof(Language))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                return item as T;
            }
            if (typeof(T) == typeof(Location))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as Location;
                if (typedT.LocationType != null)
                    typedT.LocationType = Get<LocationType>(typedT.LocationType.ID);
                if (typedT.Map != null)
                    typedT.Map = Get<Map>(typedT.Map.ID);
                return typedT as T;
            }

            if (typeof(T) == typeof(Map))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as Map;
                typedT.Parts = GetList<MapPart>(typedT.Parts.Select(x => x.ID).ToArray());

                return typedT as T;
            }
            if (typeof(T) == typeof(MaterialBonuses))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as MaterialBonuses;
                typedT.Bonuses = GetList<Bonus>(typedT.Bonuses.Select(x => x.ID).ToArray());

                return typedT as T;
            }
            if (typeof(T) == typeof(NPC))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as NPC;
                if(typedT.Location != null)
                    typedT.Location = Get<Location>(typedT.Location.ID);

                return typedT as T;
            }

            if (typeof(T) == typeof(OwnedItem))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as OwnedItem;
                typedT.Item = Get<ItemBase>(typedT.Item.ID);
                if (typedT.OwnerMaterial != null)
                    typedT.OwnerMaterial = Get<ItemMaterial>(typedT.OwnerMaterial.ID);
                typedT.EnchanmentAbilities = GetList<SpecialAbility>(typedT.EnchanmentAbilities.Select(x => x.ID).ToArray());
                typedT.EnchanmentsBonuses = GetList<Bonus>(typedT.EnchanmentsBonuses.Select(x => x.ID).ToArray());

                return typedT as T;
            }

            if (typeof(T) == typeof(Race))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as Race;
                typedT.BonusLanguages = GetList<Language>(typedT.BonusLanguages.Select(x => x.ID).ToArray());
                typedT.FavoredClasses = GetList<ClassBase>(typedT.FavoredClasses.Select(x => x.ID).ToArray());
                typedT.RaceBonuses = GetList<Bonus>(typedT.RaceBonuses.Select(x => x.ID).ToArray());
                typedT.RacialAbilities = GetList<SpecialAbility>(typedT.RacialAbilities.Select(x => x.ID).ToArray());
                return item as T;
            }
            if (typeof(T) == typeof(Round))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as Round;
                typedT.ActivatedAbilities = GetList<RoundActivateAbilities>(typedT.ActivatedAbilities.Select(x => x.ID).ToArray());
                typedT.UsedActions = GetList<RoundActionTaken>(typedT.UsedActions.Select(x => x.ID).ToArray());
                typedT.ActivatedConditions = GetList<RoundActivateConditions>(typedT.ActivatedConditions.Select(x => x.ID).ToArray());
                return item as T;
            }
            if (typeof(T) == typeof(RoundActivateAbilities))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as RoundActivateAbilities;
                typedT.ActiveTime = GetList<TimeLimitUnitParsed>(typedT.ActiveTime.Select(x => x.ID).ToArray());
                return item as T;
            }

            if (typeof(T) == typeof(SaveRate))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                return item as T;
            }

            if (typeof(T) == typeof(SpecialAbility))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as SpecialAbility;
                if (typedT.Limit != null && typedT.Limit.Amount != null)
                {
                    typedT.Limit.Amount.ClassProgression = GetList<UsableAmountClassProgression>(typedT.Limit.Amount.ClassProgression.Select(x => x.ID).ToArray());
                }
                typedT.BonusFromCharges = GetList<BonusFromCharges>(typedT.BonusFromCharges.Select(x => x.ID).ToArray());
                typedT.Bonuses = GetList<Bonus>(typedT.Bonuses.Select(x => x.ID).ToArray());
                if (typedT.ApplyConditionOnActivate != null)
                {
                    typedT.ApplyConditionOnActivate = Get<Condition>(typedT.ApplyConditionOnActivate.ID);
                }
                if (typedT.ApplyConditionOnDeactivate != null)
                {
                    typedT.ApplyConditionOnDeactivate = Get<Condition>(typedT.ApplyConditionOnDeactivate.ID);
                }

                return item as T;
            }
            
            if (typeof(T) == typeof(Skill))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                return item as T;
            }

            if (typeof (T) == typeof (SkillRank))
            {
                var item = GetQueryable<T>().First(x => x.ID == id);
                var typedT = item as SkillRank;
                typedT.Skill = Get<Skill>(typedT.Skill.ID);
                return item as T;
            }

            var itemLeft = GetQueryable<T>().First(x => x.ID == id);
            return itemLeft;
        }
        public IQueryable<T> GetQueryable<T>() where T : GameId
        {

            if (typeof(T) == typeof(AbilityScore))
            {
                var spl = Context.AbilityScores.Include(s => s.Ability);
                return spl as IQueryable<T>;
            }
            if (typeof(T) == typeof(AbilityByClassLevel))
            {
                var spl = Context.AbilityByClassLevels
                    .Include(s => s.Ability)
                    .Include(s => s.Class);
                return spl as IQueryable<T>;
            }
            if (typeof(T) == typeof(RoundActivateAbilities))
            {
                var raa = Context.RoundActivateAbilities
                    .Include(r => r.ActiveTime);

                return raa as IQueryable<T>;
            }
            if (typeof(T) == typeof(Bonus))
            {
                var bonus = Context.Bonuses
                    .Include(b => b.BonusValue)
                    .Include(b => b.BonusValue.Dice)
                    .Include(b => b.BonusValue.Dice.Dice)
                    .Include(b => b.BonusValue.AbilityModifyer)
                    .Include(b => b.BonusValue.ClassProgression);
                    //.Include("BonusValue.ClassProgression.ClassProgression")
                    //.Include("BonusValue.ClassProgression.Bonues")
                    //.Include("BonusValue.ClassProgression.Bonues.Dice")
                    //.Include(b => b.BonusValue.Dice);
                return bonus as IQueryable<T>;
            }
            if (typeof (T) == typeof (BonusFromCharges))
            {
                var bonus = Context.BonusFromCharges
                    .Include(b => b.Bonus)
                    .Include(b => b.Bonus.Dice);
                return bonus as IQueryable<T>;
            }
            if (typeof(T) == typeof(BonusToAdd))
            {
                var bonus = Context.BonusToAdds
                    .Include(b => b.AbilityModifyer)
                    .Include(b => b.ClassProgression)
                    //.Include("BonusValue.ClassProgression.ClassProgression")
                    //.Include("BonusValue.ClassProgression.Bonues")
                    //.Include("BonusValue.ClassProgression.Bonues.Dice")
                    .Include(b => b.Dice);
                return bonus as IQueryable<T>;
            }
            if (typeof(T) == typeof(BonusToAddClassProgression))
            {
                var bonus = Context.BonusToAddClassProgressions
                    .Include(b => b.Bonues)
                    .Include(b => b.Bonues.Dice)
                    .Include(b => b.ClassProgression);
                return bonus as IQueryable<T>;
            }
            if (typeof (T) == typeof (Condition))
            {
                var spl = Context.Conditions.Include(s => s.Bonuses).Include(s => s.IfAlreadyActiveApplyCondition);
                return spl as IQueryable<T>;
            }
            if (typeof(T) == typeof(ClassBase))
            {
                var bonus = Context.Classes
                    .Include(b => b.AllowedAlligments)
                    .Include(b => b.ClassAbilities)
                    .Include(b => b.ClassSkills)
                    .Include(b => b.AbilityUsedForCasting)
                    .Include(b => b.SpellKnown)
                    .Include(b => b.SpellPrDay)
                    .Include("SpellKnown.SpellsKnown")
                    .Include("SpellPrDay.NumberOfSpells")
                    .Include(b => b.SaveBonusRate);
                return bonus as IQueryable<T>;
            }
            if (typeof (T) == typeof (ClassLevel))
            {
                var cl = Context.ClassLevels
                    .Include(x => x.Class);
                return cl as IQueryable<T>;
            }
            if (typeof(T) == typeof(Character))
            {
                var bonus = Context.Characters
                    .Include(b => b.Abilities)
                    .Include(b => b.HitPoints)
                    .Include(b => b.HitPoints.Damage)
                    .Include(b => b.HitPoints.NonLethalDamage)
                    //.Include("Abilities.Ability")
                    .Include(b => b.Alligment)
                    .Include(b => b.Race)
                    .Include(b => b.Feats)
                    .Include(b => b.SkillRanks)
                    //.Include("SkillRanks.Skill")
                    .Include(b => b.Languages)
                    .Include(b => b.Inventory)
                    .Include(b => b.Inventory.BagItems)
                    .Include(b => b.CurrentClasses)
                    .Include(b => b.Round);
                    //.Include("CurrentClasses.Class")
                    //.Include("CurrentClasses.Class.ClassSkills")
                    //.Include("CurrentClasses.Class.ClassAbilities")
                    //.Include("CurrentClasses.Class.SaveBonusRate");
                return bonus as IQueryable<T>;
            }
            if (typeof(T) == typeof(Location))
            {
                var loc = Context.Locations.Include(s => s.LocationType).Include(s => s.Map).Include(s => s.NPCs);
                return loc as IQueryable<T>;
            }
            if (typeof(T) == typeof(ItemBase))
            {
                var itemBase = Context.ItemBase
                    .Include(b => b.Damage)
                    .Include(b => b.Damage.Amount)
                    .Include(b => b.Damage.Amount.Dice)
                    .Include(b => b.RequiresSlots)
                    .Include(b => b.HasSlots)
                    .Include(b => b.RequiresAbility)
                    //Bonus
                    .Include(b => b.EnchanmentsBonuses)
                    //.Include("EnchanmentsBonuses.BonusValue")
                    //.Include("EnchanmentsBonuses.BonusValue.AbilityModifyer")
                    //.Include("EnchanmentsBonuses.BonusValue.ClassProgression")
                    //.Include("EnchanmentsBonuses.BonusValue.ClassProgression.ClassProgression")
                    //.Include("EnchanmentsBonuses.BonusValue.Dice")

                    //Abilities
                    .Include("EnchanmentAbilities");
                    //.Include("EnchanmentAbilities.Limit")
                    //.Include("EnchanmentAbilities.Limit.Amount")
                    //.Include("EnchanmentAbilities.Limit.Amount.Ability")
                    //.Include("EnchanmentAbilities.Limit.Amount.ClassProgression")
                    //.Include("EnchanmentAbilities.Limit.Amount.ClassProgression.ClassProgression")
                    //.Include("EnchanmentAbilities.Limit.Duration")
                    //.Include("EnchanmentAbilities.Limit.Duration.DurationAbilityModifier")
                    //.Include("EnchanmentAbilities.Bonuses")
                    //.Include("EnchanmentAbilities.BonusFromCharges");
                return itemBase as IQueryable<T>;
            }
            if (typeof(T) == typeof(ItemMaterial))
            {
                var material = Context.ItemMaterial
                    .Include(b => b.MaterialBonues);
                return material as IQueryable<T>;
            }
            if (typeof(T) == typeof(NPC))
            {
                var material = Context.NPCs
                    .Include(b => b.Location);
                return material as IQueryable<T>;
            }
            if (typeof(T) == typeof(MaterialBonuses))
            {
                var material = Context.MaterialBonuses
                    .Include(b => b.Bonuses);
                return material as IQueryable<T>;
            }
            if (typeof(T) == typeof(Map))
            {
                var material = Context.Maps
                    .Include(b => b.Parts);
                return material as IQueryable<T>;
            }
            if (typeof(T) == typeof(OwnedItem))
            {
                var itemBase = Context.OwnedItems
                    .Include(b => b.Item)
                    .Include(b => b.OwnerMaterial)
                    .Include(b => b.EnchanmentAbilities)
                    .Include(b => b.EnchanmentsBonuses);
                return itemBase as IQueryable<T>;
            }
            if (typeof(T) == typeof(Race))
            {
                var race = Context.Races
                    .Include(b => b.AgeCategoryAtAge)
                    .Include(b => b.BonusLanguages)
                    .Include(b => b.FavoredClasses)
                    .Include(b => b.Languages)
                    .Include(b => b.RaceBonuses)
                    .Include(b => b.RacialAbilities);
                    //.Include("RaceBonuses.BonusValue.AbilityModifyer")
                    //.Include("RaceBonuses.BonusValue.ClassProgression")
                    //.Include("RaceBonuses.BonusValue.ClassProgression.ClassProgression")
                    //.Include("RaceBonuses.BonusValue.Dice")
                    //.Include("RacialAbilities.Limit")
                    //.Include("RacialAbilities.Limit.Amount")
                    //.Include("RacialAbilities.Limit.Amount.Ability")
                    //.Include("RacialAbilities.Limit.Amount.ClassProgression")
                    //.Include("RacialAbilities.Limit.Amount.ClassProgression.ClassProgression")
                    //.Include("RacialAbilities.Limit.Duration")
                    //.Include("RacialAbilities.Limit.Duration.DurationAbilityModifier")
                    //.Include("RacialAbilities.Bonuses")
                    //.Include("RacialAbilities.BonusFromCharges");
                return race as IQueryable<T>;
            }
            if (typeof (T) == typeof (Round))
            {
                var rnd = Context.Rounds
                    .Include(r => r.ActivatedAbilities)
                    .Include(r => r.ActivatedConditions)
                    .Include(r => r.UsedActions);

                return rnd as IQueryable<T>;
            }
            if (typeof(T) == typeof(SaveRate))
            {
                var saveRate = Context.SaveRate
                    .Include(b => b.Save);
                return saveRate as IQueryable<T>;
            }
            if (typeof(T) == typeof(Skill))
            {
                var skill = Context.Skills
                    .Include(b => b.SkillModifier)
                    .Include(b => b.SynergiFrom)
                    .Include(b => b.SynergiApplyTo);
                return skill as IQueryable<T>;
            }
            if (typeof(T) == typeof(SkillRank))
            {
                var skill = Context.SkillRanks
                    .Include(b => b.Skill);
                return skill as IQueryable<T>;
            }
            if (typeof(T) == typeof(SkillSynergi))
            {
                var skill = Context.SkillSynergies;
                return skill as IQueryable<T>;
            }
            if (typeof(T) == typeof(Spell))
            {
                var spell = Context.Spells
                    .Include(b => b.CasterRequirements)
                    .Include("CasterRequirements.CasterClass")
                    .Include(b => b.ComponentRequirements)
                    .Include(b => b.Damage)
                    .Include(b => b.Damage.Damage)
                    .Include(b => b.Damage.Damage.Amount)
                    .Include(b => b.Damage.Damage.Amount.Dice)
                    .Include(b => b.Damage.ByCasterLevel)
                    .Include(b => b.Descriptor)
                    .Include(b => b.School)
                    .Include(b => b.SpellAbility)
                    .Include(b => b.SpellSaveType);
                return spell as IQueryable<T>;
            }
            if (typeof(T) == typeof(SpellSlot))
            {
                var spell = Context.SpellSlots
                    .Include(b => b.Spell)
                    .Include(b => b.SpellChosenForClass)
                    .Include(b => b.Spell.CasterRequirements)
                    .Include("Spell.CasterRequirements.CasterClass")
                    .Include(b => b.Spell.ComponentRequirements)
                    .Include(b => b.Spell.Damage)
                    .Include(b => b.Spell.Damage.Damage)
                    .Include(b => b.Spell.Damage.Damage.Amount)
                    .Include(b => b.Spell.Damage.Damage.Amount.Dice)
                    .Include(b => b.Spell.Damage.ByCasterLevel)
                    .Include(b => b.Spell.Descriptor)
                    .Include(b => b.Spell.School)
                    .Include(b => b.Spell.SpellAbility)
                    .Include(b => b.Spell.SpellSaveType);
                return spell as IQueryable<T>;
            }
            if (typeof(T) == typeof(SpecialAbility))
            {
                var specialAbi = Context.SpecialAbilities
                    //.Include(b => b.RequiresSpecialAbilityActive)
                    .Include(b => b.Limit)
                    .Include(b => b.Limit.Amount)
                    .Include(b => b.Limit.Amount.TradeWith)
                    .Include(b => b.Limit.Amount.Ability)
                    .Include(b => b.Limit.Amount.ClassProgression)
                    //.Include("Limit.Amount.ClassProgression.ClassProgression")
                    .Include(b => b.Limit.Duration)
                    .Include(b => b.Limit.Duration.DurationAbilityModifier)
                    .Include(b => b.Bonuses)
                    .Include(b => b.BonusFromCharges)
                    //.Include("BonusFromCharges.Bonus")
                    //.Include("BonusFromCharges.Bonus.Dice")
                    .Include(b => b.ApplyConditionOnActivate)
                    .Include(b => b.ApplyConditionOnDeactivate);
                
                return specialAbi as IQueryable<T>;
            }
            if (typeof(T) == typeof(SpellRequiretLevel))
            {
                var spl = Context.SpellRequiretLevel.Include(s => s.CasterClass).Include(s => s.Spell);
                return spl as IQueryable<T>;
            }
            if (typeof(T) == typeof(TimeLimitUnitParsed))
            {
                var tlup = Context.TimeLimitUnitParsed;
                return tlup as IQueryable<T>;
            }
            if (typeof(T) == typeof(UsableAmountClassProgression))
            {
                var tlup = Context.UsableAmountClassProgressions.
                    Include(x => x.ClassProgression);
                return tlup as IQueryable<T>;
            }
            

            
            //Loading in default way
            var item = Context.Set<T>();
            return item;
        }
        public List<T> GetAll<T>() where T : GameId
        {
            return GetQueryable<T>().ToList();
        }

        public void CreateOrUpdate<T>(T item) where T : GameId
        {
            try
            {
                Context.Set<T>().AddOrUpdate(item);
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public List<ElementId> GetItemsFromBonusApplyToType(BonusApplyToType type)
        {
            switch (type)
            {
                case BonusApplyToType.BaseAttack:
                    return new List<ElementId>(new[]{new ElementId
                    {
                        ID = Configuration.BabId,
                        Name = "Base Attack",
                    }});
                case BonusApplyToType.Ability:
                    return GetAll<Ability>().Cast<ElementId>().ToList();
                case BonusApplyToType.Ac:
                    return new List<ElementId>(new []{new ElementId
                    {
                        ID = Configuration.AcId,
                        Name = "AC",
                    }});
                case BonusApplyToType.Class:
                    return GetAll<ClassBase>().Cast<ElementId>().ToList();
                case BonusApplyToType.DamageRecuction:
                    return new List<ElementId>(new[] { new ElementId { ID = Configuration.DamageRecuctionId, Name = "Damage Recuction" }, });
                case BonusApplyToType.Grapple:
                    return new List<ElementId>(new[] { new ElementId { ID = Configuration.GrappleId, Name = "Grapple" }, });
                case BonusApplyToType.HitPoints:
                    return new List<ElementId>(new[]
                    {
                        new ElementId { ID = Configuration.HitPointsId, Name = "Hit Points" },
                        new ElementId { ID = Configuration.HitPointsDmg, Name = "Damage" },
                        new ElementId { ID = Configuration.HitPointsDmgNl, Name = "Non lethal damage" },
                        new ElementId { ID = Configuration.HitPointsHeal, Name = "Heal" },
                    });
                case BonusApplyToType.Initiativ:
                    return new List<ElementId>(new[] { new ElementId { ID = Configuration.InitiativId, Name = "Initiativ" }, });
                case BonusApplyToType.Item:
                    return GetAll<ItemBase>().Cast<ElementId>().ToList();
                case BonusApplyToType.Race:
                    return GetAll<Race>().Cast<ElementId>().ToList();
                case BonusApplyToType.Round:
                    return new List<ElementId>(new[]{new ElementId
                    {
                        ID = Configuration.MoveAction,
                        Name = "Move Action",
                    },
                    new ElementId
                    {
                        ID = Configuration.StandardAction,
                        Name = "Standard Action",
                    },
                    new ElementId
                    {
                        ID = Configuration.FullRoundAction,
                        Name = "Full Round Action",
                    },});
                case BonusApplyToType.Save:
                    return GetAll<SaveType>().Cast<ElementId>().ToList();
                case BonusApplyToType.Skill:
                    return GetAll<Skill>().Cast<ElementId>().ToList();
                case BonusApplyToType.SpecialAbility:
                    return GetAll<SpecialAbility>().Cast<ElementId>().ToList();
                case BonusApplyToType.Spell:
                    return GetAll<Spell>().Cast<ElementId>().ToList();
                case BonusApplyToType.SpellSchool:
                    return GetAll<SpellSchool>().Cast<ElementId>().ToList();
                case BonusApplyToType.SpellResistance:
                    return new List<ElementId>(new[] { new ElementId { ID = Configuration.SpellResistanceId, Name = "Spell Resistance" }, });
                default:
                    throw new NotImplementedException("BonusApplyToType not supported");
            }
        }

        public List<ElementId> GetSubItemsFromBonusApplyToType(BonusApplyToType type)
        {
            switch (type)
            {
                case BonusApplyToType.BaseAttack:
                    return new List<ElementId>();
                case BonusApplyToType.Ability:
                    return new List<ElementId>();
                case BonusApplyToType.Ac:
                    return new List<ElementId>(new[] { new ElementId { ID = Configuration.AcFlatFootedId, Name = "Flat footed only" }, new ElementId { ID = Configuration.AcTouchId, Name = "Touch only" }, });
                case BonusApplyToType.Class:
                    return new List<ElementId>(new[]
                    {
                        new ElementId { ID = Configuration.SpellPrDayId, Name = "Spell Pr Day" },
                        new ElementId { ID = Configuration.ExistingCasterLevelDivineId, Name = "Existing caster level (divine)" },
                        new ElementId { ID = Configuration.ExistingCasterLevelArcaneId, Name = "Existing caster level (arcane)" },
                    });
                case BonusApplyToType.DamageRecuction:
                    return new List<ElementId>();
                case BonusApplyToType.Grapple:
                    return new List<ElementId>();
                case BonusApplyToType.HitPoints:
                    return new List<ElementId>();
                case BonusApplyToType.Initiativ:
                    return new List<ElementId>();
                case BonusApplyToType.Item:
                    return new List<ElementId>(new[] { new ElementId { ID = Configuration.WeightReductionInProcentage, Name = "Weight Reduction In Procentage" }, 
                        new ElementId { ID = Configuration.CostIncreaseInGold, Name = "Cost Increase In Gold" }, 
                        new ElementId { ID = Configuration.CostIncreaseInGoldPrLbWeight, Name = "Cost Increase In Gold pr lb weight" }, 
                        new ElementId { ID = Configuration.ArmorCheckPeneltyReduction, Name = "Armor Check Penelty Reduction" }, 
                        new ElementId { ID = Configuration.ExtraAC, Name = "Extra AC" }, 
                        new ElementId { ID = Configuration.MaxDexBonus, Name = "Max Dex Bonus" }, 
                        new ElementId { ID = Configuration.MaxSpeed, Name = "Max Speed" }, 
                        new ElementId { ID = Configuration.ArcaneSpellFailure, Name = "Arcane Spell Failure" }, 
                        new ElementId { ID = Configuration.Damage, Name = "Damage (All items)" }, 
                        new ElementId { ID = Configuration.DamageWeapon, Name = "Damage (Weapons only)" }, 
                        new ElementId { ID = Configuration.DamageShield, Name = "Damage (Shield only)" },
                        new ElementId { ID = Configuration.DamageArmor, Name = "Damage (Armor only)" },
                        new ElementId { ID = Configuration.DamageMisc, Name = "Damage (Misc only)" },
                        new ElementId { ID = Configuration.DamageUnarmed, Name = "Damage (Unarmed only)" },
                        new ElementId { ID = Configuration.AttackBonues, Name = "Attack bonues" }, 
                        new ElementId { ID = Configuration.CritRange, Name = "Crit Range (multiplier)" }, 
                        new ElementId { ID = Configuration.RangeIncrement, Name = "Range Increment (ft)" }, }).OrderBy(x => x.Name).ToList();
                case BonusApplyToType.Race:
                    return new List<ElementId>(new[]
                    {
                        new ElementId { ID = Configuration.SizeId, Name = "Size Category" },
                        new ElementId { ID = Configuration.SpeedId, Name = "Speed" },
                    }).OrderBy(x => x.Name).ToList();
                case BonusApplyToType.Round:
                    return new List<ElementId>();
                case BonusApplyToType.Save:
                    return new List<ElementId>();
                case BonusApplyToType.Skill:
                    var list = new List<ElementId>(new[]
                    {
                        new ElementId { ID = Configuration.SkillPointPrLevel, Name = "Skill point pr level" },
                    }).OrderBy(x => x.Name).ToList();
                    list.AddRange(GetAll<Ability>().Cast<ElementId>().Select(x => new ElementId
                    {
                        ID = x.ID,
                        Description = x.Description,
                        Name = "Skill based on " + x.Name
                    }).ToList());
                    return list;
                case BonusApplyToType.SpecialAbility:
                    return new List<ElementId>(new[]
                    {
                        new ElementId { ID = Configuration.Feat, Name = "Feat" },
                    }).OrderBy(x => x.Name).ToList();
                case BonusApplyToType.Spell:
                    return new List<ElementId>();
                case BonusApplyToType.SpellSchool:
                    return new List<ElementId>();
                case BonusApplyToType.SpellResistance:
                    return new List<ElementId>();
                default:
                    throw new NotImplementedException("BonusApplyToType not supported");
            }
        }

        public Character LoadCharacter(Guid id)
        {
            var watch = new Stopwatch();
            watch.Start();

            var baseCharacter = Get<Character>(id);
            baseCharacter.Conditions = GetAll<Condition>();

            baseCharacter.Load();

            watch.Stop();
            var ellapsed = watch.ElapsedMilliseconds;
            if (ellapsed != 0)
            {
                
            }

            return baseCharacter;
        }

        public List<OwnedItem> GetOwnedItems(Guid ownedId, Character character)
        {
            var item = character.Inventory.BagItems.First(x => x.ID == ownedId);
            if (item == null)
            {
                return new List<OwnedItem>();
            }
            return character.Inventory.BagItems.Where(GetOwnedItem(item)).ToList();

        }

        public List<History> GetHistory(Type type, Guid id)
        {
            return Context.History.Where(x => x.Description.Contains(type.Name + ":" + id.ToString())).OrderByDescending(y => y.Created).ToList();
        }

        public static Func<OwnedItem, bool> GetOwnedItem(OwnedItem item)
        {
            return x => x.Item.ID == item.Item.ID &&
                        x.OwnerEnchamtmentBonues == item.OwnerEnchamtmentBonues &&
                        x.OwnerMasterWorked == item.OwnerMasterWorked &&
                        ((x.OwnerMaterial == null && item.OwnerMaterial == null) ||
                         (x.OwnerMaterial != null && item.OwnerMaterial != null &&
                          x.OwnerMaterial.ID == item.OwnerMaterial.ID));
        }

    }
}