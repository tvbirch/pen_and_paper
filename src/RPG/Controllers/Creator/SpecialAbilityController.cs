using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using RPG.Models.Context;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Conditions;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;

namespace RPG.Controllers.Creator
{
    public class SpecialAbilityController : ControllerBase
    {
        //
        // GET: /SpecialAbility/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/SpecialAbility", "CreatorLayoutPage", GetSpecialAbility(id));
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                Context.Delete<SpecialAbility>(id.Value);
            }
            return View("Creator/SpecialAbility", "CreatorLayoutPage", GetSpecialAbility(null));
        }

        private SpecialAbilityDataDto GetSpecialAbility(Guid? id)
        {
            return new SpecialAbilityDataDto(Context, id);
        }

        [HttpPost]
        public ActionResult Save(SpecialAbilityDataDto data)
        {
            SpecialAbility abiToCreateOrUpdate;
            if (data.SelectedItem.ID == Guid.Empty)
            {
                abiToCreateOrUpdate = data.SelectedItem;
            }
            else
            {
                abiToCreateOrUpdate = Context.Get<SpecialAbility>(data.SelectedItem.ID);
            }

            if (data.ConditionOnActivate.HasValue)
            {
                abiToCreateOrUpdate.ApplyConditionOnActivate = Context.Get<Condition>(data.ConditionOnActivate.Value);
            }
            else
            {
                abiToCreateOrUpdate.ApplyConditionOnActivate = null;
            }
            if (data.ConditionOnDeactivate.HasValue)
            {
                abiToCreateOrUpdate.ApplyConditionOnDeactivate = Context.Get<Condition>(data.ConditionOnDeactivate.Value);
            }
            else
            {
                abiToCreateOrUpdate.ApplyConditionOnDeactivate = null;
            }

            if (data.Bonuses != null)
            {
                abiToCreateOrUpdate.Bonuses = Context.GetList<Bonus>(data.Bonuses);
            }
            else
            {
                abiToCreateOrUpdate.Bonuses = null;
            }

            if (data.LimitAmountAbility.HasValue)
            {
                abiToCreateOrUpdate.Limit.Amount.Ability = Context.Get<Ability>(data.LimitAmountAbility.Value);
            }
            if (data.LimitAmountClass.HasValue && data.LimitAmountClassAtLevel != null)
            {
                abiToCreateOrUpdate.Limit.Amount.ClassProgression = new List<UsableAmountClassProgression>();
                var classUsedForProgress = Context.Get<ClassBase>(data.LimitAmountClass.Value);
                foreach (var lvl in data.LimitAmountClassAtLevel)
                {
                    abiToCreateOrUpdate.Limit.Amount.ClassProgression.Add(new UsableAmountClassProgression
                    {
                        AtLevel = lvl,
                        ClassProgression = classUsedForProgress
                    });
                }
            }
            if (data.DurationAmountAbility.HasValue)
            {
                abiToCreateOrUpdate.Limit.Duration.DurationAbilityModifier = Context.Get<Ability>(data.DurationAmountAbility.Value);
            }

            abiToCreateOrUpdate.Name = data.SelectedItem.Name;
            abiToCreateOrUpdate.Description = data.SelectedItem.Description;
            abiToCreateOrUpdate.CanTakeAsFeat = data.SelectedItem.CanTakeAsFeat;
            abiToCreateOrUpdate.CanTakeAsEnchantment = data.SelectedItem.CanTakeAsEnchantment;
            abiToCreateOrUpdate.RequiredNumberOfCharges = data.SelectedItem.RequiredNumberOfCharges;
            abiToCreateOrUpdate.RequiresSpecialAbilityActive = data.RequiredAbility;

            if (data.SelectedItem.Limit != null)
            {
                if (abiToCreateOrUpdate.Limit == null)
                {
                    abiToCreateOrUpdate.Limit = new UsableLimit();
                    abiToCreateOrUpdate.Limit.Amount = new UsableAmount();
                    abiToCreateOrUpdate.Limit.Duration = new Duration();
                }

                abiToCreateOrUpdate.Limit.Amount.ProvokesAttackOfOppertunity = data.SelectedItem.Limit.Amount.ProvokesAttackOfOppertunity;
                abiToCreateOrUpdate.Limit.Amount.ResetUseTimeLimtUnit = data.SelectedItem.Limit.Amount.ResetUseTimeLimtUnit;
                abiToCreateOrUpdate.Limit.Amount.ActionRequired = data.SelectedItem.Limit.Amount.ActionRequired;
                abiToCreateOrUpdate.Limit.Amount.FixedAmount = data.SelectedItem.Limit.Amount.FixedAmount;
                abiToCreateOrUpdate.Limit.Amount.LimitAutoCharges = data.SelectedItem.Limit.Amount.LimitAutoCharges;
                abiToCreateOrUpdate.Limit.Amount.VariableLimitAutoCharges = data.SelectedItem.Limit.Amount.VariableLimitAutoCharges;

                abiToCreateOrUpdate.Limit.Duration.Amount = data.SelectedItem.Limit.Duration.Amount;
                abiToCreateOrUpdate.Limit.Duration.Unit = data.SelectedItem.Limit.Duration.Unit;
            }
            abiToCreateOrUpdate.Name = data.SelectedItem.Name;

            if (abiToCreateOrUpdate.BonusFromCharges == null)
            {
                abiToCreateOrUpdate.BonusFromCharges = new List<BonusFromCharges>();
            }
            var nrOfChargesObjects = data.BonusFromCharges == null ? 0 : data.BonusFromCharges.Count(x => !x.IsNullOrWhiteSpace());
            var nrOfCurrentChargesObjects = abiToCreateOrUpdate.BonusFromCharges.Count;
            for (int i = 0; i < nrOfChargesObjects-nrOfCurrentChargesObjects; i++)
            {
                abiToCreateOrUpdate.BonusFromCharges.Add(new BonusFromCharges());
            }
            for (int i = 0; i < nrOfChargesObjects; i++)
            {
                abiToCreateOrUpdate.BonusFromCharges[i].NumberOfChargesForBonus = i + 1;
                var strAsDiceRoll = DiceRoll.Parse(data.BonusFromCharges[i]);
                if (abiToCreateOrUpdate.BonusFromCharges[i].Bonus != null && abiToCreateOrUpdate.BonusFromCharges[i].Bonus.ToString() != strAsDiceRoll.ToString())
                {
                    Context.Delete<DiceRoll>(abiToCreateOrUpdate.BonusFromCharges[i].Bonus.ID);
                    abiToCreateOrUpdate.BonusFromCharges[i].Bonus = strAsDiceRoll;
                } 
                else if (abiToCreateOrUpdate.BonusFromCharges[i].Bonus == null)
                {
                    abiToCreateOrUpdate.BonusFromCharges[i].Bonus = strAsDiceRoll;
                }
            }
            for (int i = nrOfCurrentChargesObjects; i > nrOfChargesObjects; i--)
            {
                Context.Delete<BonusFromCharges>(abiToCreateOrUpdate.BonusFromCharges[i-1].ID);
            }


            if (data.SelectedItem.Limit != null)
            {
                abiToCreateOrUpdate.Limit.Amount.TradeMaxTrade = data.SelectedItem.Limit.Amount.TradeMaxTrade;
                abiToCreateOrUpdate.Limit.Amount.TradeMultiplyer = data.SelectedItem.Limit.Amount.TradeMultiplyer;
                abiToCreateOrUpdate.Limit.Amount.TradeDoubleIfThw = data.SelectedItem.Limit.Amount.TradeDoubleIfThw;
                if (data.TradeWithId.HasValue)
                {
                    abiToCreateOrUpdate.Limit.Amount.TradeWith = Context.Get<Bonus>(data.TradeWithId.Value);
                }
            }


            Context.CreateOrUpdate(abiToCreateOrUpdate);
            return RedirectToAction("Index", "SpecialAbility", new { id = abiToCreateOrUpdate.ID });
            //return View("Creator/SpecialAbility", "CreatorLayoutPage", GetSpecialAbility(abiToCreateOrUpdate.ID));
        }
    }
}
