namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Wealth_CopperPrice = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.OwnedItems", "Inventory_ID", c => c.Guid());
            AddColumn("dbo.Characters", "Inventory_ID", c => c.Guid());
            CreateIndex("dbo.OwnedItems", "Inventory_ID");
            CreateIndex("dbo.Characters", "Inventory_ID");
            AddForeignKey("dbo.OwnedItems", "Inventory_ID", "dbo.Inventories", "ID");
            AddForeignKey("dbo.Characters", "Inventory_ID", "dbo.Inventories", "ID");
            DropColumn("dbo.Characters", "Inventory_Wealth_CopperPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Characters", "Inventory_Wealth_CopperPrice", c => c.Int());
            DropForeignKey("dbo.Characters", "Inventory_ID", "dbo.Inventories");
            DropForeignKey("dbo.OwnedItems", "Inventory_ID", "dbo.Inventories");
            DropIndex("dbo.Characters", new[] { "Inventory_ID" });
            DropIndex("dbo.OwnedItems", new[] { "Inventory_ID" });
            DropColumn("dbo.Characters", "Inventory_ID");
            DropColumn("dbo.OwnedItems", "Inventory_ID");
            DropTable("dbo.Inventories");
        }
    }
}
