using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances;

namespace RPG.Models.RulebookModal.BaseTypes.Races
{
    public class AgeCategoryAtAge : GameId
    {
        [Required]
        public Race Race { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public AgeCategory Category { get; set; }
    }
}