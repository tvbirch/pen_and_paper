using System.Collections;
using System.Collections.Generic;
using RPG.Models.CharacterModal;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.Rounds;


namespace RPG.Models.RulebookModal.BaseTypes.Bonuses
{
    public class GetBonusDto
    {
        public List<AbilityScore> Abilities { get; set; }
        public List<ClassLevel> Classes { get; set; }
        public List<BonusRef> ActiveBonus { get; set; }
        public List<BonusRef> PassiveBonus { get; set; }
        public List<OwnedItem> EquippedItems { get; set; }
        public List<OwnedItem> AllItems { get; set; }
        public List<SpecialAbility> Feats { get; set; }

        public List<BonusRef> Bonuses
        {
            get
            {
                var result = new List<BonusRef>(ActiveBonus);
                result.AddRange(PassiveBonus);
                return result;
            }
        }

        public Round Round { get; set; }
        public Character Character { get; set; }
    }
}
