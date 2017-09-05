using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.GmModal;
using RPG.Models.GmModal.World;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Alligments;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Conditions;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Rounds;

namespace RPG.Models.Context
{
    public class GameContext : DbContext
    {
        public static string ConnectionString = "Data Source=localhost;Initial Catalog=RPG.Models.Context.GameContext;Persist Security Info=True;User ID=rpg;Password=P@ssw0rd";
        
        //Small data carrying sets
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<AbilityScore> AbilityScores { get; set; }
        public DbSet<AbilityByClassLevel> AbilityByClassLevels { get; set; }
        public DbSet<Alligment> Alligments { get; set; }
        public DbSet<SaveType> SaveType { get; set; }
        public DbSet<SaveRate> SaveRate { get; set; }
        public DbSet<SpellComponent> SpellComponents { get; set; }
        public DbSet<SpellDescriptor> SpellDescriptors { get; set; }
        public DbSet<SpellSchool> SpellSchools { get; set; }
        public DbSet<OwnedItem> OwnedItems { get; set; }
        public DbSet<SkillRank> SkillRanks { get; set; }
        public DbSet<UsableAmountClassProgression> UsableAmountClassProgressions { get; set; }

        public DbSet<RoundActivateConditions> RoundActivateConditions { get; set; }
        public DbSet<RoundActivateAbilities> RoundActivateAbilities { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<RoundActionTaken> RoundActionsTaken { get; set; }
        public DbSet<TimeLimitUnitParsed> TimeLimitUnitParsed { get; set; }
        public DbSet<SpellPrDay> SpellPrDay { get; set; }
        public DbSet<SpellsPrLevel> SpellsPrLevel { get; set; }
        public DbSet<SpellsKnownPrLevel> SpellsKnownPrLevel { get; set; }
        public DbSet<SpellKnown> SpellsKnown { get; set; }
        public DbSet<SpellSlot> SpellSlots { get; set; }

        public DbSet<ClassLevel> ClassLevels { get; set; }

        
        public DbSet<BonusToAdd> BonusToAdds { get; set; }
        public DbSet<BonusToAddClassProgression> BonusToAddClassProgressions { get; set; }
        public DbSet<BonusFromCharges> BonusFromCharges { get; set; }


        //Rules
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<ClassBase> Classes { get; set; }
        public DbSet<ItemBase> ItemBase { get; set; }
        public DbSet<ItemMaterial> ItemMaterial { get; set; }
        public DbSet<MaterialBonuses> MaterialBonuses { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillSynergi> SkillSynergies { get; set; }
        public DbSet<SpecialAbility> SpecialAbilities { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<SpellRequiretLevel> SpellRequiretLevel { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<DiceRoll> DiceRolls { get; set; }

        //CharacterStuff
        public DbSet<Character> Characters { get; set; }
        //TODO: When adding this stuff, also change relations to Rules and small data sets, such that N to N are created correctly


        //GmStuff
        public DbSet<GmCharacterView> GmCharacterViews { get; set; }
        public DbSet<NPC> NPCs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<MapPart> MapParts { get; set; }

        public GameContext(): base(ConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false; 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SkillSynergi>()
                        .HasRequired(m => m.SynergiFrom)
                        .WithMany(t => t.SynergiFrom)
                        .HasForeignKey(m => m.SynergiFromId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<SkillSynergi>()
                        .HasRequired(m => m.SynergiApplyTo)
                        .WithMany(t => t.SynergiApplyTo)
                        .HasForeignKey(m => m.SynergiApplyToId)
                        .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Race>()
            .HasMany(p => p.BonusLanguages)
            .WithMany(r => r.BonusLangageFor)
            .Map(mc =>
            {
                mc.MapLeftKey("Race");
                mc.MapRightKey("Language");
                mc.ToTable("RacialBonusLanguages");
            });
            modelBuilder.Entity<Race>()
            .HasMany(p => p.Languages)
            .WithMany(r => r.RacialLangageFor)
            .Map(mc =>
            {
                mc.MapLeftKey("Race");
                mc.MapRightKey("Language");
                mc.ToTable("RacialLanguages");
            });
            //modelBuilder.Entity<BonusToAdd>().HasRequired(m => m.Parent).WithOptionalDependent().WillCascadeOnDelete(true);
        }
    }
}