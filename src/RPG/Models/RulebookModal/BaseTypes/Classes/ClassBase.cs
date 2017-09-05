using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Alligments;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Lists;

namespace RPG.Models.RulebookModal.BaseTypes.Classes
{
    public class ClassBase : ElementId
    {
        public List<Skill> ClassSkills { get; set; }
        public List<AbilityByClassLevel> ClassAbilities { get; set; }
        public List<SaveRate> SaveBonusRate { get; set; }
        public BaseBonusRate BaseAttackBaseSaveBonus { get; set; }
        public int BaseSkillPointsAtEachLevel { get; set; }
        public DieType HitDie { get; set; }
        public List<Alligment> AllowedAlligments { get; set; }

        public bool ArcaneCaster { get; set; }
        public bool DivineCaster { get; set; }

        public Ability AbilityUsedForCasting { get; set; }
        public List<SpellKnown> SpellKnown { get; set; }
        public List<SpellPrDay> SpellPrDay { get; set; }
        [NotMapped]
        public Dictionary<int, Dictionary<int, int>> SpellKnownDictonary
        {
            get
            {
                var knownSpellsPrDay = new Dictionary<int, Dictionary<int, int>>();
                if (SpellPrDay != null)
                {
                    foreach (var prDay in SpellKnown)
                    {
                        knownSpellsPrDay.Add(prDay.Level, prDay.SpellsKnown.ToDictionary(x => x.SpellLevel, x => x.NumberOfSpells));
                    }
                }
                for (int i = 1; i < 21; i++)
                {
                    if (knownSpellsPrDay.ContainsKey(i))
                    {
                        continue;
                    }
                    knownSpellsPrDay.Add(i, new Dictionary<int, int>());
                }
                foreach (var spellsPrDay in knownSpellsPrDay)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (spellsPrDay.Value.Any(x => x.Key == i))
                        {
                            continue;
                        }
                        spellsPrDay.Value.Add(i, 0);
                    }
                }
                return knownSpellsPrDay;
            }
        }
        [NotMapped]
        public Dictionary<int, Dictionary<int, int>> SpellPrDayDictonary
        {
            get
            {
                var knownSpellsPrDay = new Dictionary<int, Dictionary<int, int>>();
                if (SpellPrDay != null)
                {
                    foreach (var prDay in SpellPrDay)
                    {
                        knownSpellsPrDay.Add(prDay.Level, prDay.NumberOfSpells.ToDictionary(x => x.SpellLevel, x => x.SpellCastable));
                    }
                }
                for (int i = 1; i < 21; i++)
                {
                    if (knownSpellsPrDay.ContainsKey(i))
                    {
                        continue;
                    }
                    knownSpellsPrDay.Add(i, new Dictionary<int, int>());
                }
                foreach (var spellsPrDay in knownSpellsPrDay)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (spellsPrDay.Value.Any(x => x.Key == i))
                        {
                            continue;
                        }
                        spellsPrDay.Value.Add(i, 0);
                    }
                }
                return knownSpellsPrDay;
            }
        }


        public virtual ICollection<Race> FavoredClassBy { get; set; }
        public virtual ICollection<ClassLevel> ClassLevels { get; set; }

        public List<SpecialAbility> GetAbilitiesAtCurrentLevel(int level)
        {
            return ClassAbilities.Where(x => x.AvailableAtLevel <= level).Select(y => y.Ability).ToList();
        }

        public bool IsCaster()
        {
            return SpellPrDay != null && SpellPrDay.Any(x => x.NumberOfSpells.Any(y => y.SpellCastable > 0));
        }
    }
}
