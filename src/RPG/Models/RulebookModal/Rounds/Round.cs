using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Conditions;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using RPG.Models.RulebookModal.BaseTypes.Time;


namespace RPG.Models.RulebookModal.Rounds
{
    public class Round : GameId
    {
        //The guid represent what used the action
        public List<RoundActionTaken> UsedActions { get; set; }
        public List<RoundActivateAbilities> ActivatedAbilities { get; set; }
        public List<RoundActivateConditions> ActivatedConditions { get; set; }

        public void TakeAction(CastingTime castingTime)
        {
            var intCastingTime = (int) castingTime;
            RoundAction asRound;
            if (intCastingTime > (int) RoundAction.FullRound)
            {
                asRound = RoundAction.FullRound;
            }
            else
            {
                asRound = (RoundAction)intCastingTime;
            }
            TakeAction(asRound);
        }

        public void TakeAction(RoundAction action, Guid? optionWeaponKey = null)
        {
            if (UsedActions == null)
            {
                UsedActions = new List<RoundActionTaken>();
            }
            UsedActions.Add(new RoundActionTaken
            {
                Action = action,
                ActionUsedBy = optionWeaponKey,
            });
        }

        public List<RoundAction> GetPossibleActions(GetBonusDto bonues)
        {
            if (UsedActions == null)
            {
                UsedActions = new List<RoundActionTaken>();
            }
            var numberOfFullRoundLeft = 1;
            var numberOfStandardLeft = 0;
            var numberOfMoveLeft = 0;
            var numberOfSwiftLeft = 1;

            foreach (var bonus in bonues.Bonuses)
            {
                if ((bonus.Type == BonusType.FullRoundAction ||
                    bonus.Type == BonusType.StandardAction ||
                    bonus.Type == BonusType.MoveAction ||
                    bonus.Type == BonusType.SwiftAction) &&
                    bonus.IsActive(bonues))
                {
                    switch (bonus.Type)
                    {
                        case BonusType.FullRoundAction:
                            numberOfFullRoundLeft += 1;
                            break;
                        case BonusType.StandardAction:
                            numberOfStandardLeft += 1;
                            break;
                        case BonusType.MoveAction:
                            numberOfMoveLeft += 1;
                            break;
                        case BonusType.SwiftAction:
                            numberOfSwiftLeft += 1;
                            break;
                    }
                }
            }

            foreach (var usedAction in UsedActions)
            {
                switch (usedAction.Action)
                {
                    case  RoundAction.FullRound:
                        numberOfFullRoundLeft -= 1;
                        break;
                    case RoundAction.Standard:
                        numberOfStandardLeft -= 1;
                        break;
                    case RoundAction.Move:
                        numberOfMoveLeft -= 1;
                        break;
                    case RoundAction.Swift:
                        numberOfSwiftLeft -= 1;
                        break;
                }
            }

            if (numberOfSwiftLeft < 0)
            {
                numberOfStandardLeft--;
            }
            if (numberOfStandardLeft < 0 || numberOfMoveLeft < 0)
            {
                numberOfFullRoundLeft--;
                numberOfStandardLeft ++;
                numberOfMoveLeft++;
            }
            if (numberOfMoveLeft < 0 && numberOfStandardLeft > 0)
            {
                numberOfMoveLeft++;
                numberOfStandardLeft--;
            }

            var possibleAction = new List<RoundAction>();
            if (numberOfFullRoundLeft > 0)
            {
                possibleAction.Add(RoundAction.FullRound);
            }
            if (numberOfStandardLeft > 0 || numberOfFullRoundLeft > 0)
            {
                possibleAction.Add(RoundAction.Standard);
            }
            if (numberOfMoveLeft > 0 || numberOfFullRoundLeft > 0 || numberOfStandardLeft > 0)
            {
                possibleAction.Add(RoundAction.Move);
            }
            if (numberOfSwiftLeft > 0 || numberOfStandardLeft > 0 || numberOfFullRoundLeft > 0)
            {
                possibleAction.Add(RoundAction.Swift);
            }
            possibleAction.Add(RoundAction.Free);
            possibleAction.Add(RoundAction.Immediate);

            return possibleAction;
        }

        public TimeChangeRemoval ChangeTime(ChangeCharacterTimeDto changeCharacterTimeDto)
        {
            ActivatedAbilities.ForEach(x => x.ActiveTime.Add(new TimeLimitUnitParsed
            {
                Time = changeCharacterTimeDto.TimeUnit
            }));
            var toRemove = new List<RoundActivateAbilities>();
            foreach (var feat in changeCharacterTimeDto.Bonus.Feats)
            {
                toRemove.AddRange(feat.ChangeTime(changeCharacterTimeDto));
            }

            foreach (var roundActivateAbilitiese in toRemove)
            {
                //Removing the time unit just added. No need to add it to DB to be removed.
                roundActivateAbilitiese.ActiveTime.RemoveAll(x => x.ID == Guid.Empty);
            }

            var dmgToRemove = new List<DamageTaken>();
            if (changeCharacterTimeDto.TimeUnit == TimeLimitUnit.Encounter || changeCharacterTimeDto.TimeUnit == TimeLimitUnit.Day)
            {
                dmgToRemove = changeCharacterTimeDto.Bonus.Character.HitPoints.DamgeToDelete(changeCharacterTimeDto.Bonus);
            }

            //UsedActions.RemoveAll(x => x.ID == Guid.Empty);
            
            return new TimeChangeRemoval
            {
                UsedActions = GetActionToRemove(UsedActions, changeCharacterTimeDto.TimeUnit),
                DeaktivatedAbilities = toRemove,
                DeaktivatedConditions = ActivatedConditions.Where(x => x.AutoDismissAfter.HasValue && (int)x.AutoDismissAfter.Value <= (int)changeCharacterTimeDto.TimeUnit).ToList(),
                DamagesToRemove = dmgToRemove
            };
        }

        private List<RoundActionTaken> GetActionToRemove(List<RoundActionTaken> usedActions, TimeLimitUnit unit)
        {
            if (unit == TimeLimitUnit.Attack)
            {
                return usedActions.Where(x => x.Action == RoundAction.AutoOnHit || x.Action == RoundAction.AutoOnTakeDamage).ToList();
            }
            return usedActions;
        }

        public RoundAction? GetActionToUseWeapon(Guid? itemKey, int numberOfAttacks, Guid? primaryWeaponId, Guid? offHandWeapon)
        {
            var usedCount = UsedActions.Count(x => x.ActionUsedBy== itemKey);

            if (usedCount == 0 && !UsedActions.Any(x => x.Action == RoundAction.Standard || x.Action == RoundAction.FullRound))
            {
                return RoundAction.Standard;
            }
            else if (usedCount == 1 || (itemKey.HasValue && (itemKey == primaryWeaponId || itemKey == offHandWeapon)))
            {
                return RoundAction.Move;
            }
            else if (usedCount > 1 && usedCount < numberOfAttacks)
            {
                return RoundAction.Free;
            }
            else
            {
                return null;
            }
        }

        public int GetAttacksTaken()
        {
            return UsedActions.Count(x => x.ActionUsedBy != null);
        }

        public bool CanWeaponCurrentlyAttack(Guid ownedItemId, List<int> baseAttack, Guid? primaryWeaponId, Guid? offHandWeapon)
        {
            return GetBaseAttackWithWeapon(ownedItemId, baseAttack, primaryWeaponId, offHandWeapon).HasValue;
        }

        public int? GetBaseAttackWithWeapon(Guid ownedItemId, List<int> baseAttack, Guid? primaryWeaponId, Guid? offHandWeapon)
        {
            var actionToUse = GetActionToUseWeapon(ownedItemId, baseAttack.Count, primaryWeaponId, offHandWeapon);
            if (actionToUse == null)
            {
                return null;
            }
            
            var usedCount = UsedActions.Count(x => x.ActionUsedBy== ownedItemId);
            
            if (usedCount < baseAttack.Count)
            {
                return baseAttack[usedCount];
            }
            return null;
        }
        
        public bool HasAttackedWithOtherThisRound(Guid ownedItemId, List<OwnedItem> items)
        {
            var weapoinsIds = UsedActions.Where(x => x.ActionUsedBy == ownedItemId).Select(x => x.ActionUsedBy.GetValueOrDefault());
            var hasAttackedWithOtherWeapon = items.Count(x => x.IsEquipped.GetValueOrDefault() && x.Item.Type == ItemType.Weapon && weapoinsIds.Contains(x.ID)) > 0;
            return hasAttackedWithOtherWeapon;
        }

        public void ApplyCondition(Condition condition, Guid? parentAbility)
        {
            if (ActivatedConditions == null)
            {
                ActivatedConditions  = new List<RoundActivateConditions>();
            }
            if (ActivatedConditions.Any(x => x.Condition == condition.ID))
            {
                if (condition.IfAlreadyActiveApplyCondition != null)
                {
                    ActivatedConditions.RemoveAll(x => x.Condition == condition.ID);

                    ActivatedConditions.Add(new RoundActivateConditions
                    {
                        ActivitedByAbility = parentAbility,
                        AutoDismissAfter = condition.IfAlreadyActiveApplyCondition.AutoDismissAfter,
                        Condition = condition.IfAlreadyActiveApplyCondition.ID,
                        ManuallyActivated = !parentAbility.HasValue
                    });
                }
                return;
            }
            ActivatedConditions.Add(new RoundActivateConditions
            {
                ActivitedByAbility = parentAbility,
                AutoDismissAfter = condition.AutoDismissAfter,
                Condition = condition.ID,
                ManuallyActivated = !parentAbility.HasValue
            });
        }

        public void EnrichActiveBonusDto(GetBonusDto dto)
        {
            var aktiveConditions = ActivatedConditions == null
                ? new List<Guid>()
                : ActivatedConditions.Select(x => x.Condition).ToList();
            var condition = dto.Character.Conditions.Where(x => aktiveConditions.Contains(x.ID));
            foreach (var con in condition)
            {
                foreach (var b in con.Bonuses)
                {
                    dto.ActiveBonus.Add(new BonusRef(con,b));
                }
            }
        }

        public void ManullyRemoveCondition(Guid conditionId)
        {
            ActivatedConditions.RemoveAll(x => x.Condition == conditionId);
        }
    }
}
