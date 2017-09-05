namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeoff2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BonusToAdds", "Trade_ID", "dbo.BonusTradeOffs");
            DropIndex("dbo.BonusToAdds", new[] { "Trade_ID" });
            DropColumn("dbo.BonusToAdds", "Trade_ID");
            DropTable("dbo.BonusTradeOffs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BonusTradeOffs",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Trade = c.Int(nullable: false),
                        Multiplyer = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxTrade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.BonusToAdds", "Trade_ID", c => c.Guid());
            CreateIndex("dbo.BonusToAdds", "Trade_ID");
            AddForeignKey("dbo.BonusToAdds", "Trade_ID", "dbo.BonusTradeOffs", "ID");
        }
    }
}
