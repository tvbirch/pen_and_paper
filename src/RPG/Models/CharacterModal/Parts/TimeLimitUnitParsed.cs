using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Time;

namespace RPG.Models.CharacterModal.Parts
{
    public class TimeLimitUnitParsed : GameId
    {
        public TimeLimitUnit Time { get; set; }
    }
}