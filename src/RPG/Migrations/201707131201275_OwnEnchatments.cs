namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnEnchatments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OwnedItemSpecialAbilities",
                c => new
                    {
                        OwnedItem_ID = c.Guid(nullable: false),
                        SpecialAbility_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.OwnedItem_ID, t.SpecialAbility_ID })
                .ForeignKey("dbo.OwnedItems", t => t.OwnedItem_ID, cascadeDelete: true)
                .ForeignKey("dbo.SpecialAbilities", t => t.SpecialAbility_ID, cascadeDelete: true)
                .Index(t => t.OwnedItem_ID)
                .Index(t => t.SpecialAbility_ID);
            
            CreateTable(
                "dbo.OwnedItemBonus",
                c => new
                    {
                        OwnedItem_ID = c.Guid(nullable: false),
                        Bonus_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.OwnedItem_ID, t.Bonus_ID })
                .ForeignKey("dbo.OwnedItems", t => t.OwnedItem_ID, cascadeDelete: true)
                .ForeignKey("dbo.Bonus", t => t.Bonus_ID, cascadeDelete: true)
                .Index(t => t.OwnedItem_ID)
                .Index(t => t.Bonus_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OwnedItemBonus", "Bonus_ID", "dbo.Bonus");
            DropForeignKey("dbo.OwnedItemBonus", "OwnedItem_ID", "dbo.OwnedItems");
            DropForeignKey("dbo.OwnedItemSpecialAbilities", "SpecialAbility_ID", "dbo.SpecialAbilities");
            DropForeignKey("dbo.OwnedItemSpecialAbilities", "OwnedItem_ID", "dbo.OwnedItems");
            DropIndex("dbo.OwnedItemBonus", new[] { "Bonus_ID" });
            DropIndex("dbo.OwnedItemBonus", new[] { "OwnedItem_ID" });
            DropIndex("dbo.OwnedItemSpecialAbilities", new[] { "SpecialAbility_ID" });
            DropIndex("dbo.OwnedItemSpecialAbilities", new[] { "OwnedItem_ID" });
            DropTable("dbo.OwnedItemBonus");
            DropTable("dbo.OwnedItemSpecialAbilities");
        }
    }
}
