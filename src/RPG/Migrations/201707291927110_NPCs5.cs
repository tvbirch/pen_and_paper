namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NPCs5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Locations", "LocationType_ID", c => c.Guid());
            CreateIndex("dbo.Locations", "LocationType_ID");
            AddForeignKey("dbo.Locations", "LocationType_ID", "dbo.LocationTypes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "LocationType_ID", "dbo.LocationTypes");
            DropIndex("dbo.Locations", new[] { "LocationType_ID" });
            DropColumn("dbo.Locations", "LocationType_ID");
            DropTable("dbo.LocationTypes");
        }
    }
}
