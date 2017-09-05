using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal.Parts;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class TimeChangeRemoval
    {
        public List<RoundActionTaken> UsedActions { get; set; }
        public List<RoundActivateAbilities> DeaktivatedAbilities { get; set; }
        public List<RoundActivateConditions> DeaktivatedConditions { get; set; }
        public List<DamageTaken> DamagesToRemove { get; set; }
    }
}