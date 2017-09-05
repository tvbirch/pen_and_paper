using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.BaseTypes.Items;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class EquipedItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ItemSlotRequirements> Slot { get; set; }
    }
}