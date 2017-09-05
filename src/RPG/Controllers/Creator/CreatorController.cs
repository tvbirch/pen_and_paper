using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.RulebookModal.BaseTypes.Languages;

namespace RPG.Controllers.Creator
{
    public class CreatorController : Controller
    {
        //
        // GET: /Creator/

        public ActionResult Index()
        {
            return View("Creator/Creator", "CreatorLayoutPage");
        }

    }
}
