using System;
using System.Collections.Generic;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;


namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class SpellPrDay : GameId
    {
        public int Level { get; set; }
        public List<SpellsPrLevel> NumberOfSpells { get; set; }
    }
}
