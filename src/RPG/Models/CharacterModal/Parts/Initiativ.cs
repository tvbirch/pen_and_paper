using System.Collections.Generic;
using System.Linq;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;


namespace RPG.Models.CharacterModal.Parts
{
    public static class Initiativ
    {
        public static int GetInitiativBonus(GetBonusDto bonusDto)
        {
            var dex = bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiDexId).GetCurrentModifier(bonusDto);
            var bonus = bonusDto.Bonuses.Where(x => x.ShouldApplyTo(new GameId {ID = Configuration.InitiativId}, typeof (Initiativ))
                 && x.ShouldApplyToSubType(null)).MaxBonuesSum(bonusDto);
            return dex + bonus;
        }
    }
}
