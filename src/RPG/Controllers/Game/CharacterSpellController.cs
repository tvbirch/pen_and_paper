using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.ControllerDto.Game;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Spells;

namespace RPG.Controllers.Game
{
    public class CharacterSpellController : ControllerBase
    {
        //
        // GET: /CharacterSpell/

        public ActionResult Index(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Game/CharacterSelector");
            }
            return View("Game/CharacterSpell", new CharacterSpellDto(Context, id.Value));
        }
        public ActionResult SearchSpells(string q, string level, string casterlevel, string classid, string charid)
        {
            var trimedQuery = q.Trim();
            var levelAsInt = level.ToInt();
            var casterlevelAsInt = casterlevel.ToInt();
            var classGuid = classid.ToGuid();
            var charGuid = charid.ToGuid();

            if (classGuid == null || classGuid == Guid.Empty)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            var classObject = Context.Get<ClassBase>(classGuid.GetValueOrDefault());
            
            var spellsKnownAtCurrentLevel = classObject.SpellKnownDictonary[casterlevelAsInt.GetValueOrDefault()][levelAsInt.GetValueOrDefault()];
            var spells = new List<Spell>();
            if (spellsKnownAtCurrentLevel == -1)
            {
                //Load all spells, since caster can cast them all
                spells =
                    Context.GetQueryable<Spell>().Where(
                            x => x.Name.Contains(q) && x.CasterRequirements.Any(y => y.CasterClass.ID == classGuid && y.CasterLevel == levelAsInt))
                        .ToList();
            }
            //Load only from spellbook, even if caster knows all spells, they might have extra in spell book, from domains etc
            
            var result = spells.Select(s => new SearchSpell
            {
                id = s.ID.ToString(),
                text = s.Name,
                Description = s.Description,
                CastingTime = s.CastingTime.ToString(),
                Components = s.GetComponentsString(),
                Range = s.GetRangeString(),
                Save = s.GetSave(),
                SpellResistance = s.GetSpellResistanceString(),
                Target = s.GetTarget()
            }).ToList();
            return Json(new { result }, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult FillSlot(Guid id, int slotLevel, Guid classId, Guid charId)
        {
            if (id == Guid.Empty)
            {
                return Json("No spell selected", JsonRequestBehavior.AllowGet);
            }
            var classBase = Context.Get<ClassBase>(classId);
            if (classBase == null)
            {
                return Json("Class does not exist", JsonRequestBehavior.AllowGet);
            }
            var character = Context.LoadCharacter(charId);
            if (character.SpellSlots == null)
            {
                character.SpellSlots = new List<SpellSlot>();
            }

            var spell = Context.Get<Spell>(id);
            if (spell == null)
            {
                return Json("Spell does not exist", JsonRequestBehavior.AllowGet);
            }

            var spellSlots = character.GetSpellsSlots(slotLevel,classId);
            if (spellSlots.All(x => x != null))
            {
                return Json("No empty slots", JsonRequestBehavior.AllowGet);
            }
            character.SpellSlots.Add(new SpellSlot
            {
                Spell = spell,
                SpellChosenForClass = classBase,
                Used = false
            });
            Context.Context.SaveChanges();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SpendSlot(Guid id, int slotLevel, Guid classId, Guid charId)
        {
            if (id == Guid.Empty)
            {
                return Json("Spellslot guid must not be guid.empty", JsonRequestBehavior.AllowGet);
            }
            var classBase = Context.Get<ClassBase>(classId);
            if (classBase == null)
            {
                return Json("Class does not exist", JsonRequestBehavior.AllowGet);
            }
            var character = Context.LoadCharacter(charId);
            if (character.SpellSlots == null)
            {
                character.SpellSlots = new List<SpellSlot>();
            }

            var spellSlot = character.SpellSlots.FirstOrDefault(x => x.ID == id);
            if (spellSlot == null)
            {
                return Json("SpellSlot does not exist", JsonRequestBehavior.AllowGet);
            }
            spellSlot.Used = true;
            Context.Context.SaveChanges();
            
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveSpellFromSlot(Guid id, int slotLevel, Guid classId, Guid charId)
        {
            if (id == Guid.Empty)
            {
                return Json("Spellslot guid must not be guid.empty", JsonRequestBehavior.AllowGet);
            }
            var classBase = Context.Get<ClassBase>(classId);
            if (classBase == null)
            {
                return Json("Class does not exist", JsonRequestBehavior.AllowGet);
            }
            var character = Context.LoadCharacter(charId);
            if (character.SpellSlots == null)
            {
                character.SpellSlots = new List<SpellSlot>();
            }

            var spellSlot = character.SpellSlots.FirstOrDefault(x => x.ID == id);
            if (spellSlot == null)
            {
                return Json("SpellSlot does not exist", JsonRequestBehavior.AllowGet);
            }
            Context.Delete<SpellSlot>(id);
            Context.Context.SaveChanges();

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
