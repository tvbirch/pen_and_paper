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
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Models.ControllerDto.Creator
{
    public class ItemMaterialBonusDataDto : DataDto
    {
        public ItemMaterialBonusDataDto(){}

        public ItemMaterialBonusDataDto(ContextManager manager, Guid? itemMaterialId, Guid? itemMaterialBonuesId)
        {
            if (itemMaterialId.HasValue)
            {
                SelectedItemMaterial = manager.Get<ItemMaterial>(itemMaterialId.Value);
                RelatedBonuses = new List<ElementId>(SelectedItemMaterial.MaterialBonues);
            }
            if (itemMaterialBonuesId.HasValue && itemMaterialBonuesId != Guid.Empty)
            {
                SelectedItem = manager.Get<MaterialBonuses>(itemMaterialBonuesId.Value);
                GrantedBonues = SelectedItem.Bonuses.Select(x => x.ID).ToArray();
            }
            Data = new List<ElementId>(manager.GetAll<ItemMaterial>());

            AllBonuses = new List<ElementId>(manager.GetAll<Bonus>());
            SpecialAbilities = new List<ElementId>(manager.GetAll<SpecialAbility>());
        }

        public ItemMaterial SelectedItemMaterial { get; set; }
        public MaterialBonuses SelectedItem { get; set; }
        public IEnumerable<ElementId> AllBonuses { get; set; }
        public IEnumerable<ElementId> RelatedBonuses { get; set; }
        public IEnumerable<ElementId> SpecialAbilities { get; set; }

        public Guid[] GrantedBonues { get; set; }
    }
}