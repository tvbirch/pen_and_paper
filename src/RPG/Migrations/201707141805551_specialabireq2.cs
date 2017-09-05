namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class specialabireq2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecialAbilities", "RequiresSpecialAbilityActive", c => c.Guid());
            AddColumn("dbo.SpecialAbilities", "RequiredNumberOfCharges", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpecialAbilities", "RequiredNumberOfCharges");
            DropColumn("dbo.SpecialAbilities", "RequiresSpecialAbilityActive");
        }
    }
}
