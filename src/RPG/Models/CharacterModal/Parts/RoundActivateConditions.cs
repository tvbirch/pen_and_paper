using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Time;

namespace RPG.Models.CharacterModal.Parts
{
    public class RoundActivateConditions : GameId
    {
        public TimeLimitUnit? AutoDismissAfter { get; set; }
        public bool? ManuallyActivated { get; set; }
        public Guid? ActivitedByAbility { get; set; }
        public Guid Condition { get; set; }
    }
}