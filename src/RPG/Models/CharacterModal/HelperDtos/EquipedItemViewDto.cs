using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.BaseTypes.Items;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class EquipedItemViewDto
    {
        public List<EquipedItem> Item { get; set; }
        public ItemSlotRequirement Slot { get; set; }
        public Guid CharacterId { get; set; }
        public bool? SkipFirst { get; set; }
    }
}