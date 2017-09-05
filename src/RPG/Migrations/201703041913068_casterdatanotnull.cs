namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class casterdatanotnull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpellsKnownPrLevels", "NumberOfSpells", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SpellsKnownPrLevels", "NumberOfSpells", c => c.Int());
        }
    }
}
