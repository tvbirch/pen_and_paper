namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeoff5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UsableAmounts", "TradeWith", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsableAmounts", "TradeWith", c => c.Int(nullable: false));
        }
    }
}
