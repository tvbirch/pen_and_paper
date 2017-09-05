using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal;
using RPG.Models.Context;

namespace RPG.Models.ControllerDto.Game
{
    public class CharacterSelectorDto
    {
        public List<Character> Characters { get; set; }

        public CharacterSelectorDto(ContextManager context)
        {
            Characters = context.GetAll<Character>();
        }
    }
}