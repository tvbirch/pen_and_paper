namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapzoom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Maps", "Width", c => c.Int(nullable: false));
            AddColumn("dbo.Maps", "Height", c => c.Int(nullable: false));
            AddColumn("dbo.Maps", "MinZoom", c => c.Int(nullable: false));
            AddColumn("dbo.Maps", "MaxZoom", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Maps", "MaxZoom");
            DropColumn("dbo.Maps", "MinZoom");
            DropColumn("dbo.Maps", "Height");
            DropColumn("dbo.Maps", "Width");
        }
    }
}
