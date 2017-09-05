namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itemOwnScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemBases", "UseItemsOwnAbilistyScore", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemBases", "UseItemsOwnAbilistyScore");
        }
    }
}
