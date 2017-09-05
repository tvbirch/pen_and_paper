namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AgeCategoryAtAges", "Race_ID", "dbo.Races");
            DropIndex("dbo.AgeCategoryAtAges", new[] { "Race_ID" });
            AlterColumn("dbo.AgeCategoryAtAges", "Race_ID", c => c.Guid(nullable: false));
            CreateIndex("dbo.AgeCategoryAtAges", "Race_ID");
            AddForeignKey("dbo.AgeCategoryAtAges", "Race_ID", "dbo.Races", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AgeCategoryAtAges", "Race_ID", "dbo.Races");
            DropIndex("dbo.AgeCategoryAtAges", new[] { "Race_ID" });
            AlterColumn("dbo.AgeCategoryAtAges", "Race_ID", c => c.Guid());
            CreateIndex("dbo.AgeCategoryAtAges", "Race_ID");
            AddForeignKey("dbo.AgeCategoryAtAges", "Race_ID", "dbo.Races", "ID");
        }
    }
}
