namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeoff11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsableAmounts", "TradeDoubleIfThw", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsableAmounts", "TradeDoubleIfThw");
        }
    }
}
