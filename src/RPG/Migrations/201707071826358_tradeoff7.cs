namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeoff7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoundActivateAbilities", "Multiplier", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.UsableAmounts", "TradeMultiplyer", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.UsableAmounts", "TradeMaxTrade", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsableAmounts", "TradeMaxTrade", c => c.Int(nullable: false));
            AlterColumn("dbo.UsableAmounts", "TradeMultiplyer", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.RoundActivateAbilities", "Multiplier");
        }
    }
}
