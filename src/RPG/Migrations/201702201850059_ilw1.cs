namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ilw1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemBases", "IsLightWeapon", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemBases", "IsLightWeapon");
        }
    }
}
