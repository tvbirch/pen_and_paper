using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Races;

namespace RPG.Models.ControllerDto.Creator
{
    public class RaceAgeDataDto : DataDto
    {
        public RaceAgeDataDto(){}

        public RaceAgeDataDto(ContextManager manager, Guid? selectedGuid, Guid? selectedAge)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Race>(selectedGuid.Value);
            }
            if (selectedAge.HasValue && selectedAge != Guid.Empty)
            {
                AgeCategoryAtAge = manager.Get<AgeCategoryAtAge>(selectedAge.Value);
            }
            Data = new List<ElementId>(manager.GetAll<Race>().OrderBy(x => x.Name));
            RaceAges = manager.GetQueryable<AgeCategoryAtAge>().Where(x => x.Race.ID == selectedGuid).Select(x => new ElementId
                {
                    ID = x.ID,
                    Name = x.Age + " - " + x.Category
                }).ToList();
        }

        public List<ElementId> RaceAges { get; set; }

        public Race SelectedItem { get; set; }
        public AgeCategoryAtAge AgeCategoryAtAge { get; set; }
    }
}