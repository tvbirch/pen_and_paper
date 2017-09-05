using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.CharacterModal.Parts
{
    public class RoundActivateAbilities :GameId
    {
        public Guid AbilityId { get; set; }
        public List<TimeLimitUnitParsed> ActiveTime { get; set; }
        public int Charges { get; set; }
        public bool IsActive { get; set; }
        public decimal? Multiplier { get; set; }
    }
}