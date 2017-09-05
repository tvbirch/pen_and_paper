using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Items
{
    public class ItemSlotRequirements : GameId
    {
        public ItemSlotRequirement Requirement { get; set; }
    }
}