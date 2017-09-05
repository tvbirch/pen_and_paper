using System.Collections.Generic;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellSchool : ElementId
    {
        public List<Spell> SpellsUsingSchool { get; set; }
    }
}
