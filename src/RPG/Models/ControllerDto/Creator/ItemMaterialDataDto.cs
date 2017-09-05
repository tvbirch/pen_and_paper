using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Languages;

namespace RPG.Models.ControllerDto.Creator
{
    public class ItemMaterialDataDto : DataDto
    {
        public ItemMaterialDataDto (){}

        public ItemMaterialDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<ItemMaterial>(selectedGuid.Value);
            }
            Data = new List<ElementId>(manager.GetAll<ItemMaterial>().OrderBy(x => x.Name));
        }

        public ItemMaterial SelectedItem { get; set; }
    }
}