namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoundActivateAbilities",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        AbilityId = c.Guid(nullable: false),
                        Round_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Rounds", t => t.Round_ID)
                .Index(t => t.Round_ID);
            
            CreateTable(
                "dbo.TimeLimitUnitParseds",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Time_ID = c.Guid(),
                        RoundActivateAbilities_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TimeLimitUnitParseds", t => t.Time_ID)
                .ForeignKey("dbo.RoundActivateAbilities", t => t.RoundActivateAbilities_ID)
                .Index(t => t.Time_ID)
                .Index(t => t.RoundActivateAbilities_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoundActivateAbilities", "Round_ID", "dbo.Rounds");
            DropForeignKey("dbo.TimeLimitUnitParseds", "RoundActivateAbilities_ID", "dbo.RoundActivateAbilities");
            DropForeignKey("dbo.TimeLimitUnitParseds", "Time_ID", "dbo.TimeLimitUnitParseds");
            DropIndex("dbo.TimeLimitUnitParseds", new[] { "RoundActivateAbilities_ID" });
            DropIndex("dbo.TimeLimitUnitParseds", new[] { "Time_ID" });
            DropIndex("dbo.RoundActivateAbilities", new[] { "Round_ID" });
            DropTable("dbo.TimeLimitUnitParseds");
            DropTable("dbo.RoundActivateAbilities");
        }
    }
}
