using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.CharacterModal;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Alligments;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Currency;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Controllers.Creator
{
    public class CharacterCreatorController : ControllerBase
    {
        public ActionResult Index(Guid? id)
        {
            return View("Creator/CharacterCreator", "CreatorLayoutPage", GetCharactor(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                new ContextManager().Delete<Character>(id.Value);
            }
            return View("Creator/CharacterCreator", "CreatorLayoutPage", GetCharactor(null));
        }

        private CharactorCreatorDataDto GetCharactor(Guid? id)
        {
            return new CharactorCreatorDataDto(Context, id);
        }

        [HttpPost]
        public ActionResult Save(CharactorCreatorDataDto data)
        {
            Character charToUpdate;
            if (data.SelectedItem.ID == Guid.Empty)
            {
                charToUpdate = new Character();
                charToUpdate.Abilities = new List<AbilityScore>();
                charToUpdate.CurrentClasses = new List<ClassLevel>();
                charToUpdate.Feats = new List<SpecialAbility>();
                charToUpdate.HitPoints = new HitPoints();
                charToUpdate.Languages = new List<Language>();
                charToUpdate.PhysicalAppearance = new PhysicalAppearance();
                charToUpdate.PhysicalAppearance.Age = new Age();
                charToUpdate.PhysicalAppearance.Height = new ImperialDistance();
                charToUpdate.PhysicalAppearance.Weight = new ImperialWeight();
                charToUpdate.SkillRanks = new List<SkillRank>();
                charToUpdate.SpellSlots = new List<SpellSlot>();

                charToUpdate.Inventory = new Inventory();
                charToUpdate.Inventory.Wealth = new Money();
            }
            else
            {
                charToUpdate = Context.Get<Character>(data.SelectedItem.ID);
            }

            charToUpdate.Name = data.SelectedItem.Name;
            charToUpdate.Description = data.SelectedItem.Description;
            charToUpdate.Xp = data.SelectedItem.Xp;

            if (charToUpdate.HitPoints == null)
            {
                charToUpdate.HitPoints = new HitPoints();
            }
            charToUpdate.HitPoints.BaseHp = data.SelectedItem.HitPoints.BaseHp;

            charToUpdate.PhysicalAppearance.Age.CurrentAge = data.SelectedItem.PhysicalAppearance.Age.CurrentAge;
            charToUpdate.PhysicalAppearance.Eyes = data.SelectedItem.PhysicalAppearance.Eyes;
            charToUpdate.PhysicalAppearance.Hair = data.SelectedItem.PhysicalAppearance.Hair;
            charToUpdate.PhysicalAppearance.Gender = data.SelectedItem.PhysicalAppearance.Gender;
            charToUpdate.PhysicalAppearance.Height._inches = data.SelectedItem.PhysicalAppearance.Height._inches;
            charToUpdate.PhysicalAppearance.Skin = data.SelectedItem.PhysicalAppearance.Skin;
            charToUpdate.PhysicalAppearance.Weight.Lb = data.SelectedItem.PhysicalAppearance.Weight.Lb;

            charToUpdate.Languages = Context.GetList<Language>(data.LearnedLanguages);

            if (data.SelectedItem.Alligment != null && data.SelectedItem.Alligment.ID != Guid.Empty)
            {
                charToUpdate.Alligment = Context.Get<Alligment>(data.SelectedItem.Alligment.ID);
            }
            if (data.Race.HasValue)
            {
                charToUpdate.Race = Context.Get<Race>(data.Race.Value);
            }

            if (data.AbilityScores != null)
            {
                if (charToUpdate.Abilities == null)
                {
                    charToUpdate.Abilities = new List<AbilityScore>();
                }
                foreach (var abi in data.AbilityScores)
                {
                    //FIX SÅ ABIS IKKE TILFØJES IGEN OG IGEN.
                    var cAbi = charToUpdate.Abilities.FirstOrDefault(x => x.Ability != null && x.Ability.ID == abi.Ability.ID);
                    if (cAbi == null)
                    {
                        cAbi = new AbilityScore
                        {
                            Ability = Context.Get<Ability>(abi.Ability.ID),
                        };
                        charToUpdate.Abilities.Add(cAbi);
                    }
                    cAbi.BaseValue = abi.BaseValue;
                }
            }

            if (data.CurrentClassToAdd.HasValue)
            {
                if (charToUpdate.CurrentClasses == null)
                {
                    charToUpdate.CurrentClasses = new List<ClassLevel>();
                }
                charToUpdate.CurrentClasses.Add(new ClassLevel
                {
                    Level = 1,
                    Class = Context.Get<ClassBase>(data.CurrentClassToAdd.Value)
                });
            }
            if (data.SelectedItem.CurrentClasses != null)
            {
                foreach (var currentClass in data.SelectedItem.CurrentClasses)
                {
                    charToUpdate.CurrentClasses.First(x => x.Class.ID == currentClass.Class.ID).Level =
                        currentClass.Level;
                }
            }

            if (data.LearnedFeats != null)
            {
                charToUpdate.Feats = Context.GetList<SpecialAbility>(data.LearnedFeats);
            }

            if (data.SelectedItem.SkillRanks != null)
            {
                //TODO add dem.
                if (charToUpdate.SkillRanks == null)
                {
                    charToUpdate.SkillRanks = new List<SkillRank>();
                }
                List<SkillRank> toAdd = new List<SkillRank>();
                foreach (var newSkillRank in data.SelectedItem.SkillRanks)
                {
                    var found = false;
                    foreach (var skillRank in charToUpdate.SkillRanks)
                    {
                        if (skillRank.Skill.ID == newSkillRank.Skill.ID)
                        {
                            found = true;
                            skillRank.Ranks = newSkillRank.Ranks;
                            break;
                        }
                    }
                    if (!found)
                    {
                        toAdd.Add(new SkillRank
                        {
                            Ranks = newSkillRank.Ranks,
                            Skill = Context.Get<Skill>(newSkillRank.Skill.ID),
                        });
                    }
                }
                charToUpdate.SkillRanks.AddRange(toAdd);
            }


            Context.CreateOrUpdate(charToUpdate);
            return RedirectToAction("Index", "CharacterCreator", new { id = charToUpdate.ID });
            //return View("Creator/CharacterCreator", "CreatorLayoutPage", GetCharactor(charToUpdate.ID));
        }


        public ActionResult UpdateCharacterData(int? age, Guid? raceId, Dictionary<string, int> abilityKeyVal, Dictionary<string, string> abilityMap)
        {
            var currentCategory = "N/A";
            var ageCategories = "N/A";
            var abilityModifiersWithBonusStat = new Dictionary<string, int>();
            var abilityModifiersWithBonus = new Dictionary<string, int>();
            var abilityRaceModifiers = new Dictionary<string, int>();
            var abilityAgeModifiers = new Dictionary<string, int>();

            Race race = null;
            var abilties = new List<AbilityScore>();
            var raceAbiMods = new List<Bonus>();
            var ageAbiMods = new List<Bonus>();

            foreach (var i in abilityKeyVal)
            {
                abilties.Add(new AbilityScore
                {
                    BaseValue = i.Value,
                    Ability = Context.Get<Ability>(Guid.Parse(i.Key)),
                });
            }

            if (raceId.HasValue)
            {
                race = Context.Get<Race>(raceId.Value);
                if (race != null)
                {
                    if (age.HasValue)
                    {
                        currentCategory = race.GetAgeCategoryFromAge(age.Value).ToString();
                    }
                    ageCategories = string.Join(", ", race.AgeCategoryAtAge.Select(x => x.Category.ToString() + " at " + x.Age));
                    //Getting race ability mods
                    raceAbiMods = race.RaceBonuses.Where(b => b.ApplyTo.ApplyToType == BonusApplyToType.Ability).ToList();
                }
            }
            //Calculating race modifiers
            foreach (var map in abilityMap)
            {
                var bonusIfAny = raceAbiMods.Where(a => a.ApplyTo.ApplyToGuid == Guid.Parse(map.Key)).Select(x => x.GetBonus(null)).FirstOrDefault();
                if (bonusIfAny != null)
                {
                    abilityRaceModifiers.Add(map.Value, bonusIfAny.GetFixedAmount());
                }
            }

            //Calculating age modifiers
            var ageObj = new Age
            {
                CurrentAge = age.GetValueOrDefault()
            };
            foreach (var map in abilityMap)
            {
                var bonusIfAny = 0;
                if (age.HasValue && race != null)
                {
                    bonusIfAny =
                        ageObj.GetAgeBonuses(race, abilties)
                            .Where(b => b.ApplyTo.ApplyToGuid == Guid.Parse(map.Key))
                            .Select(x => x.BonusValue.FixedValue)
                            .FirstOrDefault()
                            .GetValueOrDefault();
                }
                abilityAgeModifiers.Add(map.Value, bonusIfAny);
            }
            ageAbiMods.AddRange(ageObj.GetAgeBonuses(race, abilties));
            
            //Calculate ability modifier 
            var allBonuses = new List<BonusRef>(raceAbiMods.Select(x => new BonusRef(new ElementId{Name = "Race"}, x)));
            allBonuses.AddRange(ageAbiMods.Select(x => new BonusRef(new ElementId { Name = "Age" }, x)));
            foreach (var i in abilityMap)
            {
                var currentAbi = abilties.FirstOrDefault(x => x.Ability.ID == Guid.Parse(i.Key));
                if (currentAbi != null)
                {
                    abilityModifiersWithBonusStat.Add(i.Value, currentAbi.GetCurrent(new GetBonusDto
                    {
                        ActiveBonus = new List<BonusRef>(),
                        PassiveBonus = allBonuses,
                    }).GetValueAsInt());
                    abilityModifiersWithBonus.Add(i.Value, currentAbi.GetCurrentModifier(new GetBonusDto
                    {
                        ActiveBonus = new List<BonusRef>(),
                        PassiveBonus = allBonuses,
                    }));
                }
            }


            //Creating json result TODO, og add til html
            
            return Json(new { 
                currentCategory = currentCategory,
                ageCategories = ageCategories,
                abilityModifiersWithBonusStat = abilityModifiersWithBonusStat.ToList(),
                abilityModifiersWithBonus = abilityModifiersWithBonus.ToList(),
                abilityRaceModifiers = abilityRaceModifiers.ToList(),
                abilityAgeModifiers = abilityAgeModifiers.ToList()
                 }, JsonRequestBehavior.AllowGet);
        }

    }
}
