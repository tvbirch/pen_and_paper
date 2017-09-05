namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsableAmounts", "ProvokesAttackOfOppertunity", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsableAmounts", "ProvokesAttackOfOppertunity");
        }
    }
}
