using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.GmModal
{
    public class GmCharacterView : GameId
    {
        public Guid CharacterGuid { get; set; }
    }
}