using System.Collections.Generic;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Classes;
using RPG.Models.RulebookModal.BaseTypes.Items;
using RPG.Models.RulebookModal.BaseTypes.Languages;
using RPG.Models.RulebookModal.BaseTypes.Races;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.Skills;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;

namespace RPG.Models.RulebookModal
{
    public class Rulebook
    {
        //TODO skal fjernes og hentes via manager
        public List<Race> Races { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<ClassBase> Classes { get; set; }
        public List<SpecialAbility> SpecialAbilities { get; set; }
        public List<Language> Languages { get; set; }
        public List<ItemMaterial> ItemMaterials { get; set; }
        public List<ItemBase> Items { get; set; }
        public List<Spell> Spells { get; set; }
        public List<Save> Saves { get; set; }

        public Rulebook()
        {
        }

        /*public Rulebook(XmlBook.XmlBook xmlbook)
        {
            Abilities = AbilityScore.Translate(xmlbook.Abilities);
            Saves = new List<Save>();
            Saves.Add(new Save(new Guid(), SaveType.Fortitude, Abilities));
            Saves.Add(new Save(new Guid(), SaveType.Reflex, Abilities));
            Saves.Add(new Save(new Guid(), SaveType.Will, Abilities));

            Data = Language.Translate(xmlbook.Laungages);
            Skills = Skill.Translate(xmlbook.Skills, Abilities);
            SpecialAbilities = SpecialAbility.Translate(xmlbook.SpecialAbilities, Abilities);
            Feats = Feat.Translate(xmlbook.Feats, Abilities);
            Proficiencies = Proficiency.Translate(xmlbook.Proficiencies);

            var specialAbilitiesBase = new List<SpecialAbility>();
            specialAbilitiesBase.AddRange(SpecialAbilities.Cast<SpecialAbility>());
            specialAbilitiesBase.AddRange(Feats.Cast<SpecialAbility>());

            
            
            var allSpecialAbilities = new List<SpecialAbility>(Feats);
            allSpecialAbilities.AddRange(SpecialAbilities);
            Classes = ClassBase.Translate(xmlbook, Abilities, Skills, allSpecialAbilities,Proficiencies);
            
            Races = Race.Translate(xmlbook.Races, Abilities, Data, specialAbilitiesBase, Classes, Skills, Proficiencies);


            Spells = Spell.Translate(xmlbook.Spells, Classes, Abilities, Skills, specialAbilitiesBase, Proficiencies, Races, Saves);

            ItemMaterials = ItemMaterial.Translate(xmlbook.ItemMaterial);
            Items = ItemBase.Translate(xmlbook.Items, specialAbilitiesBase, Abilities, ItemMaterials, Races, Classes, Skills, Proficiencies);


            SpecialAbility.EnrichWithBonuses(xmlbook.SpecialAbilities,Abilities, Data, specialAbilitiesBase, Classes, Skills, Proficiencies, Races);
            Feat.EnrichWithBonuses(xmlbook.Feats, Abilities, Data, specialAbilitiesBase, Classes, Skills, Proficiencies, Races);
            Race.EnrichWithBonuses(xmlbook.Races, Abilities, Data, specialAbilitiesBase, Classes, Skills, Proficiencies, Races);
            ItemBase.EnrichWithBonuses(xmlbook.Items, Abilities, Data, specialAbilitiesBase, Classes, Skills, Proficiencies, Races, Items);
            ItemMaterial.EnrichWithBonuses(xmlbook.ItemMaterial, Abilities, Data, specialAbilitiesBase, Classes, Skills, Proficiencies, Races, ItemMaterials);
        }*/

        /*public static Rulebook LoadRulebook()
        {
            
            var book = new Rulebook();
            book.Abilities = new List<AbilityScore>(new []
            {
                new AbilityScore(Guid.Empty,"Strength"),
                new AbilityScore(Guid.Empty,"Dexterity"),
                new AbilityScore(Guid.Empty,"Constitution"),
                new AbilityScore(Guid.Empty,"Intelligence"),
                new AbilityScore(Guid.Empty,"Wisdom"),
                new AbilityScore(Guid.Empty,"Charisma"), 
            });
            book.Races = new List<Race>();
            book.Saves = new List<Save>();
            book.Saves.Add(new Save(new Guid(), SaveType.Fortitude, book.Abilities));
            book.Saves.Add(new Save(new Guid(), SaveType.Reflex, book.Abilities));
            book.Saves.Add(new Save(new Guid(), SaveType.Will, book.Abilities));

            book.Data = new List<Language>();
            var common = new Language(Guid.Empty,"Common");
            var elven = new Language(Guid.Empty, "Elven");
            var orc = new Language(Guid.Empty, "Orc");
            book.Data.Add(common);
            book.Data.Add(elven);
            book.Data.Add(orc);

            var orcBonusLanguage = new List<Language>();
            orcBonusLanguage.Add(new Language(Guid.Empty, "Draconic"));
            orcBonusLanguage.Add(new Language(Guid.Empty, "Giant"));
            orcBonusLanguage.Add(new Language(Guid.Empty, "Gnoll"));
            orcBonusLanguage.Add(new Language(Guid.Empty, "Goblin"));
            orcBonusLanguage.Add(new Language(Guid.Empty, "Abyssal"));
            book.Data.AddRange(orcBonusLanguage);

            book.Proficiencies = new List<Proficiency>();
            var simpleWeapons = new Proficiency(Guid.Empty, "Simple Weapons");
            book.Proficiencies.Add(simpleWeapons);
            var martialWeapons = new Proficiency(Guid.Empty, "Martial Weapons");
            book.Proficiencies.Add(martialWeapons);
            var exoticWeapons = new Proficiency(Guid.Empty, "Exotic Weapons");
            book.Proficiencies.Add(exoticWeapons);
            var lightArmor = new Proficiency(Guid.Empty, "Light Armor");
            book.Proficiencies.Add(lightArmor);
            var mediumArmor = new Proficiency(Guid.Empty, "Medium Armor");
            book.Proficiencies.Add(mediumArmor);
            var heavyArmor = new Proficiency(Guid.Empty, "Heavy Armor");
            book.Proficiencies.Add(heavyArmor);
            var shields = new Proficiency(Guid.Empty, "Shields");
            book.Proficiencies.Add(shields);
            var towerShields = new Proficiency(Guid.Empty, "Tower Shield");
            book.Proficiencies.Add(towerShields);

            book.Feats = new List<Feat>();
            var uncannyDodge = new Feat(Guid.Empty, "Uncanny dodge",null);
            book.Feats.Add(uncannyDodge);

            var testFeat = new Feat(Guid.Empty, "Test feat", "testestest");
            book.Feats.Add(testFeat);


            var improvedUncannyDodge = new Feat(Guid.Empty, "Improved uncanny dodge", null);
            book.Feats.Add(improvedUncannyDodge);


            book.SpecialAbilities = new List<SpecialAbility>();
            var fastMovement = new SpecialAbility(Guid.Empty, "Fast movement", new List<Bonus>(new[] { new Bonus(Guid.Empty, "speed", new BonusToAdd(10), BonusType.NaturalBonus), }), null);
            book.SpecialAbilities.Add(fastMovement);

            var turnOrRebuke = new SpecialAbility(Guid.Empty, "Turn or rebunke undead", null, new UsableLimit(Guid.Empty, new Duration(DurationUnit.Instant, 0), new UsableAmount(RoundAction.Standard, 3, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"))), null, null);
            book.SpecialAbilities.Add(turnOrRebuke);


            var fear = new SpecialAbility(Guid.Empty, "Fear", null);
            book.SpecialAbilities.Add(fear);

            var darkvision = new SpecialAbility(Guid.Empty, "Darkvision", null);
            book.SpecialAbilities.Add(darkvision);

            var orcblood = new SpecialAbility(Guid.Empty, "Orc blood", null);
            book.SpecialAbilities.Add(orcblood);

            var illiteracy = new SpecialAbility(Guid.Empty, "Illiteracy", null);
            book.SpecialAbilities.Add(illiteracy);

            var rage = new SpecialAbility(Guid.Empty, "Rage", ConditionType.Fatigued, null);
            book.SpecialAbilities.Add(rage);


            var trapsence = new SpecialAbility(Guid.Empty, "Trap sense", null);
            book.SpecialAbilities.Add(trapsence);

            var damageReduction = new SpecialAbility(Guid.Empty, "Damage Reducion", null);
            book.SpecialAbilities.Add(damageReduction);

            var reRollDeplomacy = new SpecialAbility(Guid.Empty, "Reroll deplomacy", null, new UsableLimit(Guid.Empty, new Duration(DurationUnit.Instant, 0), new UsableAmount(RoundAction.Free, 1), TimeLimitUnit.Day), null);
            book.SpecialAbilities.Add(reRollDeplomacy);
            
            book.Skills = new List<Skill>();
            var appraise = new Skill(Guid.Empty, "Appraise", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var bluff = new Skill(Guid.Empty, "Bluff", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var consentration = new Skill(Guid.Empty, "Concentration", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_constitution"), null, null);
            //var craft = new Skill(Guid.Empty, "Craft", null, null, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null);
            var decipher = new Skill(Guid.Empty, "Decipher Script", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var disable = new Skill(Guid.Empty, "Disable Device", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var disguise = new Skill(Guid.Empty, "Disguise", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), new List<Skill>(new[] { bluff }), null);
            var escape = new Skill(Guid.Empty, "Escape Artist", true, true, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), null, null);
            var forgery = new Skill(Guid.Empty, "Forgery", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var handleAnimal = new Skill(Guid.Empty, "Handle Animal", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var heal = new Skill(Guid.Empty, "Heal", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_wisdom"), null, null);
            var hide = new Skill(Guid.Empty, "Hide", true, true, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), null, null);
            var intimidate = new Skill(Guid.Empty, "Intimidate", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), new List<Skill>(new[] { bluff }), null);
            var jump = new Skill(Guid.Empty, "Jump", true, true, false, Tools.GetAbilityFromKey(book.Abilities, "abi_strength"), null, null);
            var knowledgeArcana = new Skill(Guid.Empty, "Knowledge Arcana", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgeArchitecture = new Skill(Guid.Empty, "Knowledge Architecture and engineering", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgeDungeoneering = new Skill(Guid.Empty, "Knowledge Dungeoneering", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgeGeography = new Skill(Guid.Empty, "Knowledge Geography", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgeHistory = new Skill(Guid.Empty, "Knowledge History", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgeLocal = new Skill(Guid.Empty, "Knowledge Local", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgeNature = new Skill(Guid.Empty, "Knowledge Nature", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgeNobility = new Skill(Guid.Empty, "Knowledge Nobility and royalty", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgeReligion = new Skill(Guid.Empty, "Knowledge Religion", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var knowledgePlanes = new Skill(Guid.Empty, "Knowledge The planes", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), null, null);
            var gatherInformation = new Skill(Guid.Empty, "Gather Information", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), new List<Skill>(new[] { knowledgeLocal }), null);

            var listen = new Skill(Guid.Empty, "Listen", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_wisdom"), null, null);
            var move = new Skill(Guid.Empty, "Move Silently", true, true, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), null, null);
            var open = new Skill(Guid.Empty, "Open Lock", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), null, null);
            var performAct = new Skill(Guid.Empty, "Perform Act", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var performComedy = new Skill(Guid.Empty, "Perform Comedy", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var performDance = new Skill(Guid.Empty, "Perform Dance", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var performKeyboard = new Skill(Guid.Empty, "Perform Keyboard instruments", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var performOratory = new Skill(Guid.Empty, "Perform Oratory", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var performPercussion = new Skill(Guid.Empty, "Perform Percussion instruments", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var performString = new Skill(Guid.Empty, "Perform String instruments", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var performWind = new Skill(Guid.Empty, "Perform Wind instruments", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            var performSing = new Skill(Guid.Empty, "Perform", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), null, null);
            //var profession = new Skill(Guid.Empty, "Profession", null, null, false, Tools.GetAbilityFromKey(book.Abilities, "abi_wisdom"), null);
            var ride = new Skill(Guid.Empty, "Ride", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), new List<Skill>(new[] { handleAnimal }), null);
            var search = new Skill(Guid.Empty, "Search", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), new List<Skill>(new[] { knowledgeArchitecture }), null);
            var sense = new Skill(Guid.Empty, "Sense Motive", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_wisdom"), null, null);
            var sleightOfHand = new Skill(Guid.Empty, "Sleight of WeaponHand", false, true, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), new List<Skill>(new[] { bluff }), null);
            //var speak = new Skill(Guid.Empty, "Speak Language", null, null, false, Tools.GetAbilityFromKey(book.Abilities, "none"), null);
            var spellcraft = new Skill(Guid.Empty, "Spellcraft", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_intelligence"), new List<Skill>(new[] { knowledgeArcana }), null);
            var spot = new Skill(Guid.Empty, "Spot", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_wisdom"), null, null);
            var survival = new Skill(Guid.Empty, "Survival", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_wisdom"), new List<Skill>(new[] { knowledgeDungeoneering, knowledgeGeography, knowledgeNature, knowledgePlanes, search }), null);
            var swim = new Skill(Guid.Empty, "Swim", true, true, true, Tools.GetAbilityFromKey(book.Abilities, "abi_strength"), null, null);
            var tumble = new Skill(Guid.Empty, "Tumble", false, true, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), new List<Skill>(new[] { jump }), null);
            var balance = new Skill(Guid.Empty, "Balance", true, true, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), new List<Skill>(new[] { tumble }), null);
            var useMagicDevice = new Skill(Guid.Empty, "Use Magic Device", false, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), new List<Skill>(new[] { decipher, spellcraft }), null);
            var useRope = new Skill(Guid.Empty, "Use Rope", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), new List<Skill>(new[] { escape }), null);
            var climb = new Skill(Guid.Empty, "Climb", true, true, false, Tools.GetAbilityFromKey(book.Abilities, "abi_strength"), new List<Skill>(new[] { useRope }), null);
            var diplomacy = new Skill(Guid.Empty, "Diplomacy", true, false, false, Tools.GetAbilityFromKey(book.Abilities, "abi_charisma"), new List<Skill>(new[] { bluff, knowledgeNobility, sense }), null);
            knowledgeNature.GetsSynergiFrom = new List<Skill>(new []{survival});
            jump.GetsSynergiFrom = new List<Skill>(new[] { jump });
            escape.GetsSynergiFrom = new List<Skill>(new[] { useRope });
            spellcraft.GetsSynergiFrom.Add(useMagicDevice);
            book.Skills.AddRange(new []
            {
                appraise,balance,bluff,climb,consentration,decipher,diplomacy,disable,disguise,escape,forgery,gatherInformation,handleAnimal,heal,hide,intimidate,jump,
                listen,move,open,ride,search,sense,sleightOfHand,spellcraft,spot,survival,swim,tumble,useMagicDevice,useRope,
                knowledgeArcana, knowledgeArchitecture, knowledgeDungeoneering, knowledgeGeography, knowledgeHistory, knowledgeLocal, knowledgeNature,
                knowledgeNobility, knowledgePlanes, knowledgeReligion,
                performAct, performComedy, performDance, performKeyboard, performOratory, performPercussion, performSing, performString, performWind,
            });

            var levelAbleList = new LevelableList<SpecialAbility>();
            levelAbleList.AbilitiesByLevel.Add(1, new List<SpecialAbility>(new[] { fastMovement, illiteracy, rage }));
            levelAbleList.AbilitiesByLevel.Add(2, new List<SpecialAbility>(new[] { uncannyDodge }));
            levelAbleList.AbilitiesByLevel.Add(3, new List<SpecialAbility>(new[] { trapsence }));
            levelAbleList.AbilitiesByLevel.Add(5, new List<SpecialAbility>(new[] { improvedUncannyDodge }));
            levelAbleList.AbilitiesByLevel.Add(7, new List<SpecialAbility>(new[] { damageReduction }));
            //TODO model greater rage as new ability, model mighty rage as new ability, model Indomitable Will as new ability

            book.Classes = new List<ClassBase>();
            var barberian = new ClassBase(Guid.Empty, "Barbarian", new List<Skill>(new[] { climb,handleAnimal,intimidate,jump,listen,ride,survival,swim }), levelAbleList,
                BaseBonusRate.Good, BaseBonusRate.Poor, BaseBonusRate.Poor, BaseBonusRate.Good,4,DieType.D12, 
                new List<Alligment>(new []{Alligment.ChaoticEvil,Alligment.ChaoticGood,Alligment.ChaoticNeutral,Alligment.Neutral, Alligment.NeutralEvil,Alligment.NeutralGood }),
                new List<Proficiency>(new []{lightArmor,martialWeapons,mediumArmor,shields,simpleWeapons}),null);
            book.Classes.Add(barberian);

            
            var clericLevelAbleList = new LevelableList<SpecialAbility>();
            clericLevelAbleList.AbilitiesByLevel.Add(1, new List<SpecialAbility>(new[] { turnOrRebuke }));
            //TODO model greater rage as new ability, model mighty rage as new ability, model Indomitable Will as new ability

            var spellCastable = new List<SpellPrDay>();
            spellCastable.Add(new SpellPrDay(1,new List<SpellsPrLevel>( new []
            {
                new SpellsPrLevel(0,3), 
                new SpellsPrLevel(1,2), 
            })));
            spellCastable.Add(new SpellPrDay(2, new List<SpellsPrLevel>(new[]
            {
                new SpellsPrLevel(0,1), 
                new SpellsPrLevel(1,1), 
            })));
            spellCastable.Add(new SpellPrDay(3, new List<SpellsPrLevel>(new[]
            { 
                new SpellsPrLevel(2,2), 
            })));
            spellCastable.Add(new SpellPrDay(4, new List<SpellsPrLevel>(new[]
            {
                new SpellsPrLevel(0,1), 
                new SpellsPrLevel(1,1), 
                new SpellsPrLevel(2,1), 
            })));
            spellCastable.Add(new SpellPrDay(5, new List<SpellsPrLevel>(new[]
            { 
                new SpellsPrLevel(3,2), 
            })));
            spellCastable.Add(new SpellPrDay(6, new List<SpellsPrLevel>(new[]
            {
                new SpellsPrLevel(2,1), 
                new SpellsPrLevel(3,1), 
            })));
            spellCastable.Add(new SpellPrDay(7, new List<SpellsPrLevel>(new[]
            { 
                new SpellsPrLevel(0,1),
                new SpellsPrLevel(1,1),
                new SpellsPrLevel(4,2), 
            })));
            spellCastable.Add(new SpellPrDay(8, new List<SpellsPrLevel>(new[]
            { 
                new SpellsPrLevel(3,1),
                new SpellsPrLevel(4,1),
            })));
            spellCastable.Add(new SpellPrDay(9, new List<SpellsPrLevel>(new[]
            { 
                new SpellsPrLevel(2,1),
                new SpellsPrLevel(5,2),
            })));
            spellCastable.Add(new SpellPrDay(10, new List<SpellsPrLevel>(new[]
            { 
                new SpellsPrLevel(4,1),
                new SpellsPrLevel(5,2),
            })));

            //var cleric = new ClassBase(Guid.Empty, "Cleric", new List<Skill>(new[] { consentration, diplomacy,\/*craft,*\/heal, knowledgeArcana, knowledgeHistory, knowledgeReligion, knowledgePlanes,\/*profession,*\/spellcraft }), clericLevelAbleList,
            //    BaseBonusRate.Good, BaseBonusRate.Poor, BaseBonusRate.Good, BaseBonusRate.Average, 2, DieType.D8,
            //    new List<Alligment>(new[] { Alligment.ChaoticEvil, Alligment.ChaoticGood, Alligment.ChaoticNeutral, Alligment.Neutral, Alligment.NeutralEvil, Alligment.NeutralGood, Alligment.LawfulEvil, Alligment.LawfulGood, Alligment.LawfulNeutral}),
            //    new List<Proficiency>(new[] { lightArmor, mediumArmor, heavyArmor, shields, simpleWeapons }),new CasterLevelList(Tools.GetAbilityFromKey(book.Abilities,"abi_wisdom"),new List<SpellKnown>(new []
            //    {
            //        new SpellKnown(0,0,null),
            //        new SpellKnown(0,1,null),
            //        new SpellKnown(0,2,null),
            //        new SpellKnown(0,3,null),
            //        new SpellKnown(0,4,null),
            //        new SpellKnown(0,5,null),
            //        new SpellKnown(0,6,null),
            //        new SpellKnown(0,7,null),
            //        new SpellKnown(0,8,null), 
            //        new SpellKnown(0,9,null),
            //    }), 
            //    spellCastable));
            //book.Classes.Add(cleric);



            var limit = new UsableLimit(Guid.Empty, new Duration(DurationUnit.Rounds, 3, Tools.GetAbilityFromKey(book.Abilities, "abi_constitution")), new UsableAmount(RoundAction.Free, barberian, new int[] { 1, 4, 8, 12, 16, 20 }), TimeLimitUnit.Day);
            //rage.Limit = limit;
            rage.Bonuses.AddRange(new List<Bonus>(new[]
            {
                    new Bonus(Guid.Empty,"abi_strength",new BonusToAdd(4),BonusType.AbilityModifier,null,limit),
                    new Bonus(Guid.Empty,"abi_constitution",new BonusToAdd(4),BonusType.AbilityModifier,null,limit),
                    new Bonus(Guid.Empty,"save_will",new BonusToAdd(2),BonusType.MoraleModifier,null,limit),
                    new Bonus(Guid.Empty,"ac",new BonusToAdd(-2),BonusType.ArmorBonus,null,limit),
            }));

            uncannyDodge.Bonuses.Add(new Bonus(Guid.Empty, "ac_flatfooted", new BonusToAdd(Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity")), BonusType.AbilityModifier, new List<Condition>(new[] { new Condition(Tools.GetAbilityFromKey(book.Abilities, "abi_dexterity"), ConditionPretens.LargerThenZero), })));

            trapsence.Bonuses.AddRange(new List<Bonus>(new[]
            {
                    new Bonus(Guid.Empty,"armorclass",new BonusToAdd(barberian,new int[]{3,6,9,12,15,18}),BonusType.DodgeBonus, new List<Condition>(new []{new Condition(true,ConditionPretens.WhenAttackBy)})),
                    new Bonus(Guid.Empty,"save_reflex",new BonusToAdd(barberian,new int[]{3,6,9,12,15,18}),BonusType.MoraleModifier,new List<Condition>(new []{new Condition(true,ConditionPretens.WhenAttackBy)})),
            }));

            damageReduction.Bonuses.AddRange(new List<Bonus>(new[]
            {
                    new Bonus(Guid.Empty,"damage_reduction",new BonusToAdd(barberian,new int[]{7,10,13,16,19},DamageType.AllWeapons),BonusType.NaturalBonus),
            }));
            var ageDictonary = new Dictionary<AgeCategory, int>();
            ageDictonary.Add(AgeCategory.MiddleAge,30);
            ageDictonary.Add(AgeCategory.Old, 45);
            ageDictonary.Add(AgeCategory.Venerable, 60);

            book.Races.Add(new Race(Guid.Empty,"Half Orc", new List<Bonus>(new[] { 
                new Bonus(Guid.Empty,"abi_strength", new BonusToAdd(2), BonusType.RacialBonus), 
                new Bonus(Guid.Empty,"abi_intelligence", new BonusToAdd(-2), BonusType.RacialBonus),
                new Bonus(Guid.Empty,"abi_charisma", new BonusToAdd(-2), BonusType.RacialBonus),

                new Bonus(Guid.Empty,"specialability_darkvision",null, BonusType.RacialBonus),
                new Bonus(Guid.Empty,"specialability_orcblood",null, BonusType.RacialBonus),
            }), SizeCategory.Medium, 
            new List<Language>(new []{common,orc}), 
            30,
            orcBonusLanguage,
            new List<ClassBase>(new[] { barberian }),
            ageDictonary));
            

            book.ItemMaterials = new List<ItemMaterial>();
            book.ItemMaterials.Add(new ItemMaterial(Guid.Empty,"Darkleaf",new List<Bonus>(new []
            {
                new Bonus(Guid.Empty,"item_arcane_spell_failure",new BonusToAdd(-5),BonusType.MaterialBonus ),
                new Bonus(Guid.Empty,"item_max_dex",new BonusToAdd(1),BonusType.MaterialBonus ),
                new Bonus(Guid.Empty,"item_armor_check_penalty",new BonusToAdd(2),BonusType.MaterialBonus ),
                new Bonus(Guid.Empty,"item_max_speed",new BonusToAdd(10),BonusType.MaterialBonus ),
            }),new List<Bonus>()));



            

            //book.Items = new List<ItemBase>();
            //var longsword = new ItemBase(Guid.Empty, "Longsword", null, ItemType.Weapon, 4,
            //    new List<ItemSlotRequirement>(new[] {ItemSlotRequirement.WeaponHand}),
            //    new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.WeaponCrystal }), null, null, true,
            //    new Damage(Guid.Empty,new DiceRoll(Guid.Empty,0, new List<Die>(new[] {new Die(Guid.Empty,1, DieType.D8),})), DamageType.Slashing),
            //    2,
            //    false,
            //    0,
            //    true,
            //    null,
            //    null,
            //    null,
            //    null,
            //    null,
            //    null,
            //    null,
            //    null, null,);
            //book.Items.Add(longsword);


            //book.Items.Add(new ItemBase(Guid.Empty, "Merthuvial Longsword",longsword, ItemType.Weapon, 4, new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.WeaponHand }),
            //   new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.WeaponCrystal }), new List<SpecialAbility>(new[] { reRollDeplomacy }), null, true,
            //   new Damage(Guid.Empty,new DiceRoll(Guid.Empty, 0, new List<Die>(new[] { new Die(Guid.Empty, 1, DieType.D8), })), DamageType.Slashing),
            //   2,
            //   false,
            //   0,
            //   true,
            //   null,
            //   null,
            //   null,
            //   null,
            //   null,
            //   true,
            //   1,
            //   null, null));


            //var breastPlate = new ItemBase(Guid.Empty, "Breastplate", null, ItemType.Armor, 50,
            //    new List<ItemSlotRequirement>(new[] {ItemSlotRequirement.Body}),
            //    new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.ArmorCrystal }), null, null, null, null, null, null,
            //    null, null, -4, 5, 3, 20, 25, null, null, null, null);
            //book.Items.Add(breastPlate);

            //var ringOfDeflection = new ItemBase(Guid.Empty, "Ring of deflection", null, ItemType.Other, 0,
            //    new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.Ring }), null, null, new List<Bonus>(new[] { new Bonus(Guid.Empty, "ac", new BonusToAdd(2), BonusType.DeflectionBonus), }), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            //book.Items.Add(ringOfDeflection);

            //var gauntles = new ItemBase(Guid.Empty, "Gauntlets", null, ItemType.Other, 1,
            //    new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.Hands }), null, null, null, true, new Damage(Guid.Empty,new DiceRoll(Guid.Empty, 0, new List<Die>(new[] { new Die(Guid.Empty, 1, DieType.D3) })), DamageType.Bludgeoning), 1, false, null, null, null, null, null, null, null, null, null, null, null);
            //book.Items.Add(gauntles);


            //var gauntlesOfOgrePower = new ItemBase(Guid.Empty, "Gauntlets of Ogre Power", gauntles, ItemType.Other, 1,
            //    new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.Hands }), null, null, new List<Bonus>(new[] { new Bonus(Guid.Empty, "abi_strength", new BonusToAdd(2), BonusType.EnhancementBonus), }), true, new Damage(Guid.Empty,new DiceRoll(Guid.Empty, 0, new List<Die>(new[] { new Die(Guid.Empty, 1, DieType.D3) })), DamageType.Bludgeoning), 1, false, null, null, null, null, null, null, null, null, null, null, null);
            //book.Items.Add(gauntlesOfOgrePower);

            //var enlarge10r = new SpecialAbility(Guid.Empty, "Enlarge",
            //    new List<Bonus>(new[]
            //    {
            //        new Bonus(Guid.Empty, "size", new BonusToAdd(1),BonusType.EnhancementBonus),
            //        new Bonus(Guid.Empty, "abi_strength", new BonusToAdd(2),BonusType.SizeModifier),
            //        new Bonus(Guid.Empty, "abi_dexterity", new BonusToAdd(2),BonusType.SizeModifier),
            //    }),
            //    new UsableLimit(Guid.Empty, new Duration(DurationUnit.Rounds, 10), new UsableAmount(RoundAction.Standard, 3), TimeLimitUnit.Day), null);
            //book.SpecialAbilities.Add(enlarge10r);

            //var necklaceOfEnlargePerson = new ItemBase(Guid.Empty, "Necklace Of Enlarge Person", null, ItemType.Other, 0,
            //    new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.Throat }), null, new List<SpecialAbility>(new[] { enlarge10r }), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "This necklace can increase a persons size på one size category for 10 rounds using a standard action. When increased in size it provides +2str and +2dex.");
            //book.Items.Add(necklaceOfEnlargePerson);
            
            //var acrobatbootcharges = new Dictionary<int, DiceRoll>();
            //acrobatbootcharges.Add(1, new DiceRoll(Guid.Empty, 10, new List<Die>()));
            //acrobatbootcharges.Add(2, new DiceRoll(Guid.Empty, 20, new List<Die>()));
            //acrobatbootcharges.Add(3, new DiceRoll(Guid.Empty, 30, new List<Die>()));
            //var extraMoveCharges = new SpecialAbility(Guid.Empty, "Acrobat boots", new List<Bonus>(new[] { new Bonus(Guid.Empty, "speed", new BonusToAdd(0), BonusType.MoveAction) }), new UsableLimit(Guid.Empty, new Duration(DurationUnit.Rounds, 1), new UsableAmount(RoundAction.Free, 3), TimeLimitUnit.Day), null, acrobatbootcharges, "boots boots boots");
            
            //var acrobatBoots = new ItemBase(Guid.Empty, "Acrobat boots", null, ItemType.Other, 1,
            //    new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.Feet }), null, new List<SpecialAbility>(new[] { extraMoveCharges }), new List<Bonus>(new[] { new Bonus(Guid.Empty, "skill_tumble", new BonusToAdd(2), BonusType.EnhancementBonus) }), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            //book.Items.Add(acrobatBoots);

            //book.Items.Add(new ItemBase(Guid.Empty, "testmish",null,ItemType.Other, 0,new List<ItemSlotRequirement>(new []{ItemSlotRequirement.Misc, }),null,null,null,false,null,null,null,null,null,null,null,null,null,null,null,null,null,"Test item"));

            //var curelightwounds = new ItemBase(Guid.Empty, "Cure Light Wounds Potion", null, ItemType.Other, 1, new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.Misc }), null, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null, null, "Heals a character for 1d8+1.");
            //book.Items.Add(curelightwounds);

            //
            //var steamOfBlood = new SpecialAbility(Guid.Empty, "Stream of blood",
            //    new List<Bonus>(new[] { new Bonus(Guid.Empty, "item_mask_of_blood", new BonusToAdd(new List<Die>(new[] { new Die(4, DieType.D6), })), BonusType.WeaponDamage), }),
            //    new UsableLimit(new Duration(DurationUnit.Rounds, 1), new UsableAmount(RoundAction.Immediate, 2), TimeLimitUnit.Day));
            //book.SpecialAbilities.Add(steamOfBlood);
            
            // * var maskOfBlood = new ItemBase(Guid.Empty, "Mask of Blood", null, ItemType.Other, 1,
            //    new List<ItemSlotRequirement>(new[] { ItemSlotRequirement.Face }), null, new List<SpecialAbility>(new[] { steamOfBlood }), true, new Damage(new DiceRoll(0,new List<Die>()),DamageType.Acid), null, true, 30, null, null, null, null, null, null, null, null, null);
            //book.Items.Add(maskOfBlood);

            
            //book.Spells = new List<Spell>();
            //var clwByCasterLevel = new Dictionary<int, DiceRoll>();
            //clwByCasterLevel.Add(1, new DiceRoll(Guid.Empty, 1, new List<Die>(new[] { new Die(Guid.Empty, 1, DieType.D8), })));
            //clwByCasterLevel.Add(2, new DiceRoll(Guid.Empty, 2, new List<Die>(new[] { new Die(Guid.Empty, 2, DieType.D8), })));
            //clwByCasterLevel.Add(3, new DiceRoll(Guid.Empty, 3, new List<Die>(new[] { new Die(Guid.Empty, 3, DieType.D8), })));
            //clwByCasterLevel.Add(4, new DiceRoll(Guid.Empty, 4, new List<Die>(new[] { new Die(Guid.Empty, 4, DieType.D8), })));
            //clwByCasterLevel.Add(5, new DiceRoll(Guid.Empty, 5, new List<Die>(new[] { new Die(Guid.Empty, 5, DieType.D8), })));
            //book.Spells.Add(new Spell(Guid.Empty, "Magic Missile", new List<SpellRequiretLevel>(new[] { new SpellRequiretLevel(Guid.Empty, 1, cleric) }), new List<SpellComponent>(new[] { SpellComponent.Verbal, SpellComponent.Somatic }), null, SpellSchool.Conjuration, null,
            //    CastingTime.Standard, SpellBaseRange.Touch, null, new UsableLimit(Guid.Empty, new Duration(DurationUnit.Instant, 0)), new SpellDamage(Guid.Empty, clwByCasterLevel, new Damage(Guid.Empty, null, DamageType.PositiveEnergy)),
            //    "Max 15-ft apart", null, false,
            //    "A missile of magical energy darts forth from your fingertip and strikes its target, dealing 1d4+1 points of force damage. The missile strikes unerringly, even if the target is in melee combat or has less than total cover or total concealment. Specific parts of a creature can’t be singled out. Inanimate objects are not damaged by the spell. For every two caster levels beyond 1st, you gain an additional missile—two at 3rd level, three at 5th, four at 7th, and the maximum of five missiles at 9th level or higher. If you shoot multiple missiles, you can have them strike a single creature or several creatures. A single missile can strike only one creature. You must designate targets before you check for spell resistance or roll damage."));

            //book.Spells.Add(new Spell(Guid.Empty, "test spell", new List<SpellRequiretLevel>(new[] { new SpellRequiretLevel(Guid.Empty, 1, cleric) }), new List<SpellComponent>(new[] { SpellComponent.Verbal, SpellComponent.Somatic }), new List<Bonus>(new[] { new Bonus(Guid.Empty, "abi_strength", new BonusToAdd(4), BonusType.EnhancementBonus, null, new UsableLimit(Guid.Empty, new Duration(DurationUnit.Rounds, 10))), }), SpellSchool.Conjuration, null,
            //    CastingTime.Standard, SpellBaseRange.Touch, null, new UsableLimit(Guid.Empty, new Duration(DurationUnit.Instant, 0)), new SpellDamage(Guid.Empty, clwByCasterLevel, new Damage(Guid.Empty, null, DamageType.PositiveEnergy)),
            //    "Touch", null, false,
            //    "+4 str"));
            

            return book;
        }*/

        /*public List<SpecialAbility> GetSpecialAbilityBaseObjects()
        {
            var specialAbilitiesBase = new List<SpecialAbility>();
            specialAbilitiesBase.AddRange(SpecialAbilities.Cast<SpecialAbility>());
            specialAbilitiesBase.AddRange(Feats.Cast<SpecialAbility>());
            return specialAbilitiesBase;
        }

        public List<ItemBase> GetAllItems()
        {
            return Items;
        }*/
    }
}
