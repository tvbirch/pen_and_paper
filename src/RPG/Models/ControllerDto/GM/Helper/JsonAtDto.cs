using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RPG.Models.ControllerDto.GM.Helper
{
    public class JsonAtDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
    }
}