using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Spells;

namespace RPG.Controllers.Creator
{
    public class SpellCasterRequirementsController : ControllerBase
    {
        public ActionResult Index(Guid? id, Guid? crId)
        {
            if (!id.HasValue || !crId.HasValue)
            {
                return RedirectToAction("Index", "Spell", new { id = id });
            }
            return View("Creator/SpellCasterRequirements", "CreatorLayoutPage", GetSpellCasterRequirements(id, crId));
        }

        public ActionResult Delete(Guid? id, Guid? crId)
        {
            if (crId.HasValue)
            {
                Context.Delete<SpellRequiretLevel>(crId.Value);
            }
            return View("Creator/SpellCasterRequirements", "CreatorLayoutPage", GetSpellCasterRequirements(id, Guid.Empty));
        }

        [HttpPost]
        public ActionResult Save(SpellCasterRequirementsDataDto data)
        {
            SpellRequiretLevel spellRequiretLevel;
            if (data.SelectedRequirement.ID == Guid.Empty)
            {
                spellRequiretLevel = data.SelectedRequirement;
            }
            else
            {
                spellRequiretLevel = Context.Get<SpellRequiretLevel>(data.SelectedRequirement.ID);
            }
            spellRequiretLevel.CasterLevel = data.SelectedRequirement.CasterLevel;
            if (data.ClassGuid != null)
            {
                spellRequiretLevel.CasterClass = Context.Get<ClassBase>(data.ClassGuid.Value);
            }

            spellRequiretLevel.Spell = Context.Get<Spell>(data.SelectedItem.ID);

            Context.CreateOrUpdate(spellRequiretLevel);
            return RedirectToAction("Index", "SpellCasterRequirements", new { id = data.SelectedItem.ID, crId = spellRequiretLevel.ID });
            //return View("Creator/SpellCasterRequirements", "CreatorLayoutPage", GetSpellCasterRequirements(data.SelectedItem.ID, spellRequiretLevel.ID));
        }

        private SpellCasterRequirementsDataDto GetSpellCasterRequirements(Guid? id, Guid? crId)
        {
            return new SpellCasterRequirementsDataDto(Context, id, crId);
        }
    }
}