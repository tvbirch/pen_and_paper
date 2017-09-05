using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Models.ControllerDto.Creator
{
    public class RaceDataDto : DataDto
    {
        public RaceDataDto(){}

        public RaceDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Race>(selectedGuid.Value);
                KnownLanguages = SelectedItem.Languages != null
                    ? SelectedItem.Languages.OrderBy(x => x.Name).Select(x => x.ID).ToArray()
                    : null;
                BonusLanguages = SelectedItem.BonusLanguages != null
                    ? SelectedItem.BonusLanguages.OrderBy(x => x.Name).Select(x => x.ID).ToArray()
                    : null;
                FavoredClasses = SelectedItem.FavoredClasses != null
                    ? SelectedItem.FavoredClasses.OrderBy(x => x.Name).Select(x => x.ID).ToArray()
                    : null;
                RacialBonuses = SelectedItem.RaceBonuses != null
                    ? SelectedItem.RaceBonuses.OrderBy(x => x.Name).Select(x => x.ID).ToArray()
                    : null;
                RacialAbilities = SelectedItem.RacialAbilities != null
                    ? SelectedItem.RacialAbilities.OrderBy(x => x.Name).Select(x => x.ID).ToArray()
                    : null;
            }
            Data = new List<ElementId>(manager.GetAll<Race>().OrderBy(x => x.Name));
            AllLanguages = manager.GetAll<Language>();
            AllClasses = manager.GetAll<ClassBase>();
            AllBonuses = manager.GetAll<Bonus>();
            AllAbilities = manager.GetAll<SpecialAbility>();
        }

        public Race SelectedItem { get; set; }
        public IEnumerable<ElementId> AllLanguages { get; set; }
        public IEnumerable<ElementId> AllClasses { get; set; }
        public IEnumerable<ElementId> AllBonuses { get; set; }
        public IEnumerable<ElementId> AllAbilities { get; set; }
        public Guid[] KnownLanguages { get; set; }
        public Guid[] BonusLanguages { get; set; }
        public Guid[] FavoredClasses { get; set; }
        public Guid[] RacialBonuses { get; set; }
        public Guid[] RacialAbilities { get; set; }
    }
}