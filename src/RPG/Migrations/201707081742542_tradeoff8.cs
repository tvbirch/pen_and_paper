namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeoff8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsableAmounts", "TradeWith_ID", c => c.Guid());
            CreateIndex("dbo.UsableAmounts", "TradeWith_ID");
            AddForeignKey("dbo.UsableAmounts", "TradeWith_ID", "dbo.Bonus", "ID");
            DropColumn("dbo.UsableAmounts", "TradeWith");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsableAmounts", "TradeWith", c => c.Int());
            DropForeignKey("dbo.UsableAmounts", "TradeWith_ID", "dbo.Bonus");
            DropIndex("dbo.UsableAmounts", new[] { "TradeWith_ID" });
            DropColumn("dbo.UsableAmounts", "TradeWith_ID");
        }
    }
}
