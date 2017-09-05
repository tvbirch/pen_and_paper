using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RPG.Models.ControllerDto.Game;

namespace RPG.Controllers.Game
{
    public class CharacterSelectorController : ControllerBase
    {
        //
        // GET: /CharacterSelector/

        public ActionResult Index()
        {
            return View("Game/CharacterSelector", GetCharacters());
        }

        private CharacterSelectorDto GetCharacters()
        {
            return new CharacterSelectorDto(Context);
        }
    }
}
