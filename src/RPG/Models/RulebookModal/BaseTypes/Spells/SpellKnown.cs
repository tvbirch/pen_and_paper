using System.Collections.Generic;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellKnown : GameId
    {
        public int Level { get; set; }
        public List<SpellsKnownPrLevel> SpellsKnown { get; set; }

    }
}
