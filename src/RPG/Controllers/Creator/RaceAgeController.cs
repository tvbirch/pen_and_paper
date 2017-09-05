using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Races;

namespace RPG.Controllers.Creator
{
    public class RaceAgeController : ControllerBase
    {
        //
        // GET: /RaceAge/
        public ActionResult Index(Guid? id, Guid? ageId)
        {
            if (!id.HasValue || id == Guid.Empty)
            {
                return RedirectToAction("Index", "Race", new { id = id });
            }
            return View("Creator/RaceAge", "CreatorLayoutPage", GetAgeCategoryAtAge(id, ageId));
        }

        public ActionResult Delete(Guid? id, Guid? ageId)
        {
            if (ageId.HasValue)
            {
                Context.Delete<AgeCategoryAtAge>(ageId.Value);
            }
            return View("Creator/RaceAge", "CreatorLayoutPage", GetAgeCategoryAtAge(id, Guid.Empty));
        }

        [HttpPost]
        public ActionResult Save(RaceAgeDataDto data)
        {
            AgeCategoryAtAge acatToUpdate;
            if (data.AgeCategoryAtAge.ID == Guid.Empty)
            {
                acatToUpdate = data.AgeCategoryAtAge;
            }
            else
            {
                acatToUpdate = Context.Get<AgeCategoryAtAge>(data.AgeCategoryAtAge.ID);
            }


            acatToUpdate.Race = Context.Get<Race>(data.SelectedItem.ID);
            acatToUpdate.Age = data.AgeCategoryAtAge.Age;
            acatToUpdate.Category = data.AgeCategoryAtAge.Category;

            Context.CreateOrUpdate(acatToUpdate);
            return RedirectToAction("Index", "RaceAge", new { id = data.SelectedItem.ID, ageId = acatToUpdate.ID });
            //return View("Creator/RaceAge", "CreatorLayoutPage", GetAgeCategoryAtAge(data.SelectedItem.ID, acatToUpdate.ID));
        }

        private RaceAgeDataDto GetAgeCategoryAtAge(Guid? idGuid, Guid? age)
        {
            return new RaceAgeDataDto(Context, idGuid, age);
        }

    }
}
