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
using RPG.Models.RulebookModal.BaseTypes.Languages;

namespace RPG.Models.ControllerDto.Creator
{
    public class BonusDataDto : DataDto
    {
        public BonusDataDto(){}

        public BonusDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Bonus>(selectedGuid.Value);
                if (SelectedItem.BonusValue != null && SelectedItem.BonusValue.AbilityModifyer != null)
                {
                    BonusToAddAbility = SelectedItem.BonusValue.AbilityModifyer.ID;
                }
                if (SelectedItem.BonusValue != null && SelectedItem.BonusValue.ClassProgression != null && 0 < SelectedItem.BonusValue.ClassProgression.Count)
                {
                    BonusToAddClassAtLevel = SelectedItem.BonusValue.ClassProgression.Select(x => new DiceBonuesAtLevel
                    {
                        Id = x.ID,
                        Bonues = x.Bonues.ToString(),
                        Class = x.ClassProgression.ID,
                        ClassName = x.ClassProgression.Name,
                        Level = x.AtLevel
                    }).ToArray();
                }
                if (SelectedItem.BonusValue != null && SelectedItem.BonusValue.Dice != null )
                {
                    AmountDice = SelectedItem.BonusValue.Dice.ToString();
                }
            }
            Data = new List<ElementId>(manager.GetAll<Bonus>().OrderBy(x => x.Name));

            AbilityOptions = manager.GetAll<Ability>();

            Classes = manager.GetAll<ClassBase>().OrderBy(x => x.Name).ToList(); 

            //initializing needed fields


        }
        //Helper fields
        public Guid? BonusToAddAbility { get; set; }

        public DiceBonuesAtLevel[] BonusToAddClassAtLevel { get; set; }
        public Guid? NewBonusToAddClassAtLevelClass { get; set; }
        public int? NewBonusToAddClassAtLevelClassLevel { get; set; }
        public string NewBonusToAddClassAtLevelClassBonues { get; set; }


        public string AmountDice { get; set; }

        //Data
        public Bonus SelectedItem { get; set; }
        public IEnumerable<Ability> AbilityOptions { get; set; }
        public IEnumerable<ClassBase> Classes { get; set; }
        
    }

    public class DiceBonuesAtLevel
    {
        public Guid? Id { get; set; }
        public Guid Class { get; set; }
        public string ClassName { get; set; }
        public int Level { get; set; }
        public string Bonues { get; set; }
    }
}