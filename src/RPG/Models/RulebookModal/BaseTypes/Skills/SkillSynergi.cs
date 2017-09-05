using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Skills
{
    public class SkillSynergi : ElementId
    {
        public Guid SynergiFromId { get; set; }
        public virtual Skill SynergiFrom { get; set; }

        public Guid SynergiApplyToId { get; set; }
        public virtual Skill SynergiApplyTo { get; set; }

        public string Condition { get; set; }
    }
}