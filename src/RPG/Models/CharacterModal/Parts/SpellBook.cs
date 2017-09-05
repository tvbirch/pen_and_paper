using System.Collections.Generic;
using System.Linq;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.BaseTypes.Spells;

namespace RPG.Models.CharacterModal.Parts
{
    public class SpellBook
    {
        private List<Spell> SpellsInBook { get; set; }

        //public SpellBook(_Classes classes, Rulebook rulebook)
        //{
        //    _classes = classes;
        //    _rulebook = rulebook;
        //}

        //public List<Spell> GetSpellsInSpellBook()
        //{
        //    var allSpellsKnown = new List<Spell>();
        //    foreach (var currentClass in _classes.CurrentClasses)
        //    {
        //        if (currentClass.Class.Spells != null && currentClass.Class.Spells.SpellKnown != null)
        //        {
        //            foreach (var spellKnown in currentClass.Class.Spells.SpellKnown)
        //            {
        //                if (spellKnown.Number == null)
        //                {
        //                    //Finding all spells castble by current class
        //                    allSpellsKnown.AddRange(
        //                        _rulebook.Spells.Where(
        //                            spell =>
        //                                spell.CasterRequirements.Any(
        //                                    x =>
        //                                        x.CasterClass == currentClass.Class &&
        //                                        x.CasterLevel == spellKnown.SpellLevel)));
        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //            }
        //        }
        //    }
        //    if (SpellsInBook != null)
        //    {
        //        allSpellsKnown.AddRange(SpellsInBook);
        //    }
        //    return allSpellsKnown;
        //}
    }
}
