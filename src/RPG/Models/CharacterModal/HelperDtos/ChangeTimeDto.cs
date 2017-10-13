using RPG.Models.RulebookModal.BaseTypes.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class ChangeTimeDto
    {
        public Guid CharacterId { get; set; }
        public TimeLimitUnit TimeLimitUnit { get; set; }
        public Character Character { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public bool? HitTarget { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public GetBonusDto BonusDto { get; set; }
    }
}