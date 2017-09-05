namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NPCs6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Histories", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Histories", "Created");
        }
    }
}
