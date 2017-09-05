using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Lists;


namespace RPG.Models.RulebookModal.BaseTypes.Skills
{
    public class Skill : ElementId
    {
        public bool UseUntrained { get; set; }
        public bool ArmorCheckPeneltyApply { get; set; }
        public bool ArmorCheckPeneltyApplyDouble { get; set; }
        public Ability SkillModifier { get; set; }
        public bool RequiresMovement { get; set; }

        public virtual ICollection<ClassBase> ClassSkillFor { get; set; }
        public virtual ICollection<SkillSynergi> SynergiFrom { get; set; }
        public virtual ICollection<SkillSynergi> SynergiApplyTo { get; set; }
        public virtual ICollection<SkillRank> SkillRanks { get; set; }
    }
}
