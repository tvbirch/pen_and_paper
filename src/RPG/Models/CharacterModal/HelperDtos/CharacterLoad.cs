using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RPG.Models.CharacterModal.HelperDtos
{
    public class CharacterLoad
    {
        public int LightLoad { get; set; }
        public int MediumLoad { get; set; }
        public int HeavyLoad { get; set; }
        public int LiftOverHeadLoad { get; set; }
        public int LiftOverGroundLoad { get; set; }
        public int PushOrDrag { get; set; }
        public int CurrentLoad { get; set; }
        public bool IsLightLoad { get; set; }
        public bool IsMediumLoad { get; set; }
        public bool IsHeavyLoad { get; set; }
    }
}