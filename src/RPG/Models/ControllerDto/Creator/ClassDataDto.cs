using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Alligments;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using WebGrease.Css.Extensions;

namespace RPG.Models.ControllerDto.Creator
{
    public class ClassDataDto : DataDto
    {
        public ClassDataDto(){}

        public ClassDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<ClassBase>(selectedGuid.Value);
                ClassSkills = SelectedItem.ClassSkills == null ? null : SelectedItem.ClassSkills.Select(x => x.ID).ToArray();
                AllowedAllignments = SelectedItem.AllowedAlligments == null ? null : SelectedItem.AllowedAlligments.Select(x => x.ID).ToArray();
                
                SpellsPrDay = new int[SelectedItem.SpellPrDayDictonary.Count][];
                foreach (var spd in SelectedItem.SpellPrDayDictonary.OrderBy(x => x.Key))
                {
                    SpellsPrDay[spd.Key-1] = spd.Value.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
                }

                KnownSpells = new int[SelectedItem.SpellKnownDictonary.Count][];
                foreach (var skd in SelectedItem.SpellKnownDictonary.OrderBy(x => x.Key))
                {
                    KnownSpells[skd.Key-1] = skd.Value.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
                } 
            }
            Data = new List<ElementId>(manager.GetAll<ClassBase>().OrderBy(x => x.Name));
            AllSkills = new List<ElementId>(manager.GetAll<Skill>().OrderBy(x => x.Name));
            AllAlligments = new List<ElementId>(manager.GetAll<Alligment>().OrderBy(x => x.Name));
            AllAbilities = new List<OrderedElementId>(manager.GetAll<Ability>().OrderBy(x => x.Index));
        }
        public IEnumerable<ElementId> AllSkills { get; set; }
        public IEnumerable<ElementId> AllAlligments { get; set; }
        public IEnumerable<ElementId> AllAbilities { get; set; }
        public int[][] SpellsPrDay { get; set; }
        public int[][] KnownSpells { get; set; }

        public ClassBase SelectedItem { get; set; }
        public Guid[] ClassSkills { get; set; }
        public Guid[] AllowedAllignments { get; set; }
        public Guid? AbilityUsedForCasting { get; set; }
    }
}