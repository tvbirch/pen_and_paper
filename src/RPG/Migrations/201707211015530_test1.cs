namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SpecialAbilities", "ItemBase_ID", "dbo.ItemBases");
            DropForeignKey("dbo.SpecialAbilities", "Race_ID", "dbo.Races");
            DropForeignKey("dbo.ItemBases", "SpecialAbility_ID", "dbo.SpecialAbilities");
            DropForeignKey("dbo.ItemBases", "SpecialAbility_ID1", "dbo.SpecialAbilities");
            DropIndex("dbo.SpecialAbilities", new[] { "ItemBase_ID" });
            DropIndex("dbo.SpecialAbilities", new[] { "Race_ID" });
            DropIndex("dbo.ItemBases", new[] { "SpecialAbility_ID" });
            DropIndex("dbo.ItemBases", new[] { "SpecialAbility_ID1" });
            CreateTable(
                "dbo.ItemBaseSpecialAbilities",
                c => new
                    {
                        ItemBase_ID = c.Guid(nullable: false),
                        SpecialAbility_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemBase_ID, t.SpecialAbility_ID })
                .ForeignKey("dbo.ItemBases", t => t.ItemBase_ID, cascadeDelete: true)
                .ForeignKey("dbo.SpecialAbilities", t => t.SpecialAbility_ID, cascadeDelete: true)
                .Index(t => t.ItemBase_ID)
                .Index(t => t.SpecialAbility_ID);
            
            CreateTable(
                "dbo.RaceSpecialAbilities",
                c => new
                    {
                        Race_ID = c.Guid(nullable: false),
                        SpecialAbility_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Race_ID, t.SpecialAbility_ID })
                .ForeignKey("dbo.Races", t => t.Race_ID, cascadeDelete: true)
                .ForeignKey("dbo.SpecialAbilities", t => t.SpecialAbility_ID, cascadeDelete: true)
                .Index(t => t.Race_ID)
                .Index(t => t.SpecialAbility_ID);
            
            DropColumn("dbo.SpecialAbilities", "ItemBase_ID");
            DropColumn("dbo.SpecialAbilities", "Race_ID");
            DropColumn("dbo.ItemBases", "SpecialAbility_ID");
            DropColumn("dbo.ItemBases", "SpecialAbility_ID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemBases", "SpecialAbility_ID1", c => c.Guid());
            AddColumn("dbo.ItemBases", "SpecialAbility_ID", c => c.Guid());
            AddColumn("dbo.SpecialAbilities", "Race_ID", c => c.Guid());
            AddColumn("dbo.SpecialAbilities", "ItemBase_ID", c => c.Guid());
            DropForeignKey("dbo.RaceSpecialAbilities", "SpecialAbility_ID", "dbo.SpecialAbilities");
            DropForeignKey("dbo.RaceSpecialAbilities", "Race_ID", "dbo.Races");
            DropForeignKey("dbo.ItemBaseSpecialAbilities", "SpecialAbility_ID", "dbo.SpecialAbilities");
            DropForeignKey("dbo.ItemBaseSpecialAbilities", "ItemBase_ID", "dbo.ItemBases");
            DropIndex("dbo.RaceSpecialAbilities", new[] { "SpecialAbility_ID" });
            DropIndex("dbo.RaceSpecialAbilities", new[] { "Race_ID" });
            DropIndex("dbo.ItemBaseSpecialAbilities", new[] { "SpecialAbility_ID" });
            DropIndex("dbo.ItemBaseSpecialAbilities", new[] { "ItemBase_ID" });
            DropTable("dbo.RaceSpecialAbilities");
            DropTable("dbo.ItemBaseSpecialAbilities");
            CreateIndex("dbo.ItemBases", "SpecialAbility_ID1");
            CreateIndex("dbo.ItemBases", "SpecialAbility_ID");
            CreateIndex("dbo.SpecialAbilities", "Race_ID");
            CreateIndex("dbo.SpecialAbilities", "ItemBase_ID");
            AddForeignKey("dbo.ItemBases", "SpecialAbility_ID1", "dbo.SpecialAbilities", "ID");
            AddForeignKey("dbo.ItemBases", "SpecialAbility_ID", "dbo.SpecialAbilities", "ID");
            AddForeignKey("dbo.SpecialAbilities", "Race_ID", "dbo.Races", "ID");
            AddForeignKey("dbo.SpecialAbilities", "ItemBase_ID", "dbo.ItemBases", "ID");
        }
    }
}
