using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.CharacterModal;
using RPG.Models.ControllerDto.GM;
using RPG.Models.GmModal;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Controllers.GM
{
    public class GmOverviewController : ControllerBase
    {
        //
        // GET: /GMOverview/

        public ActionResult Index()
        {
            var dto = new CharactersDto(Context);
            return View("GM/GM", dto);
        }

        [HttpPost]
        public ActionResult Save(CharactersDto data)
        {
            var currentChars = Context.GetAll<GmCharacterView>();
            foreach (var gmCharacterView in currentChars)
            {
                Context.Delete<GmCharacterView>(gmCharacterView.ID);
            }
            foreach (var characterGuid in data.CharacterGuids)
            {
                var newView = new GmCharacterView
                {
                    CharacterGuid = characterGuid,
                };
                Context.CreateOrUpdate(newView);
            }

            var dto = new CharactersDto(Context);
            return View("GM/GM", dto);
        }

        public ActionResult RenderCharacterCard(Guid? id)
        {
            var character = id.HasValue ? Context.LoadCharacter(id.Value) : null;
            return PartialView("GM/CharacterCard", character);
        }
    }
}
