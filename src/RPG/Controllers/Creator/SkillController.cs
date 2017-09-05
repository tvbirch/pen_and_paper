using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Skills;

namespace RPG.Controllers.Creator
{
    public class SkillController : Controller
    {
        //
        // GET: /Skill/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/Skill", "CreatorLayoutPage", GetSkills(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                new ContextManager().Delete<Skill>(id.Value);
            }
            return View("Creator/Skill", "CreatorLayoutPage", GetSkills(null));
        }

        private static SkillDataDto GetSkills(Guid? id)
        {
            return new SkillDataDto(new ContextManager(), id);
        }

        [HttpPost]
        public ActionResult Save(SkillDataDto data)
        {
            var context = new ContextManager();
            //Setting data from result
            data.SelectedItem.SkillModifier = context.Get<Ability>(data.SelectedAbility.ToGuid().GetValueOrDefault());
            context.CreateOrUpdate(data.SelectedItem);
            return RedirectToAction("Index", "Skill", new { id = data.SelectedItem.ID });
            //return View("Creator/Skill", "CreatorLayoutPage", GetSkills(data.SelectedItem.ID));
        }
    }
}
