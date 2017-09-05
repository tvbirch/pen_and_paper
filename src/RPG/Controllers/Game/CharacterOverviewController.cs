using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.ControllerDto.Game;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Controllers.Game
{
    public class CharacterOverviewController : ControllerBase
    {
        //
        // GET: /CharacterOverview/

        public ActionResult Index(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Game/CharacterSelector");
            }
            return View("Game/CharacterOverview", new CharacterOverviewDto(Context,id.Value));
        }
    }
}
