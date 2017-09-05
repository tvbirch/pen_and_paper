using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class CalculatedSkill
    {
        public bool IsClassSkill { get; set; }
        public string Name { get; set; }
        public bool UseUntrained { get; set; }
        public string Ability { get; set; }

        public CalculatedString Ranks { get; set; }
        
        //public int AbilityBonues { get; set; }
        public int SkillRanks { get; set; }
        //public int MiscModifier { get; set; }
        //public int SkillModifier { get; set; }
    }
}