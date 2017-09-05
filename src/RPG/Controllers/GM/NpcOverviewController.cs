using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.ControllerDto.GM;
using RPG.Models.ControllerDto.GM.Helper;
using RPG.Models.GmModal.World;
using RPG.Models.RulebookModal.BaseTypes.Alligments;

namespace RPG.Controllers.GM
{
    public class NpcOverviewController : ControllerBase
    {

        // GET: NpcOverview
        public ActionResult Index(Guid? id)
        {
            var dto = new NpcDto(Context, id);
            return View("GM/NPC", dto);
        }

        public ActionResult Save(NpcDto data)
        {
            NPC npcToUpdate;
            if (data.SelectedItem.ID != Guid.Empty)
            {
                npcToUpdate = Context.Get<NPC>(data.SelectedItem.ID);
            }
            else
            {
                npcToUpdate = new NPC();
            }

            npcToUpdate.Gender = data.SelectedItem.Gender;
            npcToUpdate.Profession = data.SelectedItem.Profession;
            npcToUpdate.Race = data.SelectedItem.Race;
            npcToUpdate.Affiliations = data.SelectedItem.Affiliations;
            npcToUpdate.Age = data.SelectedItem.Age;
            npcToUpdate.Alligment= data.SelectedItem.Alligment;
            npcToUpdate.CombatStatistics = data.SelectedItem.CombatStatistics;
            npcToUpdate.RelationToParty = data.SelectedItem.RelationToParty;
            npcToUpdate.VoiceMannersPersonality = data.SelectedItem.VoiceMannersPersonality;
            npcToUpdate.Description = data.SelectedItem.Description;
            npcToUpdate.Name = data.SelectedItem.Name;

            if (data.SelectedItem.Location == null)
            {
                npcToUpdate.Location = null;
            }
            else
            {
                npcToUpdate.Location = Context.Get<Location>(data.SelectedItem.Location.ID);
            }
            if (data.SelectedItem.Alligment == null)
            {
                npcToUpdate.Alligment = null;
            }
            else
            {
                npcToUpdate.Alligment = Context.Get<Alligment>(data.SelectedItem.Alligment.ID);
            }

            Context.CreateOrUpdate(npcToUpdate);

            return RedirectToAction("Index", "NpcOverview", new { id = npcToUpdate.ID });
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                Context.Delete<NPC>(id.Value);
            }
            var dto = new NpcDto(Context, null);
            return View("GM/NPC", dto);
        }
    }
}