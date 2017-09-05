namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AbilityByClassLevels", "ClassBase_ID", "dbo.ClassBases");
            DropIndex("dbo.AbilityByClassLevels", new[] { "ClassBase_ID" });
            RenameColumn(table: "dbo.AbilityByClassLevels", name: "ClassBase_ID", newName: "Class_ID");
            AlterColumn("dbo.AbilityByClassLevels", "Class_ID", c => c.Guid(nullable: false));
            CreateIndex("dbo.AbilityByClassLevels", "Class_ID");
            AddForeignKey("dbo.AbilityByClassLevels", "Class_ID", "dbo.ClassBases", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbilityByClassLevels", "Class_ID", "dbo.ClassBases");
            DropIndex("dbo.AbilityByClassLevels", new[] { "Class_ID" });
            AlterColumn("dbo.AbilityByClassLevels", "Class_ID", c => c.Guid());
            RenameColumn(table: "dbo.AbilityByClassLevels", name: "Class_ID", newName: "ClassBase_ID");
            CreateIndex("dbo.AbilityByClassLevels", "ClassBase_ID");
            AddForeignKey("dbo.AbilityByClassLevels", "ClassBase_ID", "dbo.ClassBases", "ID");
        }
    }
}
