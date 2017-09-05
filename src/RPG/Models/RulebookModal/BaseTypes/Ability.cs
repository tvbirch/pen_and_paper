using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Lists;

namespace RPG.Models.RulebookModal.BaseTypes
{
    public class Ability : OrderedElementId
    {
        public bool IsPhycicalStat { get; set; }
        public List<AbilityScore> AbilityScoresUsingAbility { get; set; }
    }
}
