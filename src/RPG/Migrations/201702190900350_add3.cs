namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeLimitUnitParseds", "Time_ID", "dbo.TimeLimitUnitParseds");
            DropIndex("dbo.TimeLimitUnitParseds", new[] { "Time_ID" });
            AddColumn("dbo.TimeLimitUnitParseds", "Time", c => c.Int(nullable: false));
            DropColumn("dbo.TimeLimitUnitParseds", "Time_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TimeLimitUnitParseds", "Time_ID", c => c.Guid());
            DropColumn("dbo.TimeLimitUnitParseds", "Time");
            CreateIndex("dbo.TimeLimitUnitParseds", "Time_ID");
            AddForeignKey("dbo.TimeLimitUnitParseds", "Time_ID", "dbo.TimeLimitUnitParseds", "ID");
        }
    }
}
