using System;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellRequiretLevel : GameId
    {
        public int CasterLevel { get; set; }
        public ClassBase CasterClass { get; set; }
        public Spell Spell { get; set; }
    }
}
