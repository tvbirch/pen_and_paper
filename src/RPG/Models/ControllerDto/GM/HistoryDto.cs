using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPG.Models.Context;
using RPG.Models.GmModal.World;

namespace RPG.Models.ControllerDto.GM
{
    public class HistoryDto
    {
        public HistoryDto() { }
        public HistoryDto(ContextManager context, Guid? selectedHistory)
        {
            if (selectedHistory.HasValue)
            {
                SelectedItem = context.Get<History>(selectedHistory.Value);
            }
            WorldHistory = context.GetQueryable<History>().OrderByDescending(x => x.Created).ToList();
        }
        public History SelectedItem { get; set; }
        public List<History> WorldHistory { get; set; }
    }
}