namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conditions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        AutoDismissAfter = c.Int(),
                        Name = c.String(nullable: false, maxLength: 250),
                        Description = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.SpecialAbilities", "Condition_ID", c => c.Guid());
            AddColumn("dbo.SpecialAbilities", "ApplyConditionOnActivate_ID", c => c.Guid());
            AddColumn("dbo.SpecialAbilities", "ApplyConditionOnDeactivate_ID", c => c.Guid());
            AddColumn("dbo.Bonus", "Condition_ID", c => c.Guid());
            CreateIndex("dbo.SpecialAbilities", "Condition_ID");
            CreateIndex("dbo.SpecialAbilities", "ApplyConditionOnActivate_ID");
            CreateIndex("dbo.SpecialAbilities", "ApplyConditionOnDeactivate_ID");
            CreateIndex("dbo.Bonus", "Condition_ID");
            AddForeignKey("dbo.Bonus", "Condition_ID", "dbo.Conditions", "ID");
            AddForeignKey("dbo.SpecialAbilities", "Condition_ID", "dbo.Conditions", "ID");
            AddForeignKey("dbo.SpecialAbilities", "ApplyConditionOnActivate_ID", "dbo.Conditions", "ID");
            AddForeignKey("dbo.SpecialAbilities", "ApplyConditionOnDeactivate_ID", "dbo.Conditions", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpecialAbilities", "ApplyConditionOnDeactivate_ID", "dbo.Conditions");
            DropForeignKey("dbo.SpecialAbilities", "ApplyConditionOnActivate_ID", "dbo.Conditions");
            DropForeignKey("dbo.SpecialAbilities", "Condition_ID", "dbo.Conditions");
            DropForeignKey("dbo.Bonus", "Condition_ID", "dbo.Conditions");
            DropIndex("dbo.Bonus", new[] { "Condition_ID" });
            DropIndex("dbo.SpecialAbilities", new[] { "ApplyConditionOnDeactivate_ID" });
            DropIndex("dbo.SpecialAbilities", new[] { "ApplyConditionOnActivate_ID" });
            DropIndex("dbo.SpecialAbilities", new[] { "Condition_ID" });
            DropColumn("dbo.Bonus", "Condition_ID");
            DropColumn("dbo.SpecialAbilities", "ApplyConditionOnDeactivate_ID");
            DropColumn("dbo.SpecialAbilities", "ApplyConditionOnActivate_ID");
            DropColumn("dbo.SpecialAbilities", "Condition_ID");
            DropTable("dbo.Conditions");
        }
    }
}
