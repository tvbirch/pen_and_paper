using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Conditions;

namespace RPG.Controllers.Creator
{
    public class ConditionController : ControllerBase
    {
        //
        // GET: /Condition/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/Condition", "CreatorLayoutPage", GetConditions(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                Context.Delete<Condition>(id.Value);
            }
            return View("Creator/Condition", "CreatorLayoutPage", GetConditions(null));
        }

        private ConditionDataDto GetConditions(Guid? id)
        {
            return new ConditionDataDto(Context, id);
        }

        [HttpPost]
        public ActionResult Save(ConditionDataDto data)
        {
            Condition condition;

            if (data.SelectedItem.ID == Guid.Empty)
            {
                condition = data.SelectedItem;
            }
            else
            {
                condition = Context.Get<Condition>(data.SelectedItem.ID);
            }

            condition.Name = data.SelectedItem.Name;
            condition.Description = data.SelectedItem.Description;

            condition.Bonuses = Context.GetList<Bonus>(data.BonusesSelected);
            if (data.IfAlreadyActiveApplyCondition.HasValue)
            {
                condition.IfAlreadyActiveApplyCondition =
                    Context.Get<Condition>(data.IfAlreadyActiveApplyCondition.Value);
            }
            else
            {
                condition.IfAlreadyActiveApplyCondition = null;
            }

            Context.CreateOrUpdate(condition);
            return RedirectToAction("Index", "Condition", new { id = condition.ID });
            //return View("Creator/Condition", "CreatorLayoutPage", GetConditions(condition.ID));
        }

    }
}
