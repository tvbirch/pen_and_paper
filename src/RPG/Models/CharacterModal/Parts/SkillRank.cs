using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;

namespace RPG.Models.CharacterModal.Parts
{
    public class SkillRank: GameId
    {
        public Skill Skill { get; set; }

        public int Ranks { get; set; }

        //[NotMapped]
        //private bool IsClassSkill { get; set; }
        //[NotMapped]
        //private MaxBonusList<Bonus> MiscModifiers { get; set; }

        //public void SetRanks(int ranks)
        //{
        //    Ranks = ranks;
        //}

        //public int GetSkillModifier(GetBonusDto bonusDto, List<Skill> skills)
        //{
        //    return 0;
        //    //var abiSkillMod = bonusDto.Abilities.First(x => x.ID == Skill.SkillModifier.ID).GetCurrentModifier(bonusDto);
        //    //return Ranks + abiSkillMod + MiscModifiers.Sum(x => x.GetBonus(bonusDto).GetValueOrDefault()) + GetSynergiBonuses(skills);
        //}

        //public int GetSynergiBonuses(List<SkillRank> skills)
        //{
        //    return 0;
        //    //return Skill.GetsSynergiFrom.Count(s => s.Ranks >= 5) * 2;
        //}

        //public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<Ability> abilities)
        //{
        //    //MiscModifiers.ChangeTime(currentSpecialAbilities, unit);
        //    return false;
        //}

        //public void SetToClassSkill()
        //{
        //    IsClassSkill = true;
        //}

        ///*public void AddBonues(List<Bonus> bonues)
        //{
        //    foreach (var bonus in bonues)
        //    {
        //        if (bonus.ShouldApplyTo(this))
        //        {
        //            MiscModifiers.Add(bonus);
        //        }
        //    }
        //}

        //public void RemoveBonues(List<Bonus> bonues)
        //{
        //    if (bonues == null)
        //    {
        //        return;
        //    }
        //    foreach (var bonus in bonues)
        //    {
        //        MiscModifiers.RemoveAll(x => x.ID == bonus.ID);
        //    }
        //}*/


        //public string GetAbilityString()
        //{
        //    return Skill.SkillModifier.Name + (Skill.ArmorCheckPeneltyApply ? "*" : string.Empty);
        //}
        ///*
        //public int GetMiscModifier(GetBonusDto bonusDto, List<Skill> skills)
        //{
        //    return MiscModifiers.GetMaxValue(bonusDto) + GetSynergiBonuses(skills);
        //}*/
    }
}