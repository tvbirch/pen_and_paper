using System.Collections.Generic;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;


namespace RPG.Models.RulebookModal.Lists
{
    public class RoundableList<T> : List<T>, IRoundable where T : IRoundable
    {
        public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<AbilityScore> abilities)
        {
            if (Count != 0)
            {
                for (var i = Count - 1; i >= 0; i--)
                {
                    var remove = this[i].ChangeTime(currentSpecialAbilities,unit, abilities);
                    if(remove)
                    {
                        this.RemoveAt(i);
                    }
                }
            }
            return false;
        }
    }
}
