namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LanguageCharacters", newName: "CharacterLanguages");
            RenameTable(name: "dbo.SpecialAbilityItemBases", newName: "ItemBaseSpecialAbilities");
            RenameTable(name: "dbo.BonusItemBases", newName: "ItemBaseBonus");
            DropForeignKey("dbo.AbilityByClassLevels", "Ability_ID", "dbo.SpecialAbilities");
            DropIndex("dbo.AbilityByClassLevels", new[] { "Ability_ID" });
            DropPrimaryKey("dbo.CharacterLanguages");
            DropPrimaryKey("dbo.ItemBaseSpecialAbilities");
            DropPrimaryKey("dbo.ItemBaseBonus");
            AlterColumn("dbo.AbilityByClassLevels", "Ability_ID", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.CharacterLanguages", new[] { "Character_ID", "Language_ID" });
            AddPrimaryKey("dbo.ItemBaseSpecialAbilities", new[] { "ItemBase_ID", "SpecialAbility_ID" });
            AddPrimaryKey("dbo.ItemBaseBonus", new[] { "ItemBase_ID", "Bonus_ID" });
            CreateIndex("dbo.AbilityByClassLevels", "Ability_ID");
            AddForeignKey("dbo.AbilityByClassLevels", "Ability_ID", "dbo.SpecialAbilities", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbilityByClassLevels", "Ability_ID", "dbo.SpecialAbilities");
            DropIndex("dbo.AbilityByClassLevels", new[] { "Ability_ID" });
            DropPrimaryKey("dbo.ItemBaseBonus");
            DropPrimaryKey("dbo.ItemBaseSpecialAbilities");
            DropPrimaryKey("dbo.CharacterLanguages");
            AlterColumn("dbo.AbilityByClassLevels", "Ability_ID", c => c.Guid());
            AddPrimaryKey("dbo.ItemBaseBonus", new[] { "Bonus_ID", "ItemBase_ID" });
            AddPrimaryKey("dbo.ItemBaseSpecialAbilities", new[] { "SpecialAbility_ID", "ItemBase_ID" });
            AddPrimaryKey("dbo.CharacterLanguages", new[] { "Language_ID", "Character_ID" });
            CreateIndex("dbo.AbilityByClassLevels", "Ability_ID");
            AddForeignKey("dbo.AbilityByClassLevels", "Ability_ID", "dbo.SpecialAbilities", "ID");
            RenameTable(name: "dbo.ItemBaseBonus", newName: "BonusItemBases");
            RenameTable(name: "dbo.ItemBaseSpecialAbilities", newName: "SpecialAbilityItemBases");
            RenameTable(name: "dbo.CharacterLanguages", newName: "LanguageCharacters");
        }
    }
}
