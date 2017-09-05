using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances;
using RPG.Models.RulebookModal.BaseTypes.Races;


namespace RPG.Models.CharacterModal.Parts
{
    [ComplexType]
    public class PhysicalAppearance
    {
        public ImperialDistance Height { get; set; }
        public ImperialWeight Weight { get; set; }
        public Age Age { get; set; }
        public string Gender { get; set; }
        public string Eyes { get; set; }
        public string Hair { get; set; }
        public string Skin { get; set; }


        //public PhysicalAppearance(List<Ability> abilities, Race race, int age, double heightInInches, double weightInLb, string gender, string eyes, string hair, string skin)
        //{
        //    //Set race
        //    Race = race;
        //    //Set size
        //    Size = new Size(Race.Size);
        //    //Set age;
        //    Age = new Age(abilities,race,age);
        //    //set Height
        //    Height = new ImperialDistance(heightInInches);
        //    //set Weight
        //    Weight = new ImperialWeight(weightInLb);
        //    //Speed
        //    Speed = new Speed(race);

        //    Gender = gender;
        //    Eyes = eyes;
        //    Hair = hair;
        //    Skin = skin;
        //}
        public void EnrichPassiveBonusDto(GetBonusDto dto)
        {
            foreach (var bonuse in Age.GetAgeBonuses(dto.Character.Race, dto.Abilities))
            {
                dto.PassiveBonus.Add(new BonusRef(new ElementId{Name = "Age"}, bonuse));
            }
            //dto.PassiveBonus.AddRange(Age.GetAgeBonuses(dto.Race,dto.Abilities));
        }
    }
}
