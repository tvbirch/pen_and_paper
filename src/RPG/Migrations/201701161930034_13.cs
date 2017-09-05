namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bonus", "ApplyTo_ApplyToCondition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bonus", "ApplyTo_ApplyToCondition");
        }
    }
}
