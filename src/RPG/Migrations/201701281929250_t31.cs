namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonusToAddClassProgressions", "Bonues_ID", c => c.Guid());
            CreateIndex("dbo.BonusToAddClassProgressions", "Bonues_ID");
            AddForeignKey("dbo.BonusToAddClassProgressions", "Bonues_ID", "dbo.DiceRolls", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonusToAddClassProgressions", "Bonues_ID", "dbo.DiceRolls");
            DropIndex("dbo.BonusToAddClassProgressions", new[] { "Bonues_ID" });
            DropColumn("dbo.BonusToAddClassProgressions", "Bonues_ID");
        }
    }
}
