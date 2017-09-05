using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Items;

namespace RPG.Controllers.Creator
{
    public class ItemMaterialBonusController : Controller
    {
        //
        // GET: /ItemMaterialBonues/

        public ActionResult Index(Guid? id, Guid? itemMaterialBonuesId)
        {
            if (!id.HasValue || !itemMaterialBonuesId.HasValue)
            {
                return RedirectToAction("Index", "ItemMaterial", new { id = id });
            }
            return View("Creator/ItemMaterialBonus", "CreatorLayoutPage", GetItemMaterialBonus(id, itemMaterialBonuesId));
        }

        public ActionResult Delete(Guid? id, Guid? itemMaterialBonuesId)
        {
            if (itemMaterialBonuesId.HasValue)
            {
                new ContextManager().Delete<MaterialBonuses>(itemMaterialBonuesId.Value);
            }
            return View("Creator/ItemMaterialBonus", "CreatorLayoutPage", GetItemMaterialBonus(id, Guid.Empty));
        }

        [HttpPost]
        public ActionResult Save(ItemMaterialBonusDataDto data)
        {
            var manager = new ContextManager();
            MaterialBonuses materialToUpdate;

            if (data.SelectedItem.ID == Guid.Empty)
            {
                materialToUpdate = data.SelectedItem;
            }
            else
            {
                materialToUpdate = manager.Get<MaterialBonuses>(data.SelectedItem.ID);
            }
            
            materialToUpdate.Name = data.SelectedItem.Name;
            materialToUpdate.Description = data.SelectedItem.Description;
            materialToUpdate.ApplyToItemType = data.SelectedItem.ApplyToItemType;
            if (data.GrantedBonues != null)
            {
                materialToUpdate.Bonuses = manager.GetList<Bonus>(data.GrantedBonues);
            }

            var itemMaterial = manager.Get<ItemMaterial>(data.SelectedItemMaterial.ID);
            itemMaterial.MaterialBonues.Add(materialToUpdate);

            manager.CreateOrUpdate(itemMaterial);

            return RedirectToAction("Index", "ItemMaterialBonus", new { id = data.SelectedItem.ID, itemMaterialBonuesId = itemMaterial.ID });
            //return View("Creator/ItemMaterialBonus", "CreatorLayoutPage", GetItemMaterialBonus(data.SelectedItemMaterial.ID, materialToUpdate.ID));
        }

        private static ItemMaterialBonusDataDto GetItemMaterialBonus(Guid? itemMaterialId, Guid? itemMaterialBonuesId)
        {
            return new ItemMaterialBonusDataDto(new ContextManager(), itemMaterialId, itemMaterialBonuesId);
        }

    }
}
