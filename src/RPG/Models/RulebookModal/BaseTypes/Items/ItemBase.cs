using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.CoreModal.DTO;
using RPG.Models.CoreModal.Extensions;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Currency;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.RulebookModal.BaseTypes.Items
{
    public enum ItemSlotRequirement
    {
        Face = 0,
        Head = 1,
        Throat = 2,
        Shoulder = 3,
        Body = 4,
        Torso= 5,
        Hands = 6,
        Arms = 7,
        Waist = 8,
        Feet = 9,
        Ring = 10,
        Misc = 11,
        WeaponHand = 12,
        WeaponCrystal = 13,
        ArmorCrystal = 14
    }

    public enum ItemType
    {
        Weapon = 0,
        Armor = 1,
        Shield = 2,
        Other = 3,
        Unarmed = 4
    }
    public class ItemBase : ElementId
    {
        public ItemBase BaseItem { get; set; }
        public ItemType Type { get; set; }
        public ImperialWeight Weight { get; set; }
        public Money Price { get; set; }
        public ItemMaterial Material { get; set; }

        public List<ItemProficiency> RequiresAbility { get; set; }
        public List<ItemSlotRequirements> RequiresSlots { get; set; }
        public List<ItemSlotRequirements> HasSlots { get; set; }

        public List<SpecialAbility> EnchanmentAbilities { get; set; }
        public List<Bonus> EnchanmentsBonuses { get; set; }
        public int? EnchanmentBonus { get; set; }
        public bool Masterworked { get; set; }

        public int? ArmorCheckPenelty { get; set; }
        public int? AC { get; set; }
        public int? MaxDexBonus { get; set; }
        public int? MaxSpeed { get; set; }
        public int? ArcaneSpellFailure { get; set; }
        
        public bool CanAttack { get; set; }
        public Damage Damage { get; set; }
        public int? UseItemsOwnAbilistyScore { get; set; }
        public int? AttackBonus { get; set; }
        public int? CritRange { get; set; }
        public bool IsOneAndAHalfHanded { get; set; }
        public bool IsLightWeapon { get; set; }
        public bool IsRanged { get; set; }
        public int? RangeIncrement { get; set; }
        public int? CriticalMultiplier { get; set; }

        public virtual ICollection<OwnedItem> OwnedItems { get; set; }
    }
}
