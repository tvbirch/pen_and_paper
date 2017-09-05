using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;

namespace RPG.Models.ControllerDto.Game
{
    public class CharacterEquipmentDto : CharacterDto
    {
        public CharacterEquipmentDto(ContextManager context, Guid value)
        {
            Character = context.LoadCharacter(value);
        }
    }
}