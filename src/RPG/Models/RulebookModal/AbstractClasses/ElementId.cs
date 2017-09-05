using System;
using System.ComponentModel.DataAnnotations;

namespace RPG.Models.RulebookModal.AbstractClasses
{
    public class ElementId : GameId
    {
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
