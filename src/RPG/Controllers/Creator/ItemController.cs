using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Controllers.Creator
{
    public class ItemController : Controller
    {
        //
        // GET: /Item/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/Item", "CreatorLayoutPage", GetItem(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                new ContextManager().Delete<ItemBase>(id.Value);
            }
            return View("Creator/Item", "CreatorLayoutPage", GetItem(null));
        }

        private static ItemDataDto GetItem(Guid? id)
        {
            return new ItemDataDto(new ContextManager(), id);
        }

        [HttpPost]
        public ActionResult Save(ItemDataDto data)
        {
            var manager = new ContextManager();
            ItemBase itemToUpdate;

            if (data.SelectedItem.ID == Guid.Empty)
            {
                itemToUpdate = data.SelectedItem;
            }
            else
            {
                itemToUpdate = manager.Get<ItemBase>(data.SelectedItem.ID);
            }
            if (data.DamageType.HasValue && !string.IsNullOrEmpty(data.DamageString))
            {
                if (itemToUpdate.Damage == null)
                {
                    itemToUpdate.Damage = new Damage();
                }
                itemToUpdate.Damage.Type = data.DamageType.Value;
                var newDiceRoll = DiceRoll.Parse(data.DamageString);
                if (itemToUpdate.Damage.Amount == null)
                {
                    itemToUpdate.Damage.Amount = newDiceRoll;
                }
                else
                {
                    itemToUpdate.Damage.Amount.FixedAmount = newDiceRoll.FixedAmount;
                    itemToUpdate.Damage.Amount.Dice = newDiceRoll.Dice;
                }
            }
            else
            {
                itemToUpdate.Damage = null;
            }
            if (data.RequiredSlots != null)
            {
                itemToUpdate.RequiresSlots = data.RequiredSlots.Select(x => new ItemSlotRequirements
                {
                    Requirement = x == -1 ? ItemSlotRequirement.WeaponHand : (ItemSlotRequirement)x
                }).ToList();
            }
            if (data.GrantedSlots != null)
            {
                itemToUpdate.HasSlots = data.GrantedSlots.Select(x => new ItemSlotRequirements
                {
                    Requirement = (ItemSlotRequirement)x
                }).ToList();
            }
            if (data.EnchantetBonuses != null)
            {
                itemToUpdate.EnchanmentsBonuses = manager.GetList<Bonus>(data.EnchantetBonuses);
            }
            if (data.EnchantetSpecialAbilities != null)
            {
                itemToUpdate.EnchanmentAbilities = manager.GetList<SpecialAbility>(data.EnchantetSpecialAbilities);
            }
            if (data.RequiresSpecialAbilities != null)
            {
                foreach (var newAbiReq in data.RequiresSpecialAbilities)
                {
                    if (itemToUpdate.RequiresAbility != null && itemToUpdate.RequiresAbility.All(x => x.SpecialAbilityGuid != newAbiReq))
                    {
                        itemToUpdate.RequiresAbility.Add(new ItemProficiency
                        {
                            SpecialAbilityGuid = newAbiReq
                        });
                    }
                }
                //TODO delete requirements if not in list
            }

            if (data.SpecialMaterial != null)
            {
                itemToUpdate.Material = manager.Get<ItemMaterial>(data.SpecialMaterial.Value);
            }
            itemToUpdate.AC = data.SelectedItem.AC;
            itemToUpdate.ArcaneSpellFailure = data.SelectedItem.ArcaneSpellFailure;
            itemToUpdate.ArmorCheckPenelty = data.SelectedItem.ArmorCheckPenelty;
            itemToUpdate.AttackBonus = data.SelectedItem.AttackBonus;
            itemToUpdate.CanAttack = data.SelectedItem.CanAttack;
            itemToUpdate.CritRange = data.SelectedItem.CritRange;
            itemToUpdate.Description = data.SelectedItem.Description;
            itemToUpdate.EnchanmentBonus = data.SelectedItem.EnchanmentBonus;
            itemToUpdate.IsOneAndAHalfHanded = data.SelectedItem.IsOneAndAHalfHanded;
            itemToUpdate.IsLightWeapon = data.SelectedItem.IsLightWeapon;
            itemToUpdate.IsRanged = data.SelectedItem.IsRanged;
            itemToUpdate.Masterworked = data.SelectedItem.Masterworked;
            itemToUpdate.IsOneAndAHalfHanded = data.SelectedItem.IsOneAndAHalfHanded;
            itemToUpdate.IsRanged = data.SelectedItem.IsRanged;
            itemToUpdate.UseItemsOwnAbilistyScore = data.SelectedItem.UseItemsOwnAbilistyScore;
            itemToUpdate.Masterworked = data.SelectedItem.Masterworked;
            itemToUpdate.MaxDexBonus = data.SelectedItem.MaxSpeed;
            itemToUpdate.MaxSpeed = data.SelectedItem.MaxSpeed;
            itemToUpdate.Price = data.SelectedItem.Price;
            itemToUpdate.RangeIncrement = data.SelectedItem.RangeIncrement;
            itemToUpdate.Type = data.SelectedItem.Type;
            itemToUpdate.Weight = data.SelectedItem.Weight;
            itemToUpdate.CriticalMultiplier = data.SelectedItem.CriticalMultiplier;
            itemToUpdate.Name = data.SelectedItem.Name;

            manager.CreateOrUpdate(itemToUpdate);
            return RedirectToAction("Index", "Item", new { id = itemToUpdate.ID });
            //return View("Creator/Item", "CreatorLayoutPage", GetItem(itemToUpdate.ID));
        }

    }
}
