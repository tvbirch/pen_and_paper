using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.GmModal.World
{
    public class MapPart : GameId
    {
        public virtual Map Map { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public byte[] Data { get; set; }
        public string FileType { get; set; }

        public override string ToString()
        {
            return $"{Z}/{X}/{Y}.{FileType}";
        }
    }
}