namespace RPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NPCs4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Abilities", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.SpecialAbilities", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Conditions", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Bonus", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.ItemBases", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.ItemMaterials", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.MaterialBonuses", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Races", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Languages", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Characters", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Alligments", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.ClassBases", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Skills", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.SkillSynergis", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.SaveTypes", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.NPCs", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Locations", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Spells", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.SpellComponents", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.SpellDescriptors", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.SpellSchools", "Name", c => c.String(maxLength: 250));
            AlterColumn("dbo.Histories", "Name", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Histories", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.SpellSchools", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.SpellDescriptors", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.SpellComponents", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Spells", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Locations", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.NPCs", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.SaveTypes", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.SkillSynergis", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Skills", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.ClassBases", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Alligments", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Characters", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Languages", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Races", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.MaterialBonuses", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.ItemMaterials", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.ItemBases", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Bonus", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Conditions", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.SpecialAbilities", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Abilities", "Name", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
