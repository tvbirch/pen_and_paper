using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Models.RulebookModal.BaseTypes.Classes
{
    public class AbilityByClassLevel : GameId
    {
        [Required]
        public ClassBase Class { get; set; }
        [Required]
        public SpecialAbility Ability { get; set; }
        [Required]
        public int AvailableAtLevel { get; set; }
    }
}