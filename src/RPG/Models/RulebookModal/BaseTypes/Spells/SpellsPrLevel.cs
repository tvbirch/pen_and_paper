using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellsPrLevel : GameId
    {
        public int SpellLevel { get; set; }
        public int SpellCastable { get; set; }
    }
}
