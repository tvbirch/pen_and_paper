using System.Collections.Generic;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellComponent : ElementId
    {
        public List<Spell> SpellsUsingComponent { get; set; }
    }
}
