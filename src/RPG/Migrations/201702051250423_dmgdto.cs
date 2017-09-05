namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dmgdto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HitPoints",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        BaseHp = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DamageTakens",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        HitPoints_ID = c.Guid(),
                        HitPoints_ID1 = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HitPoints", t => t.HitPoints_ID)
                .ForeignKey("dbo.HitPoints", t => t.HitPoints_ID1)
                .Index(t => t.HitPoints_ID)
                .Index(t => t.HitPoints_ID1);
            
            AddColumn("dbo.Characters", "HitPoints_ID", c => c.Guid());
            CreateIndex("dbo.Characters", "HitPoints_ID");
            AddForeignKey("dbo.Characters", "HitPoints_ID", "dbo.HitPoints", "ID");
            DropColumn("dbo.Characters", "HitPoints_BaseHp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Characters", "HitPoints_BaseHp", c => c.Int(nullable: false));
            DropForeignKey("dbo.Characters", "HitPoints_ID", "dbo.HitPoints");
            DropForeignKey("dbo.DamageTakens", "HitPoints_ID1", "dbo.HitPoints");
            DropForeignKey("dbo.DamageTakens", "HitPoints_ID", "dbo.HitPoints");
            DropIndex("dbo.DamageTakens", new[] { "HitPoints_ID1" });
            DropIndex("dbo.DamageTakens", new[] { "HitPoints_ID" });
            DropIndex("dbo.Characters", new[] { "HitPoints_ID" });
            DropColumn("dbo.Characters", "HitPoints_ID");
            DropTable("dbo.DamageTakens");
            DropTable("dbo.HitPoints");
        }
    }
}
