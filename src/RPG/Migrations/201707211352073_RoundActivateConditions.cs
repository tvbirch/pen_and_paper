namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoundActivateConditions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoundActivateConditions",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        AutoDismissAfter = c.Int(),
                        ManuallyActivated = c.Boolean(),
                        ActivitedByAbility = c.Guid(),
                        Condition = c.Guid(nullable: false),
                        Round_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Rounds", t => t.Round_ID)
                .Index(t => t.Round_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoundActivateConditions", "Round_ID", "dbo.Rounds");
            DropIndex("dbo.RoundActivateConditions", new[] { "Round_ID" });
            DropTable("dbo.RoundActivateConditions");
        }
    }
}
