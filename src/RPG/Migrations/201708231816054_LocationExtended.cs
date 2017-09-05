namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationExtended : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Population", c => c.String());
            AddColumn("dbo.Locations", "Governance", c => c.String());
            AddColumn("dbo.Locations", "ProsperityAndCrime", c => c.String());
            AddColumn("dbo.Locations", "Races", c => c.String());
            AddColumn("dbo.Locations", "Military", c => c.String());
            AddColumn("dbo.Locations", "SourceOfIncome", c => c.String());
            AddColumn("dbo.Locations", "SourceOfFood", c => c.String());
            AddColumn("dbo.Locations", "GroupsAndFactions", c => c.String());
            AddColumn("dbo.Locations", "FeaturesAndLandmarks", c => c.String());
            AddColumn("dbo.Locations", "NotableLocations", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "NotableLocations");
            DropColumn("dbo.Locations", "FeaturesAndLandmarks");
            DropColumn("dbo.Locations", "GroupsAndFactions");
            DropColumn("dbo.Locations", "SourceOfFood");
            DropColumn("dbo.Locations", "SourceOfIncome");
            DropColumn("dbo.Locations", "Military");
            DropColumn("dbo.Locations", "Races");
            DropColumn("dbo.Locations", "ProsperityAndCrime");
            DropColumn("dbo.Locations", "Governance");
            DropColumn("dbo.Locations", "Population");
        }
    }
}
