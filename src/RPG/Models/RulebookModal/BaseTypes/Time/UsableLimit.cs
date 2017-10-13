using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Rounds;


namespace RPG.Models.RulebookModal.BaseTypes.Time
{
    public class UsableLimit : GameId
    {
        public UsableAmount Amount { get; set; }
        public Duration Duration { get; set; }

        public bool UseChagesIfPossible(int charges, GetBonusDto bonues, Guid parentAbilityId)
        {
            if (Amount.IsChargesPossible(charges, bonues, parentAbilityId))
            {
                Amount.Use(charges, bonues, parentAbilityId);
                //SetIsActive(bonues.Abilities);
                return true;
            }
            return false;
        }

        //private void SetIsActive(List<AbilityScore> abilities)
        //{
        //    IsActive = Duration == null || Duration.GetNumberOfRounds(abilities) > 0;
        //}

        public bool IsChargesPossible(int charge, GetBonusDto bonues, Guid parentAbilityId)
        {
            return Amount == null || Amount.IsChargesPossible(charge, bonues,parentAbilityId);
        }
        
        public string ToString(GetBonusDto bonus, Guid id)
        {
            return Amount.GetAmount(bonus,id) + "/" + Amount.ResetUseTimeLimtUnit.ToString();
        }

        public bool ChangeTime(TimeLimitUnit unit, GetBonusDto bonues, Guid parentAbilityId)
        {
            if (GetIsActive(bonues, parentAbilityId))
            {
                var timeLeft = GetRoundsLeft(bonues, parentAbilityId);
                if (0 >= timeLeft)
                {
                    if (unit == TimeLimitUnit.Attack)
                    {
                        if (Duration != null && Duration.Unit == DurationUnit.Attack)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetRoundsLeft(GetBonusDto bonues, Guid parentAbilityId)
        {
            var abilityActiveFor = bonues.Round.ActivatedAbilities.Where(x => x.AbilityId == parentAbilityId && x.IsActive).Select(x => x.ActiveTime).SelectMany(x => x).ToList();
            var numberOfRoundsActive = 0;
            foreach (var currentAbilityActiveFor in abilityActiveFor)
            {
                switch (currentAbilityActiveFor.Time)
                {
                    case TimeLimitUnit.Attack:
                        return 0;
                        //break;
                    case TimeLimitUnit.Round:
                        numberOfRoundsActive++;
                        break;
                    case TimeLimitUnit.Encounter:
                        numberOfRoundsActive += 600;
                        break;
                    case TimeLimitUnit.Day:
                        numberOfRoundsActive += 10 * 60 * 24;
                        break;
                }
            }
            var nOfRounds = Duration.GetNumberOfRounds(bonues.Abilities, bonues);
            return nOfRounds - numberOfRoundsActive;
        }

        public bool GetIsActive(GetBonusDto bonues, Guid parentAbilityId, bool? targetHit = null)
        {
            var isActive = bonues.Round.ActivatedAbilities.Any(x => x.AbilityId == parentAbilityId && x.IsActive);

            var autoActive = (Amount.ActionRequired == RoundAction.AutoOnHit && targetHit.GetValueOrDefault()) || Amount.ActionRequired == RoundAction.AutoOnTakeDamage;
            if (isActive && !autoActive)
            {
                return true;
            }
            
            var autoChargePossible = IsChargesPossible(1, bonues, parentAbilityId);

            return autoActive && autoChargePossible;
        }
    }
}
