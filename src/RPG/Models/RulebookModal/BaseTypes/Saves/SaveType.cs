using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.CharacterModal.Parts;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.RulebookModal.BaseTypes.Saves
{
    public class SaveType : OrderedElementId
    {
        public Ability AbilityModifier { get; set; }
    }
}