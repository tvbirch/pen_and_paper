using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;

namespace RPG.Models.CoreModal.DTO
{
    public class BonusRef
    {
        public ElementId Parent { get; set; }
        public Bonus Bonues { get; set; }

        public BonusType Type
        {
            get { return Bonues.Type; }
        }

        public BonusRef(ElementId parent, Bonus bonues)
        {
            Parent = parent;
            Bonues = bonues;
        }

        public bool ShouldApplyTo(GameId idAble, Type overrideType = null)
        {
            return Bonues.ShouldApplyTo(idAble, overrideType);
        }

        public bool ShouldApplyToSubType(Guid? subtype)
        {
            return Bonues.ShouldApplyToSubType(subtype);
        }

        public DiceRoll GetBonus(GetBonusDto bonusDto)
        {
            return Bonues.GetBonus(bonusDto);
        }

        public bool IsActive(GetBonusDto bonus)
        {
            return Bonues.IsActive(bonus);
        }

        public bool IsOfType(DamageType type)
        {
            return Bonues.IsOfType(type);
        }

        public string GetCondition()
        {
            return Bonues.GetCondition();
        }

        public override string ToString()
        {
            return Parent == null ? "N/A" : Parent.Name;
        }
    }
}