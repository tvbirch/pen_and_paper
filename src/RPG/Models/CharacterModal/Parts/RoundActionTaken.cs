using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.CharacterModal.Parts
{
    public class RoundActionTaken : GameId
    {
        public RoundAction Action { get; set; }
        public Guid? ActionUsedBy { get; set; }
    }
}