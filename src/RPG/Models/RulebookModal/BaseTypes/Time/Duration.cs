using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;


namespace RPG.Models.RulebookModal.BaseTypes.Time
{

    public class Duration : GameId
    {
        public DurationUnit? Unit { get; set; }
        public int? Amount { get; set; }
        public Ability DurationAbilityModifier { get;  set; }
        

        public int GetNumberOfRounds(List<AbilityScore> abilities, GetBonusDto bonusDto)
        {
            int multiplyer = 1;
            switch (Unit)
            {
                    case DurationUnit.Instant:
                    case DurationUnit.Attack:
                        return 0;
                    case DurationUnit.Rounds:
                        multiplyer = 1;
                        break;
                    case DurationUnit.Minutes:
                        multiplyer = 10;
                        break;
                    case DurationUnit.Hours:
                        multiplyer = 600;
                        break;
            }
            return (Amount.GetValueOrDefault() + (DurationAbilityModifier != null ? abilities.First(x => x.Ability.ID == DurationAbilityModifier.ID).GetCurrentModifier(bonusDto) : 0)) * multiplyer;
        }
    }
}
