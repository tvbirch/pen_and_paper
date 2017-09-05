using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Damages;

namespace RPG.Models.RulebookModal.BaseTypes.Bonuses
{
    [ComplexType]
    public class BonusApplyTo
    {
        public bool ApplyToAll { get; set; }
        public Guid? ApplyToGuid { get; set; }
        public Guid? ApplyToSubtypeGuid { get; set; }
        public string ApplyToCondition { get; set; }

        [Required]
        public BonusApplyToType ApplyToType { get; set; }

    }

    public enum BonusApplyToType
    {
        Ability = 1,
        Ac = 2,
        Class = 5,
        DamageRecuction = 6,
        Grapple = 7,
        HitPoints = 8,
        Item = 9,
        Initiativ = 10,
        Race = 11,
        Round = 12,
        Save = 13,
        Skill = 15,
        SpecialAbility = 16,
        Spell = 17,
        SpellSchool = 18,
        SpellResistance = 19,
        BaseAttack = 20,
    }
}