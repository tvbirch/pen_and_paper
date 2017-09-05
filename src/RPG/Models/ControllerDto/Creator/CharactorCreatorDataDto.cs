using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.Context;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Alligments;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;

namespace RPG.Models.ControllerDto.Creator
{
    public class CharactorCreatorDataDto : DataDto
    {
        public CharactorCreatorDataDto(){}

        public CharactorCreatorDataDto(ContextManager manager, Guid? selectedGuid)
        {
            if (selectedGuid.HasValue)
            {
                SelectedItem = manager.Get<Character>(selectedGuid.Value);
                Race = SelectedItem.Race != null ? SelectedItem.Race.ID : (Guid?) null;
                AbilityScores = SelectedItem.Abilities.ToArray();
                RemaningClasses = manager.GetAll<ClassBase>().Except(SelectedItem.CurrentClasses.Select(x=> x.Class));
                LearnedLanguages = SelectedItem.Languages.Select(x => x.ID).ToArray();
                LearnedFeats = SelectedItem.Feats.Select(x => x.ID).ToArray();
                var currentSkillRanks = new List<SkillRank>(SelectedItem.SkillRanks);

                var localAllSkillRanks = manager.GetAll<Skill>().Select(x => new SkillRank
                {
                    Ranks = 0,
                    Skill = x,
                }).ToList();

                var toAdd = localAllSkillRanks.Where(x => !currentSkillRanks.Exists(y => y.Skill.ID == x.Skill.ID)).ToList();
                currentSkillRanks.AddRange(toAdd);

                SelectedItem.SkillRanks = currentSkillRanks.OrderBy(x => x.Skill.Name).ToList();
            }
            else
            {
                AbilityScores = manager.GetAll<Ability>().Select(x => new AbilityScore
                {
                    Ability = x,
                    BaseValue = 10,
                }).ToArray();
                RemaningClasses = manager.GetAll<ClassBase>();
                LearnedLanguages = new Guid[0];
            }
            Data = new List<ElementId>(manager.GetAll<Character>().OrderBy(x => x.Name));
            AllAlligments = new List<ElementId>(manager.GetAll<Alligment>());

            AllRaces = new List<ElementId>(manager.GetAll<Race>()); 
            AllLanguages = manager.GetAll<Language>();
            AllFeats = manager.GetQueryable<SpecialAbility>().Where(x => x.CanTakeAsFeat).ToList();

        }

        public Character SelectedItem { get; set; }
        public AbilityScore[] AbilityScores { get; set; }

        public IEnumerable<ElementId> AllAlligments { get; set; }
        public IEnumerable<ElementId> AllRaces { get; set; }
        public IEnumerable<ElementId> RemaningClasses { get; set; }
        public IEnumerable<ElementId> AllLanguages { get; set; }
        public IEnumerable<ElementId> AllFeats { get; set; }

        public Guid[] LearnedLanguages { get; set; }
        public Guid[] LearnedFeats { get; set; }
        public Guid? Race { get; set; }
        public Guid? CurrentClassToAdd { get; set; }
    }
}