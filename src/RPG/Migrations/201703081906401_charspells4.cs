namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class charspells4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Spells", "SpellSaveType_ID", c => c.Guid());
            CreateIndex("dbo.Spells", "SpellSaveType_ID");
            AddForeignKey("dbo.Spells", "SpellSaveType_ID", "dbo.SaveTypes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Spells", "SpellSaveType_ID", "dbo.SaveTypes");
            DropIndex("dbo.Spells", new[] { "SpellSaveType_ID" });
            DropColumn("dbo.Spells", "SpellSaveType_ID");
        }
    }
}
