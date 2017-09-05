using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Models.ControllerDto.Game
{
    public class CharacterEquipmentModifierDto : CharacterDto
    {
        public Guid? SelectedItem { get; set; }
        public string Name { get; set; }
        public List<ElementId> OwnedItems { get; set; }
        public bool Masterworked { get; set; }
        public int EnchantmentBonus { get; set; }
        public Guid? SpecialMaterial { get; set; }
        public Guid[] EnchantetBonuses { get; set; }
        public Guid[] EnchantetSpecialAbilities { get; set; }


        public IEnumerable<ElementId> Materials { get; set; }
        public IEnumerable<ElementId> Bonuses { get; set; }
        public IEnumerable<ElementId> SpecialAbilities { get; set; }

        public CharacterEquipmentModifierDto()
        {
            
        }

        public CharacterEquipmentModifierDto(ContextManager context, Guid value, Guid? itemId)
        {
            Materials = new List<ElementId>(context.GetAll<ItemMaterial>().OrderBy(x => x.Name));
            Bonuses = new List<ElementId>(context.GetQueryable<Bonus>().Where(x => x.CanTakeAsEnchantment).OrderBy(x => x.Name)).ToList();
            SpecialAbilities = new List<ElementId>(context.GetQueryable<SpecialAbility>().Where(x => x.CanTakeAsEnchantment).OrderBy(x => x.Name)).ToList();

            Character = context.LoadCharacter(value);
            OwnedItems = new List<ElementId>();
            OwnedItems.Add(new ElementId
            {
                Name = "N/A",
                ID = Guid.Empty
            });
            OwnedItems.AddRange(Character.GetEquippedItems().Select(x => new ElementId
            {
                Name = x.Item.Name,
                Description = x.Item.Description,
                ID = x.ID
            }).ToList());

            if (itemId.HasValue)
            {
                SelectedItem = itemId;
                var loalItem = context.Get<OwnedItem>(itemId.Value);
                Name = loalItem.Item.Name;
                Masterworked = loalItem.OwnerMasterWorked.GetValueOrDefault();
                SpecialMaterial = loalItem.OwnerMaterial != null ? loalItem.OwnerMaterial.ID : (Guid?)null;
                EnchantetBonuses = loalItem.EnchanmentsBonuses == null ? null : loalItem.EnchanmentsBonuses.Select(x => x.ID).ToArray();
                EnchantetSpecialAbilities = loalItem.EnchanmentAbilities == null ? null : loalItem.EnchanmentAbilities.Select(x => x.ID).ToArray();
                EnchantmentBonus = loalItem.OwnerEnchamtmentBonues.GetValueOrDefault();
            }
        }
    }
}