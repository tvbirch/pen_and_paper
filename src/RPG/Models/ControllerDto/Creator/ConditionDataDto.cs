using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Conditions;

namespace RPG.Models.ControllerDto.Creator
{
    public class ConditionDataDto : DataDto
    {
        public ConditionDataDto(){}

        public ConditionDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Condition>(selectedGuid.Value);
                BonusesSelected = SelectedItem.Bonuses.Select(x => x.ID).ToArray();
                if (SelectedItem.IfAlreadyActiveApplyCondition != null)
                {
                    IfAlreadyActiveApplyCondition = SelectedItem.IfAlreadyActiveApplyCondition.ID;
                }
            }
            Data = new List<ElementId>(manager.GetAll<Condition>().OrderBy(x => x.Name));
            Bonuses = new List<ElementId>(manager.GetAll<Bonus>().OrderBy(x => x.Name));
        }

        public Condition SelectedItem { get; set; }
        public List<ElementId> Bonuses { get; set; }


        public Guid[] BonusesSelected { get; set; }
        public Guid? IfAlreadyActiveApplyCondition { get; set; }
    }
}