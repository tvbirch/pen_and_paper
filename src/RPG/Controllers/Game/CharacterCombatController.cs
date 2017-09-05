using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.ControllerDto.Game;

namespace RPG.Controllers.Game
{
    public class CharacterCombatController : ControllerBase
    {
        //
        // GET: /CharacterMelee/

        public ActionResult Index(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Game/CharacterSelector");
            }
            return View("Game/CharacterCombat", new CharacterMeleeDto(Context, id.Value));
        }

    }
}
