using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Currency;

namespace RPG.Models.CharacterModal.Parts
{
    public class Inventory : GameId
    {
        public Money Wealth { get; set; }
        public List<OwnedItem> BagItems { get; set; }
    }
}