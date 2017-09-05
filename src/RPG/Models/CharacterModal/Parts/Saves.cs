using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RPG.Models.CharacterModal.HelperDtos;
using RPG.Models.RulebookModal;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;
using RPG.Models.RulebookModal.BaseTypes.Saves;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Time;
using RPG.Models.RulebookModal.Interfaces;
using RPG.Models.RulebookModal.Lists;


namespace RPG.Models.CharacterModal.Parts
{
    public static class Saves
    {
        public static List<Save> GetSaves(GetBonusDto bonusDto)
        {
            var characterSaves = new List<Save>();
            //Adding saves and seeting base save
            foreach (var classLevel in bonusDto.Classes)
            {
                foreach (var saveRate in classLevel.Class.SaveBonusRate)
                {
                    if (characterSaves.All(x => x.SaveType != saveRate.Save))
                    {
                        characterSaves.Add(new Save
                        {
                            Description = saveRate.Save.Description,
                            Name = saveRate.Save.Name,
                            SaveType = saveRate.Save,
                            ID = saveRate.Save.ID,
                            SaveBonus = new CalculatedString()
                        });
                    }
                    var currentSave = characterSaves.First(x => x.SaveType == saveRate.Save);
                    currentSave.AddBaseSave(classLevel.Class.Name,classLevel.Level, saveRate.Rate);
                }
            }

            //Adding bonuses
            foreach (var characterSave in characterSaves)
            {
                characterSave.SetBonuesToSave(bonusDto);
            }

            return characterSaves.OrderBy(x => x.SaveType.Index).ToList();
        }
    }
}
