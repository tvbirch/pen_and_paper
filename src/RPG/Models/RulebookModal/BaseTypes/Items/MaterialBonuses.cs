using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;

namespace RPG.Models.RulebookModal.BaseTypes.Items
{
    public class MaterialBonuses : ElementId
    {
        public ItemType ApplyToItemType { get; set; }
        public List<Bonus> Bonuses { get; set; }
        public Guid? RequiredAbility { get; set; }
    }
}