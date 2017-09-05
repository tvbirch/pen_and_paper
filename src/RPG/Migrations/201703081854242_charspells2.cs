namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class charspells2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Spells", "Duration_ID", "dbo.UsableLimits");
            DropForeignKey("dbo.Bonus", "Spell_ID", "dbo.Spells");
            DropIndex("dbo.Bonus", new[] { "Spell_ID" });
            DropIndex("dbo.Spells", new[] { "Duration_ID" });
            AddColumn("dbo.Spells", "SpellAbility_ID", c => c.Guid());
            CreateIndex("dbo.Spells", "SpellAbility_ID");
            AddForeignKey("dbo.Spells", "SpellAbility_ID", "dbo.SpecialAbilities", "ID");
            DropColumn("dbo.Bonus", "Spell_ID");
            DropColumn("dbo.Spells", "Duration_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Spells", "Duration_ID", c => c.Guid());
            AddColumn("dbo.Bonus", "Spell_ID", c => c.Guid());
            DropForeignKey("dbo.Spells", "SpellAbility_ID", "dbo.SpecialAbilities");
            DropIndex("dbo.Spells", new[] { "SpellAbility_ID" });
            DropColumn("dbo.Spells", "SpellAbility_ID");
            CreateIndex("dbo.Spells", "Duration_ID");
            CreateIndex("dbo.Bonus", "Spell_ID");
            AddForeignKey("dbo.Bonus", "Spell_ID", "dbo.Spells", "ID");
            AddForeignKey("dbo.Spells", "Duration_ID", "dbo.UsableLimits", "ID");
        }
    }
}
