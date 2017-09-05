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
    public class SkillDataDto : DataDto
    {
        public SkillDataDto(){}

        public SkillDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Skill>(selectedGuid.Value);
                if (SelectedItem.SkillModifier != null)
                {
                    SelectedAbility = SelectedItem.SkillModifier.ID.ToString();
                }
            }
            var skills = manager.GetAll<Skill>().OrderBy(x => x.Name);
            
            Data = new List<ElementId>(skills);
            AbilityOptions = manager.GetAll<Ability>();
        }

        public string SelectedAbility { get; set; }
        public Skill SelectedItem { get; set; }
        public IEnumerable<Ability> AbilityOptions { get; set; }
    }
}