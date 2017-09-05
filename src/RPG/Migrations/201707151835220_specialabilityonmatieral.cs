namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class specialabilityonmatieral : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaterialBonuses", "RequiredAbility", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MaterialBonuses", "RequiredAbility");
        }
    }
}
