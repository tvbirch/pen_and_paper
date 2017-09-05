namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testrelation1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CasterLevelLists", "AbilityUsedForCasting_ID", "dbo.Abilities");
            DropForeignKey("dbo.ClassBases", "Spells_ID", "dbo.CasterLevelLists");
            DropForeignKey("dbo.SpellKnowns", "CasterLevelList_ID", "dbo.CasterLevelLists");
            DropForeignKey("dbo.SpellPrDays", "CasterLevelList_ID", "dbo.CasterLevelLists");
            DropIndex("dbo.ClassBases", new[] { "Spells_ID" });
            DropIndex("dbo.CasterLevelLists", new[] { "AbilityUsedForCasting_ID" });
            DropIndex("dbo.SpellKnowns", new[] { "CasterLevelList_ID" });
            DropIndex("dbo.SpellPrDays", new[] { "CasterLevelList_ID" });
            AddColumn("dbo.ClassBases", "AbilityUsedForCasting_ID", c => c.Guid());
            AddColumn("dbo.SpellKnowns", "ClassBase_ID", c => c.Guid());
            AddColumn("dbo.SpellPrDays", "ClassBase_ID", c => c.Guid());
            CreateIndex("dbo.ClassBases", "AbilityUsedForCasting_ID");
            CreateIndex("dbo.SpellKnowns", "ClassBase_ID");
            CreateIndex("dbo.SpellPrDays", "ClassBase_ID");
            AddForeignKey("dbo.ClassBases", "AbilityUsedForCasting_ID", "dbo.Abilities", "ID");
            AddForeignKey("dbo.SpellKnowns", "ClassBase_ID", "dbo.ClassBases", "ID");
            AddForeignKey("dbo.SpellPrDays", "ClassBase_ID", "dbo.ClassBases", "ID");
            DropColumn("dbo.ClassBases", "Spells_ID");
            DropColumn("dbo.SpellKnowns", "CasterLevelList_ID");
            DropColumn("dbo.SpellPrDays", "CasterLevelList_ID");
            DropTable("dbo.CasterLevelLists");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CasterLevelLists",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        AbilityUsedForCasting_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.SpellPrDays", "CasterLevelList_ID", c => c.Guid());
            AddColumn("dbo.SpellKnowns", "CasterLevelList_ID", c => c.Guid());
            AddColumn("dbo.ClassBases", "Spells_ID", c => c.Guid());
            DropForeignKey("dbo.SpellPrDays", "ClassBase_ID", "dbo.ClassBases");
            DropForeignKey("dbo.SpellKnowns", "ClassBase_ID", "dbo.ClassBases");
            DropForeignKey("dbo.ClassBases", "AbilityUsedForCasting_ID", "dbo.Abilities");
            DropIndex("dbo.SpellPrDays", new[] { "ClassBase_ID" });
            DropIndex("dbo.SpellKnowns", new[] { "ClassBase_ID" });
            DropIndex("dbo.ClassBases", new[] { "AbilityUsedForCasting_ID" });
            DropColumn("dbo.SpellPrDays", "ClassBase_ID");
            DropColumn("dbo.SpellKnowns", "ClassBase_ID");
            DropColumn("dbo.ClassBases", "AbilityUsedForCasting_ID");
            CreateIndex("dbo.SpellPrDays", "CasterLevelList_ID");
            CreateIndex("dbo.SpellKnowns", "CasterLevelList_ID");
            CreateIndex("dbo.CasterLevelLists", "AbilityUsedForCasting_ID");
            CreateIndex("dbo.ClassBases", "Spells_ID");
            AddForeignKey("dbo.SpellPrDays", "CasterLevelList_ID", "dbo.CasterLevelLists", "ID");
            AddForeignKey("dbo.SpellKnowns", "CasterLevelList_ID", "dbo.CasterLevelLists", "ID");
            AddForeignKey("dbo.ClassBases", "Spells_ID", "dbo.CasterLevelLists", "ID");
            AddForeignKey("dbo.CasterLevelLists", "AbilityUsedForCasting_ID", "dbo.Abilities", "ID");
        }
    }
}
