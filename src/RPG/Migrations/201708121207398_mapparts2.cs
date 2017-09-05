namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapparts2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MapParts", "FileType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MapParts", "FileType");
        }
    }
}
