namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SaveRates", name: "ClassBase_ID", newName: "Class_ID");
            RenameIndex(table: "dbo.SaveRates", name: "IX_ClassBase_ID", newName: "IX_Class_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SaveRates", name: "IX_Class_ID", newName: "IX_ClassBase_ID");
            RenameColumn(table: "dbo.SaveRates", name: "Class_ID", newName: "ClassBase_ID");
        }
    }
}
