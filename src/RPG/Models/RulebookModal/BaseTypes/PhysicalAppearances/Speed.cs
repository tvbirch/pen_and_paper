using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Lists;


namespace RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances
{
    public class Speed : GameId
    {
        private List<ItemSlot> _equipedArmorItems;
        public Race RaceForBaseSpeed { get; private set; }
        public MaxBonusList<Bonus> Bonuses { get; private set; }

        public Speed(Race race)
        {
            ID = Configuration.SpeedId;
            _equipedArmorItems = new List<ItemSlot>();
            RaceForBaseSpeed = race;
            Bonuses = new MaxBonusList<Bonus>();
        }

        public bool ChangeTime(List<SpecialAbility> currentSpecialAbilities, TimeLimitUnit unit, List<AbilityScore> abilities)
        {
            Bonuses.ChangeTime(currentSpecialAbilities, unit, abilities);
            return false;
        }

        public void AddBonues(List<Bonus> bonues)
        {
            foreach (var bonus in bonues)
            {
                if (bonus.ShouldApplyTo(this))
                {
                    Bonuses.Add(bonus);
                }
            }
        }

        public void RemoveBonues(List<Bonus> bonues)
        {
            if (bonues == null)
            {
                return;
            }
            foreach (var bonus in bonues)
            {
                Bonuses.RemoveAll(x => x.ID == bonus.ID);
            }
        }

        public int GetSpeed(GetBonusDto bonusDto)
        {
            var limit = int.MaxValue;
            if (_equipedArmorItems != null && _equipedArmorItems.Any(x => x.Item != null && x.Item.Item.MaxSpeed.HasValue))
            {
                limit =
                    _equipedArmorItems.Select(x => x.Item.Item.MaxSpeed)
                        .OrderBy(x => x.GetValueOrDefault())
                        .FirstOrDefault()
                        .GetValueOrDefault();
            }

            return Bonuses.GetMaxValue(bonusDto) + Math.Min(RaceForBaseSpeed.GetBaseSpeed(), limit);
        }

        public void SetSpeedLimits(List<ItemSlot> slots)
        {
            _equipedArmorItems = slots.Where(x => x.Item != null && x.Item.Item.MaxSpeed.HasValue).ToList();
        }
    }
}
