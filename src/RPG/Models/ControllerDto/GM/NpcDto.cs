using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.GmModal;
using RPG.Models.GmModal.World;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Alligments;

namespace RPG.Models.ControllerDto.GM
{
    public class NpcDto
    {
        public NpcDto() { }
        public NpcDto(ContextManager context, Guid? selectedNpc)
        {
            if (selectedNpc.HasValue)
            {
                SelectedItem = context.Get<NPC>(selectedNpc.Value);
                History = context.GetHistory(typeof(NPC), selectedNpc.Value);
            }
            else
            {
                History = new List<History>();
            }
            NPCs = context.Context.NPCs.ToList().Cast<ElementId>().ToList();
            NPCs.Insert(0,new ElementId{ID = Guid.Empty,Name = "N/A"});
            Locations = context.Context.Locations.ToList().Cast<ElementId>().ToList();
            AllAlligments = context.Context.Alligments.ToList().Cast<ElementId>().ToList();
        }
        public NPC SelectedItem { get; set; }
        public List<ElementId> NPCs { get; set; }
        public List<ElementId> Locations { get; set; }
        public List<ElementId> AllAlligments { get; set; }
        public List<History> History { get; set; }
    }
}