using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using RPG.Models.CharacterModal.HelperDtos;
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
    public static class Grapple
    {
        //public MaxBonusList<Bonus> Bonuses { get; private set; }
        //private AbilityScore _str;
        //private Size _size;
        //private Classes _classes;


        public static CalculatedString GetGrappleBonus(GetBonusDto bonusDto)
        {
            var calcStr = new CalculatedString();


            var str = bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiStrId).GetCurrentModifier(bonusDto);
            var size = bonusDto.Character.Race.GetSpecialAttackModifier(bonusDto);
            var baseattack = bonusDto.Character.GetBaseAttack().First();
            var bonus = bonusDto.Bonuses.Where(x => x.ShouldApplyTo(new GameId { ID = Configuration.GrappleId }, typeof(Grapple)) 
                && x.ShouldApplyToSubType(null)).MaxBonusRefList(bonusDto);

            //return str + size + baseattack + bonus;
            calcStr.AddIfNotZero("Strength", null, str);
            calcStr.AddIfNotZero("Size", null, size);
            calcStr.AddIfNotZero("Base attack", null, baseattack);
            calcStr.AddPartsByRef(bonus,bonusDto);
            return calcStr;
        }


    }
}
