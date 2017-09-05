namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeoff3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsableAmounts", "TradeWith", c => c.Int(nullable: false));
            AddColumn("dbo.UsableAmounts", "TradeMultiplyer", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.UsableAmounts", "TradeMaxTrade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsableAmounts", "TradeMaxTrade");
            DropColumn("dbo.UsableAmounts", "TradeMultiplyer");
            DropColumn("dbo.UsableAmounts", "TradeWith");
        }
    }
}
