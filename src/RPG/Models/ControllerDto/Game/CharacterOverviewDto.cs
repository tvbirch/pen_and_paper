using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal;
using RPG.Models.Context;

namespace RPG.Models.ControllerDto.Game
{
    public class CharacterOverviewDto : CharacterDto
    {
        public CharacterOverviewDto(ContextManager context, Guid value)
        {
            Character = context.LoadCharacter(value);
        }
    }
}