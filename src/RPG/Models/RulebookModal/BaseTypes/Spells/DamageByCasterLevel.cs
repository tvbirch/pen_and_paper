using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;
using RPG.Models.RulebookModal.BaseTypes.Damages;

namespace RPG.Models.RulebookModal.BaseTypes.Spells
{
    public class DamageByCasterLevel : GameId
    {
        public int AvailableAtLevel { get; set; }
        public Damage Damage { get; set; }
    }
}