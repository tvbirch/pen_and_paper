using System;
using RPG.Models.CharacterModal.Parts;

namespace RPG.Models.RulebookModal.BaseTypes.Items
{
    public class ItemSlot
    {
        public ItemSlotRequirement Slot { get; private set; }
        public OwnedItem Item { get; private set; }
        public Guid? EquipedId { get; set; }

        public ItemSlot(ItemSlotRequirement slot)
        {
            Slot = slot;
        }
        public ItemSlot(ItemSlotRequirement slot, OwnedItem item, Guid equipedGuid)
        {
            Slot = slot;
            Item = item;
            EquipedId = equipedGuid;
        }
        public bool IsEmptySlot()
        {
            return Item == null;
        }

        public OwnedItem SetItem(OwnedItem item, Guid? equipedGuid)
        {
            var oldItem = Item;
            Item = item;
            EquipedId = equipedGuid;
            return oldItem;
        }

        public OwnedItem EmptySlot()
        {
            var oldItem = Item;
            Item = null;
            EquipedId = null;
            return oldItem;
        }
    }
}
