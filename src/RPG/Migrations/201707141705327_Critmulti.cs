namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Critmulti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemBases", "CriticalMultiplier", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemBases", "CriticalMultiplier");
        }
    }
}
