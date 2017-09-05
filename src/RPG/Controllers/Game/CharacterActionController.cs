using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.CharacterModal;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Controllers.Game
{
    public class CharacterActionController : CharacterControllerBase
    {
        public void ChangeRound(Guid id)
        {
            ChangeTime(id, TimeLimitUnit.Round);
        }

        private void ChangeTime(Guid id, TimeLimitUnit timeUnit, Character character = null)
        {
            var cha = character ?? Context.LoadCharacter(id);
            var actionsToRemove = cha.ChangeTime(timeUnit);
            ChangeTimeCleanDb(actionsToRemove);
            if (timeUnit == TimeLimitUnit.Day && cha.SpellSlots != null)
            {
                foreach (var spellSlot in cha.SpellSlots)
                {
                    spellSlot.Used = false;
                }
            }
            Context.Context.SaveChanges();
        }

        private void ChangeTimeCleanDb(TimeChangeRemoval actionsToRemove)
        {
            if (actionsToRemove == null)
            {
                return;
            }
            if (actionsToRemove.DeaktivatedAbilities != null)
            {
                var rabIds = actionsToRemove.DeaktivatedAbilities.Select(x => x.ID).ToList();
                var tlupIds =
                    actionsToRemove.DeaktivatedAbilities.Select(x => x.ActiveTime.Select(y => y.ID))
                        .SelectMany(x => x)
                        .ToList();

                foreach (var id in tlupIds)
                {
                    Context.Delete<TimeLimitUnitParsed>(id);
                }
                foreach (var id in rabIds)
                {
                    Context.Delete<RoundActivateAbilities>(id);
                }
            }
            if (actionsToRemove.UsedActions != null)
            {
                var rabIds = actionsToRemove.UsedActions.Select(x => x.ID).ToList();
                foreach (var id in rabIds)
                {
                    Context.Delete<RoundActionTaken>(id);
                }
            }
            if (actionsToRemove.DeaktivatedConditions != null)
            {
                var rabIds = actionsToRemove.DeaktivatedConditions.Select(x => x.ID).ToList();
                foreach (var id in rabIds)
                {
                    Context.Delete<RoundActivateConditions>(id);
                }
            }
            if (actionsToRemove.DamagesToRemove != null)
            {
                foreach (var dmg in actionsToRemove.DamagesToRemove)
                {
                    Context.Delete<DamageTaken>(dmg.ID);
                }
            }
            
            //TODO: heal skal ikke tage sig af at slette dmg historik. Det skal køres af changetime. PT virker heal fra crystal ikke til fuld health.

        }

        public void ChangeEncounter(Guid id)
        {
            ChangeTime(id,TimeLimitUnit.Encounter);
        }
        public void ChangeDay(Guid id)
        {
            ChangeTime(id, TimeLimitUnit.Day);
        }

        public void TakeAction(Guid id, int action)
        {
            var cha = Context.LoadCharacter(id);
            cha.Round.TakeAction((RoundAction)action, null);
            Context.Context.SaveChanges();
        }

        public void TriggerAbility(Guid charId, Guid abilityId, int charges)
        {
            var cha = Context.LoadCharacter(charId);
            cha.TriggerAbility(abilityId, charges);
            Context.Context.SaveChanges();            
        }

        public void TakeDamage(Guid id, bool normalDamage, int damageType, int amount)
        {
            var cha = Context.LoadCharacter(id);
            cha.TakeDamage(normalDamage, (DamageType)damageType, amount);
            Context.Context.SaveChanges();
            //ChangeTime(id,TimeLimitUnit.Damage, cha);
        }
        public void HealDamage(Guid id,int amount)
        {
            var cha = Context.LoadCharacter(id);
            cha.HealDamage(amount);
            Context.Context.SaveChanges();
        }

        public void AttackWithWeapon(Guid id, Guid weaponId, Guid? target, int? nrOfEnemies)
        {
            var cha = Context.LoadCharacter(id);

            if (target.HasValue)
            {
                //TODO
            }
            else
            {
                cha.AttackWithItem(weaponId, nrOfEnemies);
            }

            Context.Context.SaveChanges();
            ChangeTime(id, TimeLimitUnit.Attack,cha);
        }

        public void AddCondtion(Guid id, Guid conditionId)
        {
            var cha = Context.LoadCharacter(id);
            cha.AddCondtion(conditionId);
            Context.Context.SaveChanges();
        }

        public void RemoveCondtion(Guid id, Guid conditionId)
        {
            var cha = Context.LoadCharacter(id);
            cha.RemoveCondtion(conditionId);
            Context.Context.SaveChanges();
        }
    }
}
