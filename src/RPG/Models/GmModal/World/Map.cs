using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Models.GmModal.World
{
    public class Map : ElementId
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int MinZoom { get; set; }
        public int MaxZoom { get; set; }
        public double DistanceScale { get; set; }

        public List<MapPart> Parts { get; set; }
    }
}