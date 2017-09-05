using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Languages;

namespace RPG.Controllers.Creator
{
    public class LanguageController : Controller
    {
        //
        // GET: /Language/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/Language", "CreatorLayoutPage", GetLanguages(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                new ContextManager().Delete<Language>(id.Value);
            }
            return View("Creator/Language", "CreatorLayoutPage", GetLanguages(null));
        }

        private static LanguageDataDto GetLanguages(Guid? id)
        {
            return new LanguageDataDto(new ContextManager(), id);
        }

        [HttpPost]
        public ActionResult Save(LanguageDataDto data)
        {
            new ContextManager().CreateOrUpdate(data.SelectedItem);
            return RedirectToAction("Index", "Language", new { id = data.SelectedItem.ID });
            //return View("Creator/Language", "CreatorLayoutPage", GetLanguages(data.SelectedItem.ID));
        }
    }
}
