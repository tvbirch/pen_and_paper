using System;
using System.Collections.Generic;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;


namespace RPG.Models.RulebookModal.Lists
{
    public class RoundableDictonary<T> : Dictionary<Guid,T>, IRoundable where T : IRoundable
    {
        public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<AbilityScore> abilities)
        {
            if (Keys.Count != 0)
            {
                List<Guid> removals = new List<Guid>();
                foreach (Guid key in this.Keys)
                {
                    if (this[key].ChangeTime(currentSpecialAbilities,unit, abilities))
                    {
                        removals.Add(key);
                    }
                }
                foreach (Guid toRemove in removals)
                {
                    this.Remove(toRemove);
                }
            }
            return false;
        }
    }
}
