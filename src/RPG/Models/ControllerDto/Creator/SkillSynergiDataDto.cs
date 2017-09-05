using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Skills;

namespace RPG.Models.ControllerDto.Creator
{
    public class SkillSynergiDataDto : DataDto
    {
        public SkillSynergiDataDto(){}

        public SkillSynergiDataDto(ContextManager manager, Guid? skillId, Guid? skillSynergiId)
        {
            if (skillId.HasValue)
            {
                SelectedSkill = manager.Get<Skill>(skillId.Value);
            }
            if (skillSynergiId.HasValue && skillSynergiId != Guid.Empty)
            {
                SelectedItem = manager.Get<SkillSynergi>(skillSynergiId.Value);
            }
            var synergies = manager.GetQueryable<SkillSynergi>().Where(x => x.SynergiApplyToId == skillId).OrderBy(x => x.SynergiApplyTo.Name).ToList();
            Synergies = synergies;
            var allSkills = manager.GetAll<Skill>();
            Data = new List<ElementId>(allSkills);
            Skills = allSkills.Except(new []{SelectedSkill}).ToList();
        }

        public Skill SelectedSkill { get; set; }
        public SkillSynergi SelectedItem { get; set; }
        public IEnumerable<SkillSynergi> Synergies { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
    }
}