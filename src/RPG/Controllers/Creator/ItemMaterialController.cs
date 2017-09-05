using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Items;

namespace RPG.Controllers.Creator
{
    public class ItemMaterialController : Controller
    {
        //
        // GET: /ItemMaterial/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/ItemMaterial", "CreatorLayoutPage", GetItemMaterials(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                new ContextManager().Delete<ItemMaterial>(id.Value);
            }
            return View("Creator/ItemMaterial", "CreatorLayoutPage", GetItemMaterials(null));
        }

        private static ItemMaterialDataDto GetItemMaterials(Guid? id)
        {
            return new ItemMaterialDataDto(new ContextManager(), id);
        }

        [HttpPost]
        public ActionResult Save(ItemMaterialDataDto data)
        {
            new ContextManager().CreateOrUpdate(data.SelectedItem);
            return RedirectToAction("Index", "ItemMaterial", new { id = data.SelectedItem.ID });
            //return View("Creator/ItemMaterial", "CreatorLayoutPage", GetItemMaterials(data.SelectedItem.ID));
        }

    }
}
