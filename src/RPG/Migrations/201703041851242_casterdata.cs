namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class casterdata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpellsKnownPrLevels",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        SpellLevel = c.Int(nullable: false),
                        NumberOfSpells = c.Int(),
                        SpellKnown_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SpellKnowns", t => t.SpellKnown_ID)
                .Index(t => t.SpellKnown_ID);
            
            DropColumn("dbo.SpellKnowns", "SpellLevel");
            DropColumn("dbo.SpellKnowns", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpellKnowns", "Number", c => c.Int());
            AddColumn("dbo.SpellKnowns", "SpellLevel", c => c.Int(nullable: false));
            DropForeignKey("dbo.SpellsKnownPrLevels", "SpellKnown_ID", "dbo.SpellKnowns");
            DropIndex("dbo.SpellsKnownPrLevels", new[] { "SpellKnown_ID" });
            DropTable("dbo.SpellsKnownPrLevels");
        }
    }
}
