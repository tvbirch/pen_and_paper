using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.Rounds;


namespace RPG.Models.RulebookModal.BaseTypes.Time
{
    public class UsableAmount : GameId
    {
        public TimeLimitUnit? ResetUseTimeLimtUnit { get; set; }
        public RoundAction? ActionRequired { get; set; }
        public bool ProvokesAttackOfOppertunity { get; set; }
        
        //Fixed amount
        public int? FixedAmount { get; set; }
        public Ability Ability { get; set; }

        //Auto stuff
        public int? LimitAutoCharges { get; set; }
        public AutoApplyLimit? VariableLimitAutoCharges { get; set; }


        //At levels
        public List<UsableAmountClassProgression> ClassProgression { get; set; }

        //By trade
        public Bonus TradeWith { get; set; }
        public decimal? TradeMultiplyer { get; set; }
        public int? TradeMaxTrade { get; set; }
        public bool TradeDoubleIfThw { get; set; }

        public int GetAmount(GetBonusDto bonues, Guid parentAbilityId)
        {
            var totalAmount = 0;
            if (FixedAmount.HasValue)
            {
                totalAmount += FixedAmount.Value + (Ability == null ? 0 : bonues.Abilities.First(x => x.Ability.ID == Ability.ID).GetCurrentModifier(bonues));
            }
            if (ClassProgression != null)
            {
                totalAmount += ClassProgression.Count(x => x.AtLevel <= bonues.Classes.First(y => y.Class.ID == x.ClassProgression.ID).Level);
            }
            if (TradeWith != null)
            {
                switch (TradeWith.ApplyTo.ApplyToType)
                {
                    case BonusApplyToType.BaseAttack:
                        totalAmount += bonues.Character.GetCurrentMaxBaseAttack();
                        break;
                    default:
                        throw new NotImplementedException("Trade not implementet");
                }
            }
            return totalAmount - bonues.Round.ActivatedAbilities.Where(x => x.AbilityId == parentAbilityId).Select(x=>x.Charges).Sum();
        }

        public void Use(int charges, GetBonusDto bonues, Guid parentAbilityId)
        {
            charges = LimitAutoChargesIfNecessary(charges, bonues);
            var existingRoundActivateAbilities = bonues.Round.ActivatedAbilities.FirstOrDefault(x => x.AbilityId == parentAbilityId && x.IsActive);
            if (existingRoundActivateAbilities == null && ActionRequired.HasValue)
            {
                bonues.Round.TakeAction(ActionRequired.Value);
                bonues.Round.ActivatedAbilities.Add(new RoundActivateAbilities
                {
                    AbilityId = parentAbilityId,
                    ActiveTime = new List<TimeLimitUnitParsed>(),
                    Charges = charges,
                    Multiplier = TradeMultiplyer,
                    IsActive = true,
                });
            }
            else if(existingRoundActivateAbilities != null)
            {
                existingRoundActivateAbilities.Charges += charges;
            }
        }

        public int LimitAutoChargesIfNecessary(int charges, GetBonusDto bonus)
        {
            if (LimitAutoCharges.HasValue)
            {
                charges = Math.Min(charges, (int) LimitAutoCharges.Value);
            }

            if (VariableLimitAutoCharges.HasValue)
            {
                var newMax = 0;
                switch (VariableLimitAutoCharges)
                {
                    case AutoApplyLimit.MaxDmgTaken:
                        newMax = -bonus.Character.HitPoints.GetDamageTaken();
                        break;
                    default:
                        throw new NotImplementedException("TODO");
                }
                charges = Math.Min(charges, newMax);
            }
            return charges;
        }

        public bool IsChargesPossible(int charge, GetBonusDto bonues,Guid parentAbilityId)
        {
            charge = LimitAutoChargesIfNecessary(charge, bonues);
            var currentAmount = GetAmount(bonues, parentAbilityId);
            var isActionPossible = ActionRequired == RoundAction.AutoOnHit ||
                ActionRequired == RoundAction.AutoOnTakeDamage ||
                ActionRequired.HasValue && bonues.Round.GetPossibleActions(bonues).Contains(ActionRequired.Value);
            var isAbilityActive = bonues.Round.ActivatedAbilities.FirstOrDefault(x => x.AbilityId == parentAbilityId) != null;
            var isChargeAbility = bonues.Feats.First(x => x.ID == parentAbilityId).IsChargeItem();

            return currentAmount >= charge && (isActionPossible || (isChargeAbility && isAbilityActive));
        }
    }
}
