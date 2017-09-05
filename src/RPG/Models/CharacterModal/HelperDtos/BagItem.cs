using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class BagItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfItems { get; set; }
        public ImperialWeight Weight { get; set; }
        public string Description { get; set; }
    }
}