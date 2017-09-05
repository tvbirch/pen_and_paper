namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dies", "Operator", c => c.Int());
            AlterColumn("dbo.Dies", "NumberOfDice", c => c.Int());
            AlterColumn("dbo.Dies", "DieType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dies", "DieType", c => c.Int(nullable: false));
            AlterColumn("dbo.Dies", "NumberOfDice", c => c.Int(nullable: false));
            DropColumn("dbo.Dies", "Operator");
        }
    }
}
