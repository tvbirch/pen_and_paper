using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal.BaseTypes.Skills;

namespace RPG.Controllers.Creator
{
    public class SkillSynergiController : ControllerBase
    {
        //
        // GET: /SkillSynergi/

        public ActionResult Index(Guid? id, Guid? skillSynergiId)
        {
            if (!id.HasValue || !skillSynergiId.HasValue)
            {
                return RedirectToAction("Index", "Skill", new { id = id });
            }
            return View("Creator/SkillSynergi", "CreatorLayoutPage", GetSkillSynergi(id, skillSynergiId));
        }

        public ActionResult Delete(Guid? id, Guid? skillSynergiId)
        {
            if (skillSynergiId.HasValue)
            {
                Context.Delete<SkillSynergi>(skillSynergiId.Value);
            }
            return View("Creator/SkillSynergi", "CreatorLayoutPage", GetSkillSynergi(id, Guid.Empty));
        }

        [HttpPost]
        public ActionResult Save(SkillSynergiDataDto data)
        {
            //Setting data from result
            //data.SelectedItem.SynergiApplyTo = context.Get<Skill>(data.SelectedSkill.ID);
            //data.SelectedItem.SynergiApplyToId = data.SelectedItem.SynergiApplyTo.ID;
            data.SelectedItem.SynergiApplyToId = data.SelectedSkill.ID;
            
            //data.SelectedItem.SynergiFrom = data.SelectedSkill;
            data.SelectedItem.SynergiFromId = data.SelectedItem.SynergiFromId;

            Context.CreateOrUpdate(data.SelectedItem);
            return RedirectToAction("Index", "SkillSynergi", new { id = data.SelectedSkill.ID, skillSynergiId = data.SelectedItem.ID });
            //return View("Creator/SkillSynergi", "CreatorLayoutPage", GetSkillSynergi(data.SelectedSkill.ID, data.SelectedItem.ID));
        }

        private SkillSynergiDataDto GetSkillSynergi(Guid? skillId, Guid? skillSynergiId)
        {
            return new SkillSynergiDataDto(Context, skillId, skillSynergiId);
        }
    }
}
