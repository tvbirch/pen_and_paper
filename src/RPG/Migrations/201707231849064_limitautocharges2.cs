namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class limitautocharges2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsableAmounts", "VariableLimitAutoCharges", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsableAmounts", "VariableLimitAutoCharges");
        }
    }
}
