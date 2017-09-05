namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapscale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Maps", "DistanceScale", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Maps", "DistanceScale");
        }
    }
}
