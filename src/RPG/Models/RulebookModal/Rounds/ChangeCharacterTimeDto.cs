using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Time;

namespace RPG.Models.RulebookModal.Rounds
{
    public class ChangeCharacterTimeDto
    {
        public TimeLimitUnit TimeUnit { get; set; }
        public GetBonusDto Bonus { get; set; }
        public bool? TargetHit { get; set; }
    }
}