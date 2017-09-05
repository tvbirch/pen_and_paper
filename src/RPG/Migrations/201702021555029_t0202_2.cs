namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t0202_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rounds",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RoundActionTakens",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Action = c.Int(nullable: false),
                        ActionUsedBy = c.Guid(),
                        Round_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Rounds", t => t.Round_ID)
                .Index(t => t.Round_ID);
            
            AddColumn("dbo.Characters", "Round_ID", c => c.Guid());
            CreateIndex("dbo.Characters", "Round_ID");
            AddForeignKey("dbo.Characters", "Round_ID", "dbo.Rounds", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "Round_ID", "dbo.Rounds");
            DropForeignKey("dbo.RoundActionTakens", "Round_ID", "dbo.Rounds");
            DropIndex("dbo.RoundActionTakens", new[] { "Round_ID" });
            DropIndex("dbo.Characters", new[] { "Round_ID" });
            DropColumn("dbo.Characters", "Round_ID");
            DropTable("dbo.RoundActionTakens");
            DropTable("dbo.Rounds");
        }
    }
}
