namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OwnedItems", "Character_ID", "dbo.Characters");
            DropIndex("dbo.OwnedItems", new[] { "Character_ID" });
            AddColumn("dbo.Characters", "Inventory_Wealth_CopperPrice", c => c.Int());
            DropColumn("dbo.OwnedItems", "Character_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OwnedItems", "Character_ID", c => c.Guid());
            DropColumn("dbo.Characters", "Inventory_Wealth_CopperPrice");
            CreateIndex("dbo.OwnedItems", "Character_ID");
            AddForeignKey("dbo.OwnedItems", "Character_ID", "dbo.Characters", "ID");
        }
    }
}
