namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoundActivateAbilities", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoundActivateAbilities", "IsActive");
        }
    }
}
