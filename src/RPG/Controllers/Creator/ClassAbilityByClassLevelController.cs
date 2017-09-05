using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Controllers.Creator
{
    public class ClassAbilityByClassLevelController : Controller
    {
        //
        // GET: /ClassAbilityByClassLevel/
        //ClassAbilityByClassLevelDataDto
        public ActionResult Index(Guid? id, Guid? abilityByLevelId)
        {
            if (!id.HasValue || id == Guid.Empty)
            {
                return RedirectToAction("Index", "Class", new { id = id });
            }
            return View("Creator/AbilityByClassLevel", "CreatorLayoutPage", GetAbilityByClassLevel(id, abilityByLevelId));
        }

        public ActionResult Delete(Guid? id, Guid? abilityByLevelId)
        {
            if (abilityByLevelId.HasValue)
            {
                new ContextManager().Delete<AbilityByClassLevel>(abilityByLevelId.Value);
            }
            return View("Creator/AbilityByClassLevel", "CreatorLayoutPage", GetAbilityByClassLevel(id, Guid.Empty));
        }

        [HttpPost]
        public ActionResult Save(ClassAbilityByClassLevelDataDto data)
        {
            var context = new ContextManager();
            AbilityByClassLevel abclToUpdate;
            if (data.SelectedAbilityByClassLevel.ID == Guid.Empty)
            {
                abclToUpdate = data.SelectedAbilityByClassLevel;
            }
            else
            {
                abclToUpdate = context.Get<AbilityByClassLevel>(data.SelectedAbilityByClassLevel.ID);
            }

            if (data.Ability.HasValue)
            {
                abclToUpdate.Ability = context.Get<SpecialAbility>(data.Ability.Value);
            }
            else
            {
                abclToUpdate.Ability = null;
            }

            abclToUpdate.Class = context.Get<ClassBase>(data.SelectedItem.ID);
            abclToUpdate.AvailableAtLevel = data.SelectedAbilityByClassLevel.AvailableAtLevel;
            
            context.CreateOrUpdate(abclToUpdate);
            return RedirectToAction("Index", "ClassAbilityByClassLevel", new { id = data.SelectedItem.ID, abilityByLevelId = abclToUpdate.ID });
            //return View("Creator/AbilityByClassLevel", "CreatorLayoutPage", GetAbilityByClassLevel(data.SelectedItem.ID, abclToUpdate.ID));
        }

        private static ClassAbilityByClassLevelDataDto GetAbilityByClassLevel(Guid? classId, Guid? abilityId)
        {
            return new ClassAbilityByClassLevelDataDto(new ContextManager(), classId, abilityId);
        }
    }
}
