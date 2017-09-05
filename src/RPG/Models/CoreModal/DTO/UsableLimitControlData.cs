using System;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.CoreModal.DTO
{
    public class UsableLimitControlData
    {
        public int[] UsableAmountAtLevelList { get; set; }
        public TimeLimitUnit? RegainAfter { get; set; }
        public DurationUnit? DurationUnit { get; set; }
        public int? DurationFixedValue { get; set; }
        public Guid? DurationAbility { get; set; }
        public RoundAction? ActionRequired { get; set; }
        public int? AmoutFixedValue { get; set; }
        public Guid? AmountAbility { get; set; }
        public Guid? AmountClass { get; set; }
    }
}
