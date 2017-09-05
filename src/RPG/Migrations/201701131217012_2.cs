namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ItemBaseBonus", newName: "BonusItemBases");
            RenameTable(name: "dbo.AlligmentClassBases", newName: "ClassBaseAlligments");
            RenameTable(name: "dbo.RaceClassBases", newName: "ClassBaseRaces");
            DropForeignKey("dbo.Bonus", "AbilityScore_ID", "dbo.AbilityScores");
            DropForeignKey("dbo.Bonus", "Save_ID", "dbo.Saves");
            DropIndex("dbo.Bonus", new[] { "AbilityScore_ID" });
            DropIndex("dbo.Bonus", new[] { "Save_ID" });
            DropPrimaryKey("dbo.BonusItemBases");
            DropPrimaryKey("dbo.ClassBaseAlligments");
            DropPrimaryKey("dbo.ClassBaseRaces");
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Xp = c.Int(nullable: false),
                        HitPoints_BaseHp = c.Int(nullable: false),
                        PhysicalAppearance_Height__inches = c.Double(nullable: false),
                        PhysicalAppearance_Weight_Lb = c.Double(nullable: false),
                        PhysicalAppearance_Age_CurrentAge = c.Int(nullable: false),
                        PhysicalAppearance_Gender = c.String(),
                        PhysicalAppearance_Eyes = c.String(),
                        PhysicalAppearance_Hair = c.String(),
                        PhysicalAppearance_Skin = c.String(),
                        Name = c.String(nullable: false, maxLength: 250),
                        Description = c.String(maxLength: 4000),
                        Alligment_ID = c.Guid(),
                        Race_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Alligments", t => t.Alligment_ID)
                .ForeignKey("dbo.Races", t => t.Race_ID)
                .Index(t => t.Alligment_ID)
                .Index(t => t.Race_ID);
            
            CreateTable(
                "dbo.SkillRanks",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Ranks = c.Int(nullable: false),
                        Skill_ID = c.Guid(),
                        Character_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Skills", t => t.Skill_ID)
                .ForeignKey("dbo.Characters", t => t.Character_ID)
                .Index(t => t.Skill_ID)
                .Index(t => t.Character_ID);
            
            CreateTable(
                "dbo.ClassLevels",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        Class_ID = c.Guid(),
                        Character_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ClassBases", t => t.Class_ID)
                .ForeignKey("dbo.Characters", t => t.Character_ID)
                .Index(t => t.Class_ID)
                .Index(t => t.Character_ID);
            
            CreateTable(
                "dbo.LanguageCharacters",
                c => new
                    {
                        Language_ID = c.Guid(nullable: false),
                        Character_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Language_ID, t.Character_ID })
                .ForeignKey("dbo.Languages", t => t.Language_ID, cascadeDelete: true)
                .ForeignKey("dbo.Characters", t => t.Character_ID, cascadeDelete: true)
                .Index(t => t.Language_ID)
                .Index(t => t.Character_ID);
            
            AddColumn("dbo.AbilityScores", "Character_ID", c => c.Guid());
            AddColumn("dbo.OwnedItems", "IsEquipped", c => c.Boolean());
            AddColumn("dbo.OwnedItems", "Character_ID", c => c.Guid());
            AddPrimaryKey("dbo.BonusItemBases", new[] { "Bonus_ID", "ItemBase_ID" });
            AddPrimaryKey("dbo.ClassBaseAlligments", new[] { "ClassBase_ID", "Alligment_ID" });
            AddPrimaryKey("dbo.ClassBaseRaces", new[] { "ClassBase_ID", "Race_ID" });
            CreateIndex("dbo.AbilityScores", "Character_ID");
            CreateIndex("dbo.OwnedItems", "Character_ID");
            AddForeignKey("dbo.AbilityScores", "Character_ID", "dbo.Characters", "ID");
            AddForeignKey("dbo.OwnedItems", "Character_ID", "dbo.Characters", "ID");
            DropColumn("dbo.Bonus", "AbilityScore_ID");
            DropColumn("dbo.Bonus", "Save_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bonus", "Save_ID", c => c.Guid());
            AddColumn("dbo.Bonus", "AbilityScore_ID", c => c.Guid());
            DropForeignKey("dbo.SkillRanks", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.ClassLevels", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.ClassLevels", "Class_ID", "dbo.ClassBases");
            DropForeignKey("dbo.OwnedItems", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.SkillRanks", "Skill_ID", "dbo.Skills");
            DropForeignKey("dbo.Characters", "Race_ID", "dbo.Races");
            DropForeignKey("dbo.LanguageCharacters", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.LanguageCharacters", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Characters", "Alligment_ID", "dbo.Alligments");
            DropForeignKey("dbo.AbilityScores", "Character_ID", "dbo.Characters");
            DropIndex("dbo.LanguageCharacters", new[] { "Character_ID" });
            DropIndex("dbo.LanguageCharacters", new[] { "Language_ID" });
            DropIndex("dbo.ClassLevels", new[] { "Character_ID" });
            DropIndex("dbo.ClassLevels", new[] { "Class_ID" });
            DropIndex("dbo.SkillRanks", new[] { "Character_ID" });
            DropIndex("dbo.SkillRanks", new[] { "Skill_ID" });
            DropIndex("dbo.OwnedItems", new[] { "Character_ID" });
            DropIndex("dbo.Characters", new[] { "Race_ID" });
            DropIndex("dbo.Characters", new[] { "Alligment_ID" });
            DropIndex("dbo.AbilityScores", new[] { "Character_ID" });
            DropPrimaryKey("dbo.ClassBaseRaces");
            DropPrimaryKey("dbo.ClassBaseAlligments");
            DropPrimaryKey("dbo.BonusItemBases");
            DropColumn("dbo.OwnedItems", "Character_ID");
            DropColumn("dbo.OwnedItems", "IsEquipped");
            DropColumn("dbo.AbilityScores", "Character_ID");
            DropTable("dbo.LanguageCharacters");
            DropTable("dbo.ClassLevels");
            DropTable("dbo.SkillRanks");
            DropTable("dbo.Characters");
            AddPrimaryKey("dbo.ClassBaseRaces", new[] { "Race_ID", "ClassBase_ID" });
            AddPrimaryKey("dbo.ClassBaseAlligments", new[] { "Alligment_ID", "ClassBase_ID" });
            AddPrimaryKey("dbo.BonusItemBases", new[] { "ItemBase_ID", "Bonus_ID" });
            CreateIndex("dbo.Bonus", "Save_ID");
            CreateIndex("dbo.Bonus", "AbilityScore_ID");
            AddForeignKey("dbo.Bonus", "Save_ID", "dbo.Saves", "ID");
            AddForeignKey("dbo.Bonus", "AbilityScore_ID", "dbo.AbilityScores", "ID");
            RenameTable(name: "dbo.ClassBaseRaces", newName: "RaceClassBases");
            RenameTable(name: "dbo.ClassBaseAlligments", newName: "AlligmentClassBases");
            RenameTable(name: "dbo.BonusItemBases", newName: "ItemBaseBonus");
        }
    }
}
