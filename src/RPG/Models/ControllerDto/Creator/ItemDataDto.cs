using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Models.ControllerDto.Creator
{
    public class ItemDataDto : DataDto
    {
        public ItemDataDto(){}

        public ItemDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<ItemBase>(selectedGuid.Value);
                SpecialMaterial = SelectedItem.Material != null ? SelectedItem.Material.ID : (Guid?)null;
                RequiredSlots = SelectedItem.RequiresSlots == null ? null : SelectedItem.RequiresSlots.Select(x => (int)x.Requirement).ToArray();
                if (RequiredSlots != null && RequiredSlots.Count(x => x == 12) == 2)
                {
                    for (int i = 0; i < RequiredSlots.Length; i++)
                    {
                        if (RequiredSlots[i] == 12)
                        {
                            RequiredSlots[i] = -1;
                            break;
                        }
                    }
                }
                
                GrantedSlots = SelectedItem.HasSlots == null ? null : SelectedItem.HasSlots.Select(x => (int)x.Requirement).ToArray();
                DamageString = SelectedItem.Damage == null ? null : SelectedItem.Damage.Amount.ToString();
                DamageType = SelectedItem.Damage == null ? (DamageType?)null : SelectedItem.Damage.Type;

                EnchantetBonuses = SelectedItem.EnchanmentsBonuses == null ? null : SelectedItem.EnchanmentsBonuses.Select(x => x.ID).ToArray();
                EnchantetSpecialAbilities = SelectedItem.EnchanmentAbilities == null ? null : SelectedItem.EnchanmentAbilities.Select(x => x.ID).ToArray();

                RequiresSpecialAbilities = SelectedItem.RequiresAbility == null ? null : SelectedItem.RequiresAbility.Select(x => x.SpecialAbilityGuid).ToArray();
            }
            Data = new List<ElementId>(manager.GetAll<ItemBase>().OrderBy(x => x.Name));
            Materials = new List<ElementId>(manager.GetAll<ItemMaterial>().OrderBy(x => x.Name));
            Bonuses = new List<ElementId>(manager.GetAll<Bonus>().OrderBy(x => x.Name));
            SpecialAbilities = new List<ElementId>(manager.GetAll<SpecialAbility>().OrderBy(x => x.Name));
        }

        public ItemBase SelectedItem { get; set; }
        public int[] RequiredSlots { get; set; }
        public int[] GrantedSlots { get; set; }
        public string DamageString { get; set; }
        public DamageType? DamageType { get; set; }
        public Guid? SpecialMaterial { get; set; }
        public Guid[] EnchantetBonuses { get; set; }
        public Guid[] EnchantetSpecialAbilities { get; set; }
        public Guid[] RequiresSpecialAbilities { get; set; }


        public IEnumerable<ElementId> Materials { get; set; }
        public IEnumerable<ElementId> Bonuses { get; set; }
        public IEnumerable<ElementId> SpecialAbilities { get; set; }
    }
}