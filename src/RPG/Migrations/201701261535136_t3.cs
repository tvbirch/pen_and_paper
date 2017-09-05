namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CharacterSpecialAbilities",
                c => new
                    {
                        Character_ID = c.Guid(nullable: false),
                        SpecialAbility_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Character_ID, t.SpecialAbility_ID })
                .ForeignKey("dbo.Characters", t => t.Character_ID, cascadeDelete: true)
                .ForeignKey("dbo.SpecialAbilities", t => t.SpecialAbility_ID, cascadeDelete: true)
                .Index(t => t.Character_ID)
                .Index(t => t.SpecialAbility_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CharacterSpecialAbilities", "SpecialAbility_ID", "dbo.SpecialAbilities");
            DropForeignKey("dbo.CharacterSpecialAbilities", "Character_ID", "dbo.Characters");
            DropIndex("dbo.CharacterSpecialAbilities", new[] { "SpecialAbility_ID" });
            DropIndex("dbo.CharacterSpecialAbilities", new[] { "Character_ID" });
            DropTable("dbo.CharacterSpecialAbilities");
        }
    }
}
