using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Models.ControllerDto.Creator
{
    public class ClassAbilityByClassLevelDataDto : DataDto
    {
        public ClassAbilityByClassLevelDataDto(){}

        public ClassAbilityByClassLevelDataDto(ContextManager manager, Guid? selectedGuid, Guid? abilityByLevel)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<ClassBase>(selectedGuid.Value);
            }
            if (abilityByLevel.HasValue && abilityByLevel != Guid.Empty)
            {
                SelectedAbilityByClassLevel = manager.Get<AbilityByClassLevel>(abilityByLevel.Value);
                Ability = SelectedAbilityByClassLevel == null || SelectedAbilityByClassLevel.Ability == null ? (Guid?) null: SelectedAbilityByClassLevel.Ability.ID;
            }
            Data = new List<ElementId>(manager.GetAll<ClassBase>().OrderBy(x => x.Name));
            AllAbilities = new List<ElementId>(manager.GetAll<SpecialAbility>().OrderBy(x => x.Name));
            CurrentAbilities = manager.GetQueryable<AbilityByClassLevel>().Where(x => x.Class.ID == selectedGuid).Select(x => new ElementId
            {
                ID = x.ID,
                Name = "Lvl: " + x.AvailableAtLevel + " - " + x.Ability.Name
            }).OrderBy(x => x.Name).ToList();
        }

        public IEnumerable<ElementId> AllAbilities { get; set; }
        public IEnumerable<ElementId> CurrentAbilities { get; set; }

        public ClassBase SelectedItem { get; set; }
        public AbilityByClassLevel SelectedAbilityByClassLevel { get; set; }
        public Guid? Ability { get; set; }
    }
}