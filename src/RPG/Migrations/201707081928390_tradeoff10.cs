namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeoff10 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UsableAmounts", "TradeDoubleIfThw");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsableAmounts", "TradeDoubleIfThw", c => c.Boolean());
        }
    }
}
