using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Controllers.Creator
{
    public class RaceController : Controller
    {
        //
        // GET: /Race/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/Race", "CreatorLayoutPage", GetRace(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                new ContextManager().Delete<Race>(id.Value);
            }
            return View("Creator/Race", "CreatorLayoutPage", GetRace(null));
        }

        private static RaceDataDto GetRace(Guid? id)
        {
            return new RaceDataDto(new ContextManager(), id);
        }

        [HttpPost]
        public ActionResult Save(RaceDataDto data)
        {
            var manager = new ContextManager();
            Race raceToUpdate;
            
            if (data.SelectedItem.ID == Guid.Empty)
            {
                raceToUpdate = data.SelectedItem;
            }
            else
            {
                raceToUpdate = manager.Get<Race>(data.SelectedItem.ID);
            }
            raceToUpdate.Size = data.SelectedItem.Size;
            raceToUpdate.Name = data.SelectedItem.Name;
            raceToUpdate.Description = data.SelectedItem.Description;
            raceToUpdate.BaseSpeed = data.SelectedItem.BaseSpeed;

            if (data.KnownLanguages != null)
            {
                raceToUpdate.Languages = manager.GetList<Language>(data.KnownLanguages);
            }
            if (data.BonusLanguages != null)
            {
                raceToUpdate.BonusLanguages = manager.GetList<Language>(data.BonusLanguages);
            }
            if (data.FavoredClasses != null)
            {
                raceToUpdate.FavoredClasses = manager.GetList<ClassBase>(data.FavoredClasses);
            }
            if (data.RacialBonuses != null)
            {
                raceToUpdate.RaceBonuses = manager.GetList<Bonus>(data.RacialBonuses);
            }
            if (data.RacialAbilities != null)
            {
                raceToUpdate.RacialAbilities = manager.GetList<SpecialAbility>(data.RacialAbilities);
            }
            manager.CreateOrUpdate(raceToUpdate);
            return RedirectToAction("Index", "Race", new { id = raceToUpdate.ID });
            //return View("Creator/Race", "CreatorLayoutPage", GetRace(raceToUpdate.ID));
        }
    }
}
