using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;

namespace RPG.Models.RulebookModal.BaseTypes.Conditions
{
    public class Condition : ElementId
    {
        public List<Bonus> Bonuses { get; set; }
        public TimeLimitUnit? AutoDismissAfter { get; set; }

        public Condition IfAlreadyActiveApplyCondition { get; set; }

        public virtual ICollection<SpecialAbility> SpecialAbilitiesUsingCondition { get; set; }
    }
}