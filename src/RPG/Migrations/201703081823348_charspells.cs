namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class charspells : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SpellSpellComponents", newName: "SpellComponentSpells");
            DropPrimaryKey("dbo.SpellComponentSpells");
            CreateTable(
                "dbo.SpellSlots",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Used = c.Boolean(nullable: false),
                        Spell_ID = c.Guid(),
                        SpellChosenForClass_ID = c.Guid(),
                        Character_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Spells", t => t.Spell_ID)
                .ForeignKey("dbo.ClassBases", t => t.SpellChosenForClass_ID)
                .ForeignKey("dbo.Characters", t => t.Character_ID)
                .Index(t => t.Spell_ID)
                .Index(t => t.SpellChosenForClass_ID)
                .Index(t => t.Character_ID);
            
            AddPrimaryKey("dbo.SpellComponentSpells", new[] { "SpellComponent_ID", "Spell_ID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpellSlots", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.SpellSlots", "SpellChosenForClass_ID", "dbo.ClassBases");
            DropForeignKey("dbo.SpellSlots", "Spell_ID", "dbo.Spells");
            DropIndex("dbo.SpellSlots", new[] { "Character_ID" });
            DropIndex("dbo.SpellSlots", new[] { "SpellChosenForClass_ID" });
            DropIndex("dbo.SpellSlots", new[] { "Spell_ID" });
            DropPrimaryKey("dbo.SpellComponentSpells");
            DropTable("dbo.SpellSlots");
            AddPrimaryKey("dbo.SpellComponentSpells", new[] { "Spell_ID", "SpellComponent_ID" });
            RenameTable(name: "dbo.SpellComponentSpells", newName: "SpellSpellComponents");
        }
    }
}
