using System;
using System.Linq;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;


namespace RPG.Models.CharacterModal.Parts
{
    public static class CarryingCapacity
    {
        //TODO: add biped or quadped

        public static int GetLightLoad(GetBonusDto bonusDto)
        {
            var heavyLoad = GetHeavyLoad(bonusDto);
            var lightLoad = heavyLoad / 3;
            return lightLoad;
        }

        public static int GetMediumLoad(GetBonusDto bonusDto)
        {
            var heavyLoad = GetHeavyLoad(bonusDto);
            var mediumLoad = (int)((double)heavyLoad / 3.0 * 2.0);
            return mediumLoad;
        }

        public static int GetHeavyLoad(GetBonusDto bonusDto)
        {
            var str = bonusDto.Abilities.First(x => x.Ability.ID == Configuration.AbiStrId).GetCurrent(bonusDto).GetValueAsInt();
            var baseHeavyLoad = Math.Min(str, 10) * 10;

            if (str > 10)
            {
                baseHeavyLoad += Math.Min((str - 10), 2) * 15;
            }
            if (str > 12)
            {
                baseHeavyLoad += 20;
            }
            if (str > 13)
            {
                baseHeavyLoad += Math.Min((str - 13), 2) * 25;
            }
            if (str > 15)
            {
                baseHeavyLoad += Math.Min((str - 13), 2) * 30;
            }
            if (str > 17)
            {
                baseHeavyLoad += 40;
            }
            if (str > 18)
            {
                baseHeavyLoad += Math.Min((str - 18), 2) * 50;
            }
            if (str > 20)
            {
                baseHeavyLoad += Math.Min((str - 20), 2) * 60;
            }
            if (str > 22)
            {
                baseHeavyLoad += 80;
            }
            if (str > 23)
            {
                baseHeavyLoad += Math.Min((str - 23), 2) * 100;
            }
            if (str > 25)
            {
                baseHeavyLoad += Math.Min((str - 25), 2) * 120;
            }
            if (str > 27)
            {
                baseHeavyLoad += 160;
            }
            if (str > 29)
            {
                baseHeavyLoad += 200;
            }
            if (str > 30)
            {
                throw new NotImplementedException("Not supported carrying capacity");
            }
            return (int)((double)baseHeavyLoad * bonusDto.Character.Race.GetBipedCarryingCapacityMulitplier(bonusDto));
        }

        public static double GetCurrentLoad(GetBonusDto bonusDto)
        {
            return bonusDto.AllItems.Sum(x => x.GetWeight(bonusDto));
        }
    }
}
