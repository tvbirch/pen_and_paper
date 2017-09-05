using System.Collections.Generic;
using RPG.Models.CharacterModal;
using RPG.Models.GmModal.World;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Classes;

namespace RPG.Models.RulebookModal.BaseTypes.Alligments
{
    public class Alligment : OrderedElementId
    {
        public virtual ICollection<ClassBase> Classes { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<NPC> NPCs { get; set; }
    }
}
