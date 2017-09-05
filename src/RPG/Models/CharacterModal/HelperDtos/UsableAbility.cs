using System;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class UsableAbility
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsCurrentlyUseable { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}
