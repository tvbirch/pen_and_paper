using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Dice;

namespace RPG.Controllers.Creator
{
    public class BonusController : ControllerBase
    {
        //
        // GET: /Bonus/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/Bonus", "CreatorLayoutPage", GetBonus(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                Context.Delete<Bonus>(id.Value);
            }
            return View("Creator/Bonus", "CreatorLayoutPage", GetBonus(null));
        }

        private BonusDataDto GetBonus(Guid? id)
        {
            return new BonusDataDto(Context, id);
        }

        public ActionResult Save(BonusDataDto data)
        {
            Bonus itemToAddOrUpdate;
            if (data.SelectedItem.ID == Guid.Empty)
            {
                itemToAddOrUpdate = data.SelectedItem;
            }
            else
            {
                itemToAddOrUpdate = Context.Get<Bonus>(data.SelectedItem.ID);
            }
            itemToAddOrUpdate.Name = data.SelectedItem.Name;
            itemToAddOrUpdate.Description = data.SelectedItem.Description;
            itemToAddOrUpdate.Type = data.SelectedItem.Type;
            itemToAddOrUpdate.ParentAbility = data.SelectedItem.ParentAbility;
            itemToAddOrUpdate.ApplyTo = data.SelectedItem.ApplyTo;
            itemToAddOrUpdate.CanTakeAsEnchantment = data.SelectedItem.CanTakeAsEnchantment;
            if (itemToAddOrUpdate.BonusValue == null)
            {
                itemToAddOrUpdate.BonusValue = new BonusToAdd();    
            }
            
            itemToAddOrUpdate.BonusValue.Dice= DiceRoll.Parse(data.AmountDice);
            itemToAddOrUpdate.BonusValue.FixedValue = data.SelectedItem.BonusValue.FixedValue;

            if (data.BonusToAddAbility.HasValue)
            {
                itemToAddOrUpdate.BonusValue.AbilityModifyer = Context.Get<Ability>(data.BonusToAddAbility.Value);
            }
            if (data.NewBonusToAddClassAtLevelClass != null)
            {
                if (itemToAddOrUpdate.BonusValue.ClassProgression == null)
                {
                    itemToAddOrUpdate.BonusValue.ClassProgression = new List<BonusToAddClassProgression>();
                }
                foreach (var progression in itemToAddOrUpdate.BonusValue.ClassProgression)
                {
                    foreach (var dataProgression in data.BonusToAddClassAtLevel)
                    {
                        if (progression.ID == dataProgression.Id)
                        {
                            progression.AtLevel = dataProgression.Level;
                        }
                    }    
                }
                if (data.NewBonusToAddClassAtLevelClass != null)
                {
                    itemToAddOrUpdate.BonusValue.ClassProgression.Add(new BonusToAddClassProgression
                    {
                        AtLevel = data.NewBonusToAddClassAtLevelClassLevel.GetValueOrDefault(),
                        Bonues = DiceRoll.Parse(data.NewBonusToAddClassAtLevelClassBonues),
                        ClassProgression = Context.Get<ClassBase>(data.NewBonusToAddClassAtLevelClass.Value)
                    });
                }
            }

            Context.CreateOrUpdate(itemToAddOrUpdate);

            return RedirectToAction("Index", "Bonus", new {id = itemToAddOrUpdate.ID});
        }

        public ActionResult GetItemsFromBonusApplyToType(BonusApplyToType? type)
        {
            if (!type.HasValue)
            {
                return Json(new { data = new List<ElementId>() }, JsonRequestBehavior.AllowGet);
                
            }

            var data = Context.GetItemsFromBonusApplyToType(type.Value).Select(x => new ElementId
            {
                Description = x.Description,
                Name = x.Name,
                ID = x.ID,
            });

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubItemsFromBonusApplyToType(BonusApplyToType? type)
        {
            if (!type.HasValue)
            {
                return Json(new { data = new List<ElementId>() }, JsonRequestBehavior.AllowGet);
            }

            var data = Context.GetSubItemsFromBonusApplyToType(type.Value);

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
    }
}
