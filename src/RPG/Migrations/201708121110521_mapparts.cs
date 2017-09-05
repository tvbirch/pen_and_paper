namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapparts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapParts",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Z = c.Int(nullable: false),
                        Data = c.Binary(),
                        Map_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Maps", t => t.Map_ID)
                .Index(t => t.Map_ID);
            
            CreateTable(
                "dbo.Maps",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MapParts", "Map_ID", "dbo.Maps");
            DropIndex("dbo.MapParts", new[] { "Map_ID" });
            DropTable("dbo.Maps");
            DropTable("dbo.MapParts");
        }
    }
}
