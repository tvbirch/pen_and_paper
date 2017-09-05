using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Lists;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class Spell : ElementId
    {
        public List<SpellRequiretLevel> CasterRequirements { get; set; }
        public List<SpellComponent> ComponentRequirements { get; set; }

        public SpellSchool School { get; set; }
        public SpellDescriptor Descriptor { get; set; }
        public CastingTime CastingTime { get; set; }
        public SpellBaseRange Range { get; set; }
        public int? FixedRangeExpresedInFeet { get; set; } //Used only for SpellBaseRange RangeExpressedInFeet
        public SpecialAbility SpellAbility { get; set; }
        public string Target { get; set; }
        public SpellDamage Damage { get; set; }
        public SaveType SpellSaveType { get; set; }
        public bool SpellResistance { get; set; }
        
        public string GetComponentsString()
        {
            var builder = new StringBuilder();
            if (ComponentRequirements != null)
            {
                foreach (var componentRequirement in ComponentRequirements)
                {
                    builder.AppendFormat("{0}, ", componentRequirement.Name[0]);
                }
                if (builder.Length > 0)
                {
                    builder.Length = builder.Length - 2;
                }
            }
            return builder.ToString();
        }
        
        public string GetTarget()
        {
            return Target;
        }

        public string GetSave()
        {
            return SpellSaveType == null ? "None" : SpellSaveType.Name;
        }

        public string GetSpellResistanceString()
        {
            return SpellResistance ? "Yes" : "No";
        }

        public string GetRangeString()
        {
            return FixedRangeExpresedInFeet != null ? string.Format("{0}ft", FixedRangeExpresedInFeet) : Range.ToString();
        }
    }
}
