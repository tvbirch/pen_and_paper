namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Conditions2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conditions", "IfAlreadyActiveApplyCondition_ID", c => c.Guid());
            CreateIndex("dbo.Conditions", "IfAlreadyActiveApplyCondition_ID");
            AddForeignKey("dbo.Conditions", "IfAlreadyActiveApplyCondition_ID", "dbo.Conditions", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conditions", "IfAlreadyActiveApplyCondition_ID", "dbo.Conditions");
            DropIndex("dbo.Conditions", new[] { "IfAlreadyActiveApplyCondition_ID" });
            DropColumn("dbo.Conditions", "IfAlreadyActiveApplyCondition_ID");
        }
    }
}
