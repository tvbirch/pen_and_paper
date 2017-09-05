using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;

namespace RPG.Models.ControllerDto.Game
{
    public class CharacterSpellDto : CharacterDto
    {
        public CharacterSpellDto(ContextManager context, Guid id)
        {
            Character = context.LoadCharacter(id);
        }
    }
}