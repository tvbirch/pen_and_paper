using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Saves;

namespace RPG.Models.ControllerDto.Creator
{
    public class ClassSaveRateDataDto : DataDto
    {
        public ClassSaveRateDataDto(){}

        public ClassSaveRateDataDto(ContextManager manager, Guid? selectedGuid, Guid? selectedSaveRateGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<ClassBase>(selectedGuid.Value);
            }
            if (selectedSaveRateGuid.HasValue && selectedSaveRateGuid != Guid.Empty)
            {
                SelectedSaveRate = manager.Get<SaveRate>(selectedSaveRateGuid.Value);
                SaveGuid = SelectedSaveRate.Save.ID;
            }
            Data = new List<ElementId>(manager.GetAll<ClassBase>().OrderBy(x => x.Name));
            Saves = manager.GetAll<SaveType>();
            SaveRates = new List<SaveRate>(manager.GetQueryable<SaveRate>().Where(x => x.Class.ID == selectedGuid).ToList());
        }

        public ClassBase SelectedItem { get; set; }
        public SaveRate SelectedSaveRate { get; set; }
        public Guid? SaveGuid { get; set; }

        public IEnumerable<SaveRate> SaveRates { get; set; }
        public IEnumerable<SaveType> Saves { get; set; }
    }
}