using System;
using System.Web.UI.WebControls;
using RPG.Models.CoreModal.DTO;

namespace RPG.Models.CoreModal.Extensions
{
    public static class ListBoxExtension
    {
        public static int[] ToIntArray(this ListBox box)
        {
            if (box == null)
            {
                return null;
            }
            var newArray = new int[box.Items.Count];
            for (int i = 0; i < box.Items.Count; i++)
            {
                newArray[i] = int.Parse(box.Items[i].Text);
            }
            return newArray;
        }

        public static void SetIntArray(this ListBox box, int[] array)
        {
            if (box == null)
            {
                return;
            }
            foreach (var i in array)
            {
                box.Items.Add(new ListItem(i.ToString()));
            }
        }
        public static Guid[] ToGuidArray(this ListBox box)
        {
            if (box == null)
            {
                return null;
            }
            var newArray = new Guid[box.Items.Count];
            for (int i = 0; i < box.Items.Count; i++)
            {
                newArray[i] = Guid.Parse(box.Items[i].Value);
            }
            return newArray;
        }

        public static void SetGuidArray(this ListBox box, ObjectDescription[] array)
        {
            if (box == null)
            {
                return;
            }
            foreach (var i in array)
            {
                box.Items.Add(new ListItem(i.Name,i.Id.ToString()));
            }
        }
    }
}
