using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Saves;

namespace RPG.Controllers.Creator
{
    public class ClassSaveRateController : Controller
    {
        //
        // GET: /ClassSaveRate/

        public ActionResult Index(Guid? id, Guid? saveId)
        {
            if (!id.HasValue || !saveId.HasValue)
            {
                return RedirectToAction("Index", "Class", new { id = id });
            }
            return View("Creator/ClassSaveRate", "CreatorLayoutPage", GetClassSAveRate(id, saveId));
        }

        public ActionResult Delete(Guid? id, Guid? saveId)
        {
            if (saveId.HasValue)
            {
                new ContextManager().Delete<SaveRate>(saveId.Value);
            }
            return View("Creator/ClassSaveRate", "CreatorLayoutPage", GetClassSAveRate(id, Guid.Empty));
        }

        [HttpPost]
        public ActionResult Save(ClassSaveRateDataDto data)
        {
            var context = new ContextManager();
            //Setting data from result
            SaveRate saveRateToUpdate;
            if (data.SelectedSaveRate.ID == Guid.Empty)
            {
                saveRateToUpdate = data.SelectedSaveRate;
            }
            else
            {
                saveRateToUpdate = context.Get<SaveRate>(data.SelectedSaveRate.ID);
            }

            saveRateToUpdate.Rate = data.SelectedSaveRate.Rate;
            if (data.SaveGuid != null)
            {
                saveRateToUpdate.Save = context.Get<SaveType>(data.SaveGuid.Value);
            }
            saveRateToUpdate.Class = context.Get<ClassBase>(data.SelectedItem.ID);


            context.CreateOrUpdate(saveRateToUpdate);

            return RedirectToAction("Index", "ClassSaveRate", new { id = data.SelectedItem.ID, saveId = saveRateToUpdate.ID});
            //return View("Creator/ClassSaveRate", "CreatorLayoutPage", GetClassSAveRate(data.SelectedItem.ID, data.SelectedSaveRate.ID));
        }

        private static ClassSaveRateDataDto GetClassSAveRate(Guid? id, Guid? saveId)
        {
            return new ClassSaveRateDataDto(new ContextManager(), id, saveId);
        }

    }
}
