using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Languages;

namespace RPG.Models.ControllerDto.Creator
{
    public class LanguageDataDto : DataDto
    {
        public LanguageDataDto(){}

        public LanguageDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Language>(selectedGuid.Value);
            }
            Data = new List<ElementId>(manager.GetAll<Language>().OrderBy(x => x.Name));
        }

        public Language SelectedItem { get; set; }
    }
}