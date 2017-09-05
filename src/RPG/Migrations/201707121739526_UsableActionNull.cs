namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsableActionNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UsableAmounts", "ActionRequired", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsableAmounts", "ActionRequired", c => c.Int(nullable: false));
        }
    }
}
