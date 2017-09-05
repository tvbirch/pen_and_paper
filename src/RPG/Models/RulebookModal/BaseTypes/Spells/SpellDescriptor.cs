using System.Collections.Generic;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellDescriptor : ElementId
    {
        public List<Spell> SpellsUsingDescriptor { get; set; }
    }
}
