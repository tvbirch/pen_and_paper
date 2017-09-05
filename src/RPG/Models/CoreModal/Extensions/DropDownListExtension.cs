using System;
using System.Web.UI.WebControls;

namespace RPG.Models.CoreModal.Extensions
{
    public static class DropDownListExtension
    {
        public static void SetSelectedItemFromGuid(this DropDownList list, Guid? selected)
        {
            var indexItem = list.Items.FindByValue(selected.ToString());
            list.SelectedIndex = list.Items.IndexOf(indexItem);
        }
        public static void SetSelectedItemFromInt(this DropDownList list, int? selected)
        {
            var indexItem = list.Items.FindByValue(selected.ToString());
            list.SelectedIndex = list.Items.IndexOf(indexItem);
        }
    }
}
