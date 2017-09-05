using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.CharacterModal.Parts
{
    public class DamageTaken : GameId
    {
        public int Amount { get; set; }
    }
}