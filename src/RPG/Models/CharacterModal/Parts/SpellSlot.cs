using System.Collections.Generic;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.CharacterModal.Parts
{
    public class SpellSlot : GameId
    {
        public bool Used { get; set; }
        public Spell Spell { get; set; }
        public ClassBase SpellChosenForClass { get; set; }
        
        public string GetRange(int casterLevel)
        {
            switch (Spell.Range)
            {
                case SpellBaseRange.RangeExpressedInFeet:
                    return Spell.FixedRangeExpresedInFeet.GetValueOrDefault() + "ft";
                case SpellBaseRange.Close:
                    return (25 + (casterLevel / 2 * 5)) + "ft";
                case SpellBaseRange.Long:
                    return (100 + casterLevel * 10) + "ft";
                case SpellBaseRange.Medium:
                    return (400 + casterLevel * 40) + "ft";
                case SpellBaseRange.Personal:
                    return SpellBaseRange.Personal.ToString();
                case SpellBaseRange.Touch:
                    return SpellBaseRange.Touch.ToString();
                case SpellBaseRange.Unlimted:
                    return SpellBaseRange.Unlimted.ToString();
            }
            return "N/A";
        }

        public bool IsUsableInCombat()
        {
            return Spell.CastingTime <= CastingTime.FullRound;
        }
        public bool IsUsableInCurrentRound(List<RoundAction> actionsLeft)
        {
            if (!IsUsableInCombat() || actionsLeft == null) return false;
            foreach (var roundAction in actionsLeft)
            {
                if ((int)roundAction <= (int)Spell.CastingTime)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
