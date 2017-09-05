using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RPG.Models.CharacterModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;


namespace RPG.Models.RulebookModal.BaseTypes.Languages
{
    public class Language : ElementId
    {

        [InverseProperty("Languages")]
        public virtual ICollection<Race> RacialLangageFor { get; set; }
        [InverseProperty("BonusLanguages")]
        public virtual ICollection<Race> BonusLangageFor { get; set; }
        public virtual ICollection<Character> Character { get; set; }


        public void AddBonues(List<Bonus> bonues)
        {
            return;
        }

        public void RemoveBonues(List<Bonus> bonues)
        {
            return;
        }

        public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<Ability> abilities)
        {
            return false;
        }

        public bool ChangeEncouter(GetBonusDto bonusDto)
        {
            return false;
        }
    }
}
