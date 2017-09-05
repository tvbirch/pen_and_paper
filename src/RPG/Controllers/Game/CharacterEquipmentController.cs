using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using RPG.Models.CharacterModal;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.ControllerDto.Game;
using RPG.Models.RulebookModal.BaseTypes.Items;

namespace RPG.Controllers.Game
{
    public class CharacterEquipmentController : ControllerBase
    {
        //
        // GET: /CharacterEquipment/

        public ActionResult Index(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Game/CharacterSelector");
            }
            return View("Game/CharacterEquipment", new CharacterEquipmentDto(Context, id.Value));
        }


        public ActionResult ChangeWealth(Guid id, bool addToWealth, string money)
        {
            if (id == Guid.Empty)
            {
                return Json("No character selected", JsonRequestBehavior.AllowGet);
            }
            var character = Context.LoadCharacter(id);
            if (addToWealth)
            {
                character.Inventory.Wealth.AddFromString(money);
            }
            else
            {
                character.Inventory.Wealth.RemoveFromString(money);
            }
            

            Context.Context.SaveChanges();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Purchase(Guid id, bool payForItem, Guid charId)
        {
            if (id == Guid.Empty)
            {
                return Json("No item selected", JsonRequestBehavior.AllowGet);
            }
            var item = Context.Get<ItemBase>(id);
            var character = Context.LoadCharacter(charId);
            if (payForItem)
            {
                if (character.Inventory.Wealth.CopperPrice.GetValueOrDefault() < item.Price.CopperPrice)
                {
                    return Json("Not enough money for item", JsonRequestBehavior.AllowGet);
                }
                character.Inventory.Wealth.CopperPrice -= item.Price.CopperPrice;
            }
            if (character.Inventory.BagItems == null)
            {
                character.Inventory.BagItems = new List<OwnedItem>();
            }
            character.Inventory.BagItems.Add(new OwnedItem
            {
                IsEquipped = false,
                Item = item,
            });
            Context.Context.SaveChanges();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult EquipItem(Guid id, Guid charId, bool dequip)
        {
            if (id == Guid.Empty)
            {
                return Json("No item selected", JsonRequestBehavior.AllowGet);
            }
            var character = Context.LoadCharacter(charId);
            if (character.Inventory.BagItems == null)
            {
                character.Inventory.BagItems = new List<OwnedItem>();
            }

            var items = Context.GetOwnedItems(id, character);
            if (items == null || !items.Any())
            {
                return Json("You dont own that item", JsonRequestBehavior.AllowGet);
            }

            if (dequip)
            {
                var item = items.FirstOrDefault(x => x.IsEquipped.GetValueOrDefault());
                if (item == null)
                {
                    return Json("You dont own that item", JsonRequestBehavior.AllowGet);
                }
                item.IsEquipped = false;
                Context.Context.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            var requiredSlots = items.First().Item.RequiresSlots.Select(x => x.Requirement);
            var emptySlots = character.GetEmptySlots();
            var needToRemoveItem = !requiredSlots.All(emptySlots.Contains);
            if (needToRemoveItem)
            {
                return Json("You need to remove items to make space for this one.", JsonRequestBehavior.AllowGet);
            }
            items.First(x => !x.IsEquipped.GetValueOrDefault()).IsEquipped = true;
            Context.Context.SaveChanges();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeQuantity(Guid id, Guid charId, int newQuantity)
        {
            if (id == Guid.Empty)
            {
                return Json("No item selected", JsonRequestBehavior.AllowGet);
            }

            var character = Context.LoadCharacter(charId);
            if (character.Inventory.BagItems == null)
            {
                character.Inventory.BagItems = new List<OwnedItem>();
            }

            var item = Context.GetOwnedItems(id, character);

            //Change quantaty
            if (item.Count == newQuantity)
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            if (item.Count < newQuantity)
            {
                var toAdd = newQuantity - item.Count;
                for (int i = 0; i < toAdd; i++)
                {
                    character.Inventory.BagItems.Add(new OwnedItem
                    {
                        IsEquipped = false,
                        Item = item.First().Item,
                        OwnerEnchamtmentBonues = item.First().OwnerEnchamtmentBonues,
                        OwnerMasterWorked = item.First().OwnerMasterWorked,
                        OwnerMaterial = item.First().OwnerMaterial,
                    });    
                }
            }
            else if (newQuantity < item.Count)
            {
                var toRemove = item.Count - newQuantity;
                for (int i = 0; i < toRemove; i++)
                {
                    var index = character.Inventory.BagItems.IndexOf(item[i]);
                    Context.Delete<OwnedItem>(character.Inventory.BagItems[index].ID);
                }
            }

            Context.Context.SaveChanges();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchItems(string q)
        {
            var result = Context.GetQueryable<ItemBase>().Where(x => x.Name.Contains(q)).ToList().Select(x => new SearchItem
            {
                id = x.ID.ToString(),
                text = x.Name,
                Description = x.Description,
                Price = x.Price.ToString(),
                Weight = x.Weight.ToString(),
            }).ToList();

            return Json(new { result }, JsonRequestBehavior.AllowGet);;
        }

    }
}
