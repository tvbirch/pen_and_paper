using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Models.CharacterModal;
using RPG.Models.Context;
using RPG.Models.GmModal;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.ControllerDto.GM
{
    public class CharactersDto
    {
        public CharactersDto(){}
        public CharactersDto(ContextManager context)
        {
            Characters = context.Context.Characters.ToList().Cast<ElementId>().ToList();
            CharacterGuids = context.GetAll<GmCharacterView>().Select(x => x.CharacterGuid).ToArray();
        }
        public List<ElementId> Characters { get; set; }
        public Guid[] CharacterGuids { get; set; }
    }
}