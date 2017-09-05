using System.Collections.Generic;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;


namespace RPG.Models.RulebookModal.Interfaces
{
    public interface IRoundable
    {
        bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<AbilityScore> abilities);
    }
}
