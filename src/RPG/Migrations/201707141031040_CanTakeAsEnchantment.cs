namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CanTakeAsEnchantment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecialAbilities", "CanTakeAsEnchantment", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bonus", "CanTakeAsEnchantment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bonus", "CanTakeAsEnchantment");
            DropColumn("dbo.SpecialAbilities", "CanTakeAsEnchantment");
        }
    }
}
