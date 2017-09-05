using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.Spells;

namespace RPG.Models.ControllerDto.Creator
{
    public class SpellCasterRequirementsDataDto : DataDto
    {
        public SpellCasterRequirementsDataDto(){}

        public SpellCasterRequirementsDataDto(ContextManager manager, Guid? selectedGuid, Guid? selectedScrGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Spell>(selectedGuid.Value);
            }
            if (selectedScrGuid.HasValue && selectedScrGuid != Guid.Empty)
            {
                SelectedRequirement = manager.Get<SpellRequiretLevel>(selectedScrGuid.Value);
                ClassGuid = SelectedRequirement.CasterClass.ID;
            }
            Data = new List<ElementId>(manager.GetAll<Spell>().OrderBy(x => x.Name));
            SpellRequirement = manager.GetQueryable<SpellRequiretLevel>().Where(x => x.Spell.ID == selectedGuid.Value).ToList();
            Classes = manager.GetAll<ClassBase>();
        }

        public Spell SelectedItem { get; set; }
        public SpellRequiretLevel SelectedRequirement { get; set; }
        public Guid? ClassGuid { get; set; }

        public IEnumerable<SpellRequiretLevel> SpellRequirement { get; set; }
        public IEnumerable<ClassBase> Classes { get; set; }
    }
}