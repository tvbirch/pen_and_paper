namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class skillReqMovement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skills", "RequiresMovement", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skills", "RequiresMovement");
        }
    }
}
