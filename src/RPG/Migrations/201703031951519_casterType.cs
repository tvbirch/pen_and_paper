namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class casterType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassBases", "ArcaneCaster", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClassBases", "DivineCaster", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassBases", "DivineCaster");
            DropColumn("dbo.ClassBases", "ArcaneCaster");
        }
    }
}
