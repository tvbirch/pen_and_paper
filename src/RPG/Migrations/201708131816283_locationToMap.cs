namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class locationToMap : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Lat", c => c.Double());
            AddColumn("dbo.Locations", "Lon", c => c.Double());
            AddColumn("dbo.Locations", "Layer", c => c.Int());
            AddColumn("dbo.Locations", "Map_ID", c => c.Guid());
            CreateIndex("dbo.Locations", "Map_ID");
            AddForeignKey("dbo.Locations", "Map_ID", "dbo.Maps", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "Map_ID", "dbo.Maps");
            DropIndex("dbo.Locations", new[] { "Map_ID" });
            DropColumn("dbo.Locations", "Map_ID");
            DropColumn("dbo.Locations", "Layer");
            DropColumn("dbo.Locations", "Lon");
            DropColumn("dbo.Locations", "Lat");
        }
    }
}
