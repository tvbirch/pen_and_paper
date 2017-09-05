using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;

namespace RPG.Models.ControllerDto.Creator
{
    public class SpellDataDto : DataDto
    {
        public SpellDataDto(){}

        public SpellDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Spell>(selectedGuid.Value);
                Components = SelectedItem.ComponentRequirements != null ? SelectedItem.ComponentRequirements.Select(x => x.ID).ToArray() : null;
                SpellSchool = SelectedItem.School != null ? SelectedItem.School.ID : (Guid?)null;
                SpellDescriptor = SelectedItem.Descriptor != null ? new List<Guid>(new[] { SelectedItem.Descriptor.ID }).ToArray() : null;
                SpellAbility = SelectedItem.SpellAbility != null ? SelectedItem.SpellAbility.ID : (Guid?)null;
                Save = SelectedItem.SpellSaveType != null ? SelectedItem.SpellSaveType.ID : (Guid?)null;
                DamageType = SelectedItem.Damage != null && SelectedItem.Damage.Damage != null ? SelectedItem.Damage.Damage.Type : (DamageType?)null;
                DamageString = SelectedItem.Damage != null && SelectedItem.Damage.Damage != null ? SelectedItem.Damage.Damage.Amount.ToString() : null;
            }
            Data = new List<ElementId>(manager.GetAll<Spell>().OrderBy(x => x.Name));
            AllComponents = manager.GetAll<SpellComponent>();
            AllSchools = manager.GetAll<SpellSchool>();
            AllDescriptors = manager.GetAll<SpellDescriptor>();
            AllSpecialAbilities = manager.GetAll<SpecialAbility>();
            AllSaves = manager.GetAll<SaveType>();
        }

        public Guid[] Components { get; set; }
        public Guid? SpellSchool { get; set; }
        public Guid[] SpellDescriptor { get; set; }
        public Guid? SpellAbility { get; set; }
        public Guid? Save { get; set; }
        public string DamageString { get; set; }
        public DamageType? DamageType { get; set; }

        public Spell SelectedItem { get; set; }
        public IEnumerable<ElementId> AllComponents { get; set; }
        public IEnumerable<ElementId> AllSchools { get; set; }
        public IEnumerable<ElementId> AllDescriptors { get; set; }
        public IEnumerable<ElementId> AllSpecialAbilities { get; set; }
        public IEnumerable<ElementId> AllSaves { get; set; }
        

        //public List<SpellRequiretLevel> CasterRequirements { get; set; }
    }
}