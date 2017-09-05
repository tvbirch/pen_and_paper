namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itemrequiresability3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemProficiencies",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        SpecialAbilityGuid = c.Guid(nullable: false),
                        ItemBase_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ItemBases", t => t.ItemBase_ID)
                .Index(t => t.ItemBase_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemProficiencies", "ItemBase_ID", "dbo.ItemBases");
            DropIndex("dbo.ItemProficiencies", new[] { "ItemBase_ID" });
            DropTable("dbo.ItemProficiencies");
        }
    }
}
