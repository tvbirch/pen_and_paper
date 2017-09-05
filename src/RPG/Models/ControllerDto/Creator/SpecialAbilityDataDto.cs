using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Conditions;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Models.ControllerDto.Creator
{
    public class SpecialAbilityDataDto : DataDto
    {
        public SpecialAbilityDataDto (){}

        public SpecialAbilityDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<SpecialAbility>(selectedGuid.Value);
                Bonuses = SelectedItem.Bonuses != null
                    ? SelectedItem.Bonuses.OrderBy(x => x.Name).Select(x => x.ID).ToArray()
                    : null;

                if (SelectedItem != null && SelectedItem.RequiresSpecialAbilityActive != null)
                {
                    RequiredAbility = SelectedItem.RequiresSpecialAbilityActive;
                }

                if (SelectedItem.ApplyConditionOnActivate != null)
                {
                    ConditionOnActivate = SelectedItem.ApplyConditionOnActivate.ID;
                }
                if (SelectedItem.ApplyConditionOnDeactivate != null)
                {
                    ConditionOnDeactivate = SelectedItem.ApplyConditionOnDeactivate.ID;
                }
                if (SelectedItem.Limit != null)
                {
                    if (SelectedItem.Limit.Amount != null)
                    {
                        if (SelectedItem.Limit.Amount.Ability != null)
                        {
                            LimitAmountAbility = SelectedItem.Limit.Amount.Ability.ID;
                        }
                        if (SelectedItem.Limit.Amount.ClassProgression != null && SelectedItem.Limit.Amount.ClassProgression.Count > 0)
                        {
                            LimitAmountClass = SelectedItem.Limit.Amount.ClassProgression.First().ClassProgression.ID;
                            LimitAmountClassAtLevel =
                                SelectedItem.Limit.Amount.ClassProgression.Select(x => x.AtLevel)
                                    .OrderBy(x => x)
                                    .ToArray();
                        }

                        if (SelectedItem.Limit.Amount.TradeWith != null)
                        {
                            TradeWithId = SelectedItem.Limit.Amount.TradeWith.ID;
                        }
                    }
                    if (SelectedItem.Limit.Duration != null)
                    {
                        if (SelectedItem.Limit.Duration.DurationAbilityModifier != null)
                        {
                            DurationAmountAbility = SelectedItem.Limit.Duration.DurationAbilityModifier.ID;
                        }
                    }
                    if (SelectedItem.BonusFromCharges == null)
                    {
                        SelectedItem.BonusFromCharges = new List<BonusFromCharges>();
                    }
                    BonusFromCharges = new string[SelectedItem.BonusFromCharges.Count+1];
                    for (int i = 0; i < SelectedItem.BonusFromCharges.Count; i++)
                    {
                        BonusFromCharges[i] = SelectedItem.BonusFromCharges[i].Bonus.ToString();
                    }
                }
            }
            Data = new List<ElementId>(manager.GetAll<SpecialAbility>().OrderBy(x => x.Name));
            AllBonuses = manager.GetAll<Bonus>();
            AbilityOptions = manager.GetAll<Ability>();
            Classes = manager.GetAll<ClassBase>();
            Conditions = manager.GetAll<Condition>();
        }
        public SpecialAbility SelectedItem { get; set; }
        
        public Guid[] Bonuses { get; set; }


        public Guid? RequiredAbility { get; set; }

        public Guid? LimitAmountAbility { get; set; }
        public Guid? LimitAmountClass { get; set; }
        public int[] LimitAmountClassAtLevel { get; set; }
        public Guid? DurationAmountAbility { get; set; }
        public string[] BonusFromCharges { get; set; }

        public Guid? ConditionOnActivate { get; set; }
        public Guid? ConditionOnDeactivate { get; set; }

        public IEnumerable<ElementId> AllBonuses { get; set; }
        public IEnumerable<ElementId> AbilityOptions { get; set; }
        public IEnumerable<ElementId> Classes { get; set; }
        public IEnumerable<ElementId> Conditions { get; set; }
        public Guid? TradeWithId { get; set; }

        public Dictionary<int?, string> ClassLevels = new Dictionary<int?, string>
                                        {
                                            { 1, "1" },
                                            { 2, "2" },
                                            { 3, "3" },
                                            { 4, "4" },
                                            { 5, "5" },
                                            { 6, "6" },
                                            { 7, "7" },
                                            { 8, "8" },
                                            { 9, "9" },
                                            { 10, "10" },
                                            { 11, "11" },
                                            { 12, "12" },
                                            { 13, "13" },
                                            { 14, "14" },
                                            { 15, "15" },
                                            { 16, "16" },
                                            { 17, "17" },
                                            { 18, "18" },
                                            { 19, "19" },
                                            { 20, "20" }
                                        };
    }
}