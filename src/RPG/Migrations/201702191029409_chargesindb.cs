namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chargesindb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoundActivateAbilities", "Charges", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoundActivateAbilities", "Charges");
        }
    }
}
