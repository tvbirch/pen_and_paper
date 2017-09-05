namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeoff9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsableAmounts", "TradeDoubleIfThw", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsableAmounts", "TradeDoubleIfThw");
        }
    }
}
