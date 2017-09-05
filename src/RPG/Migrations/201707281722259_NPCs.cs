namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NPCs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Description = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Description = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NPCs",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Profession = c.String(),
                        Race = c.String(),
                        Age = c.String(),
                        Gender = c.String(),
                        VoiceMannersPersonality = c.String(),
                        RelationToParty = c.String(),
                        CombatStatistics = c.String(),
                        Affiliations = c.String(),
                        Name = c.String(nullable: false, maxLength: 250),
                        Description = c.String(maxLength: 4000),
                        Alligment_ID = c.Guid(),
                        Location_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Alligments", t => t.Alligment_ID)
                .ForeignKey("dbo.Locations", t => t.Location_ID)
                .Index(t => t.Alligment_ID)
                .Index(t => t.Location_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NPCs", "Location_ID", "dbo.Locations");
            DropForeignKey("dbo.NPCs", "Alligment_ID", "dbo.Alligments");
            DropIndex("dbo.NPCs", new[] { "Location_ID" });
            DropIndex("dbo.NPCs", new[] { "Alligment_ID" });
            DropTable("dbo.NPCs");
            DropTable("dbo.Locations");
            DropTable("dbo.Histories");
        }
    }
}
