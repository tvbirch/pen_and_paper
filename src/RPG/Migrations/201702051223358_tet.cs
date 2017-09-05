namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saves", "BaseSave", c => c.Int(nullable: false));
            AddColumn("dbo.Saves", "AbilityScore", c => c.Int(nullable: false));
            AddColumn("dbo.Saves", "TempBonus", c => c.Int(nullable: false));
            AddColumn("dbo.Saves", "MiscBonus", c => c.Int(nullable: false));
            AlterColumn("dbo.Saves", "Name", c => c.String());
            AlterColumn("dbo.Saves", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Saves", "Description", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Saves", "Name", c => c.String(nullable: false, maxLength: 250));
            DropColumn("dbo.Saves", "MiscBonus");
            DropColumn("dbo.Saves", "TempBonus");
            DropColumn("dbo.Saves", "AbilityScore");
            DropColumn("dbo.Saves", "BaseSave");
        }
    }
}
