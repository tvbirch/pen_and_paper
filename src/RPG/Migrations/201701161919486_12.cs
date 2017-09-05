namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemBaseSpecialAbilities", "ItemBase_ID", "dbo.ItemBases");
            DropForeignKey("dbo.ItemBaseSpecialAbilities", "SpecialAbility_ID", "dbo.SpecialAbilities");
            DropIndex("dbo.ItemBaseSpecialAbilities", new[] { "ItemBase_ID" });
            DropIndex("dbo.ItemBaseSpecialAbilities", new[] { "SpecialAbility_ID" });
            AddColumn("dbo.SpecialAbilities", "ItemBase_ID", c => c.Guid());
            AddColumn("dbo.SpecialAbilities", "Race_ID", c => c.Guid());
            AddColumn("dbo.ItemBases", "SpecialAbility_ID", c => c.Guid());
            AddColumn("dbo.ItemBases", "SpecialAbility_ID1", c => c.Guid());
            CreateIndex("dbo.SpecialAbilities", "ItemBase_ID");
            CreateIndex("dbo.SpecialAbilities", "Race_ID");
            CreateIndex("dbo.ItemBases", "SpecialAbility_ID");
            CreateIndex("dbo.ItemBases", "SpecialAbility_ID1");
            AddForeignKey("dbo.SpecialAbilities", "ItemBase_ID", "dbo.ItemBases", "ID");
            AddForeignKey("dbo.SpecialAbilities", "Race_ID", "dbo.Races", "ID");
            AddForeignKey("dbo.ItemBases", "SpecialAbility_ID", "dbo.SpecialAbilities", "ID");
            AddForeignKey("dbo.ItemBases", "SpecialAbility_ID1", "dbo.SpecialAbilities", "ID");
            DropTable("dbo.ItemBaseSpecialAbilities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ItemBaseSpecialAbilities",
                c => new
                    {
                        ItemBase_ID = c.Guid(nullable: false),
                        SpecialAbility_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemBase_ID, t.SpecialAbility_ID });
            
            DropForeignKey("dbo.ItemBases", "SpecialAbility_ID1", "dbo.SpecialAbilities");
            DropForeignKey("dbo.ItemBases", "SpecialAbility_ID", "dbo.SpecialAbilities");
            DropForeignKey("dbo.SpecialAbilities", "Race_ID", "dbo.Races");
            DropForeignKey("dbo.SpecialAbilities", "ItemBase_ID", "dbo.ItemBases");
            DropIndex("dbo.ItemBases", new[] { "SpecialAbility_ID1" });
            DropIndex("dbo.ItemBases", new[] { "SpecialAbility_ID" });
            DropIndex("dbo.SpecialAbilities", new[] { "Race_ID" });
            DropIndex("dbo.SpecialAbilities", new[] { "ItemBase_ID" });
            DropColumn("dbo.ItemBases", "SpecialAbility_ID1");
            DropColumn("dbo.ItemBases", "SpecialAbility_ID");
            DropColumn("dbo.SpecialAbilities", "Race_ID");
            DropColumn("dbo.SpecialAbilities", "ItemBase_ID");
            CreateIndex("dbo.ItemBaseSpecialAbilities", "SpecialAbility_ID");
            CreateIndex("dbo.ItemBaseSpecialAbilities", "ItemBase_ID");
            AddForeignKey("dbo.ItemBaseSpecialAbilities", "SpecialAbility_ID", "dbo.SpecialAbilities", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ItemBaseSpecialAbilities", "ItemBase_ID", "dbo.ItemBases", "ID", cascadeDelete: true);
        }
    }
}
