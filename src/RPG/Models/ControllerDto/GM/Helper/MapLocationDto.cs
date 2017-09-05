using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RPG.Models.GmModal.World;

namespace RPG.Models.ControllerDto.GM.Helper
{
    public class MapLocationDto
    {
        public string Type { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Description { get; set; }

        public MapLocationDto(Location loc)
        {
            Type = loc.LocationType.Name;
            Lat = loc.Lat.GetValueOrDefault();
            Lon = loc.Lon.GetValueOrDefault();
            var link = UrlHelper.GenerateUrl(null, "Index", "Location", new RouteValueDictionary(new { id = loc.ID }), RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);
            var strBuilder = new StringBuilder();
            strBuilder.Append(HtmlStr("Name", loc.Name));
            strBuilder.Append(HtmlStr("Population", loc.Population));
            strBuilder.Append(HtmlStr("Notes", loc.Description));
            strBuilder.Append($"<a href={link}>Link</a>");


            Description = strBuilder.ToString();
        }

        private string HtmlStr(string header,string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                return "";
            }
            return $"<b>{header}:</b><br>{txt}<br>\n";
        }
    }
}