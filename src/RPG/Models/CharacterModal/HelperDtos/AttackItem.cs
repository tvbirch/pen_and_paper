using System;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class AttackItem
    {
        public Guid Id { get; set; }
        public RoundAction? ActionToUse { get; set; }
        public CalculatedString AttackBonus { get; set; }
        public CalculatedString Damage { get; set; }
        public string CriticalRange { get; set; }
        public string Range { get; set; }
        public string AttackType { get; set; }
        public string Name { get; set; }
        public bool CanCurrentlyAttack { get; set; }
    }
}
