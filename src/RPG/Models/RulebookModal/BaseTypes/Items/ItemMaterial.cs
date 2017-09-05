using System;
using System.Collections.Generic;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;

namespace RPG.Models.RulebookModal.BaseTypes.Items
{
    public class ItemMaterial : ElementId
    {
        public List<MaterialBonuses> MaterialBonues { get; set; }


        public virtual ICollection<ItemBase> Items { get; set; }
        public virtual ICollection<OwnedItem> OwnedItems { get; set; }
    }
}
