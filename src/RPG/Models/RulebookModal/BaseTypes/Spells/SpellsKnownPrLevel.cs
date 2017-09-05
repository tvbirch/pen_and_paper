using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellsKnownPrLevel : GameId
    {
        public int SpellLevel { get; set; }
        public int NumberOfSpells { get; set; } //-1 = all
    }
}
