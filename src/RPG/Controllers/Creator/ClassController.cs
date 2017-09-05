using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Alligments;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.Spells;

namespace RPG.Controllers.Creator
{
    public class ClassController : Controller
    {
        //
        // GET: /Class/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/Class", "CreatorLayoutPage", GetClass(id));
        }

        [HttpPost]
        public ActionResult Save(ClassDataDto data)
        {
            var manager = new ContextManager();
            ClassBase classToUpdate;

            if (data.SelectedItem.ID == Guid.Empty)
            {
                classToUpdate = data.SelectedItem;
            }
            else
            {
                classToUpdate = manager.Get<ClassBase>(data.SelectedItem.ID);
            }
            if (data.AllowedAllignments != null)
            {
                classToUpdate.AllowedAlligments = manager.GetList<Alligment>(data.AllowedAllignments);
            }
            if (data.ClassSkills != null)
            {
                classToUpdate.ClassSkills = manager.GetList<Skill>(data.ClassSkills);
            }
            if (classToUpdate.SpellKnown == null)
            {
                classToUpdate.SpellKnown = new List<SpellKnown>();
            }
            if (classToUpdate.SpellPrDay == null)
            {
                classToUpdate.SpellPrDay = new List<SpellPrDay>();
            }
            if (data.AbilityUsedForCasting != null && data.AbilityUsedForCasting != Guid.Empty)
            {
                classToUpdate.AbilityUsedForCasting = manager.Get<Ability>(data.AbilityUsedForCasting.Value);
            }
            if (data.SpellsPrDay != null)
            {
                for (int i = 0; i < data.SpellsPrDay.Length; i++)
                {
                    var currentLevel = i + 1;
                    var existing = classToUpdate.SpellPrDay.FirstOrDefault(x => x.Level == currentLevel);
                    
                    var currentListEmpty = !data.SpellsPrDay[i].Any(x => x != 0);
                    if (existing != null && currentListEmpty)
                    {
                        manager.Delete<SpellPrDay>(existing.ID);
                        continue;
                    } 
                    if (existing == null && !currentListEmpty)
                    {
                        existing = new SpellPrDay();
                        existing.Level = i+1;
                        existing.NumberOfSpells = new List<SpellsPrLevel>();
                        classToUpdate.SpellPrDay.Add(existing);
                    }
                    if (currentListEmpty)
                    {
                        continue;
                    }
                    for (int j = 0; j < data.SpellsPrDay[i].Length; j++)
                    {
                        var existingSub = existing.NumberOfSpells.FirstOrDefault(x => x.SpellLevel == j);
                        if (existingSub != null && data.SpellsPrDay[i][j] == 0)
                        {
                            manager.Delete<SpellsPrLevel>(existingSub.ID);
                            continue;
                        }
                        if (data.SpellsPrDay[i][j] == 0)
                        {
                            continue;
                        }
                        if (existingSub == null)
                        {
                            existingSub = new SpellsPrLevel();
                            existingSub.SpellLevel = j;
                            existing.NumberOfSpells.Add(existingSub);
                        }
                        existingSub.SpellCastable = data.SpellsPrDay[i][j];
                    }
                }
            }
            if (data.KnownSpells != null)
            {
                for (int i = 0; i < data.KnownSpells.Length; i++)
                {
                    var currentLevel = i + 1;
                    var existing = classToUpdate.SpellKnown.FirstOrDefault(x => x.Level == currentLevel);

                    var currentListEmpty = !data.KnownSpells[i].Any(x => x != 0);
                    if (existing != null && currentListEmpty)
                    {
                        manager.Delete<SpellKnown>(existing.ID);
                        continue;
                    }
                    if (existing == null && !currentListEmpty)
                    {
                        existing = new SpellKnown();
                        existing.Level = i + 1;
                        existing.SpellsKnown = new List<SpellsKnownPrLevel>();
                        classToUpdate.SpellKnown.Add(existing);
                    }
                    if (currentListEmpty)
                    {
                        continue;
                    }
                    for (int j = 0; j < data.KnownSpells[i].Length; j++)
                    {
                        var existingSub = existing.SpellsKnown.FirstOrDefault(x => x.SpellLevel == j);
                        if (existingSub != null && data.KnownSpells[i][j] == 0)
                        {
                            manager.Delete<SpellsKnownPrLevel>(existingSub.ID);
                            continue;
                        }
                        if (data.SpellsPrDay[i][j] == 0)
                        {
                            continue;
                        }
                        if (existingSub == null)
                        {
                            existingSub = new SpellsKnownPrLevel();
                            existingSub.SpellLevel = j;
                            existing.SpellsKnown.Add(existingSub);
                        }
                        existingSub.NumberOfSpells = data.KnownSpells[i][j];
                    }
                }
            }
            classToUpdate.Name = data.SelectedItem.Name;
            classToUpdate.Description = data.SelectedItem.Description;
            classToUpdate.BaseAttackBaseSaveBonus = data.SelectedItem.BaseAttackBaseSaveBonus;
            classToUpdate.BaseSkillPointsAtEachLevel = data.SelectedItem.BaseSkillPointsAtEachLevel;
            classToUpdate.HitDie = data.SelectedItem.HitDie;
            classToUpdate.SaveBonusRate = data.SelectedItem.SaveBonusRate;
            classToUpdate.ArcaneCaster = data.SelectedItem.ArcaneCaster;
            classToUpdate.DivineCaster = data.SelectedItem.DivineCaster;


            manager.CreateOrUpdate(classToUpdate);
            return RedirectToAction("Index", "Class", new { id = classToUpdate.ID });
            //return View("Creator/Class", "CreatorLayoutPage", GetClass(classToUpdate.ID));
        }

        private static ClassDataDto GetClass(Guid? id)
        {
            return new ClassDataDto(new ContextManager(), id);
        }
    }
}
