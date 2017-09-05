namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gmviewchar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GmCharacterViews",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        CharacterGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GmCharacterViews");
        }
    }
}
