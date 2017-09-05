using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.ControllerDto.Game;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Controllers.Game
{
    public class CharacterEquipmentModifierController : ControllerBase
    {
        //
        // GET: /CharacterEquipmentModifier/

        public ActionResult Index(Guid? id, Guid? itemId)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "CharacterSelector");
            }
            return View("Game/CharacterEquipmentModifier", new CharacterEquipmentModifierDto(Context, id.Value, itemId));
        }

        [HttpPost]
        public ActionResult Save(CharacterEquipmentModifierDto dto)
        {
            if (dto == null || dto.Character == null)
            {
                return RedirectToAction("Index", "CharacterSelector");
            }
            if (dto.SelectedItem.HasValue)
            {
                var ownedItemFromDb = Context.Get<OwnedItem>(dto.SelectedItem.Value);
                ownedItemFromDb.OwnerEnchamtmentBonues = dto.EnchantmentBonus;
                ownedItemFromDb.OwnerMasterWorked = dto.Masterworked;
                ownedItemFromDb.OwnerMaterial = dto.SpecialMaterial.HasValue
                    ? Context.Get<ItemMaterial>(dto.SpecialMaterial.Value)
                    : null;
                ownedItemFromDb.EnchanmentsBonuses = Context.GetList<Bonus>(dto.EnchantetBonuses);
                ownedItemFromDb.EnchanmentAbilities = Context.GetList<SpecialAbility>(dto.EnchantetSpecialAbilities);    
                Context.CreateOrUpdate(ownedItemFromDb);
            }
            

            return View("Game/CharacterEquipmentModifier", new CharacterEquipmentModifierDto(Context, dto.Character.ID, dto.SelectedItem));
        }
    }
}
