namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class limitautocharges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsableAmounts", "LimitAutoCharges", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsableAmounts", "LimitAutoCharges");
        }
    }
}
