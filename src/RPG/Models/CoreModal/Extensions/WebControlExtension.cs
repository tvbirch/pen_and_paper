using System.Web.UI.WebControls;

namespace RPG.Models.CoreModal.Extensions
{
    public static class WebControlExtension
    {
        public static void SetControlColor(this DropDownList control, string color)
        {
            SetWebControlColor(control, color);
        }

        /// <summary>
        /// The Textbox must be using CssClass for the css if classes is used it will be overridden.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        public static void SetToolTip(this DropDownList control, string text)
        {
            SetWebControlToolTip(control, text);
        }

        /// <summary>
        /// The Textbox must be using CssClass for the css if classes is used it will be overridden.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        public static void SetToolTip(this TextBox control, string text)
        {
            SetWebControlToolTip(control, text);
        }


        #region WebControlFunctions
        private static void SetWebControlColor(WebControl control, string color)
        {
            string colorToSet;
            if (string.IsNullOrWhiteSpace(color))
            {
                colorToSet = "FFFFFF";
            }
            else
            {
                colorToSet = color;
            }
            control.Attributes.Add("style", "background-color:#" + colorToSet);
        }

        private static void SetWebControlToolTip(WebControl control, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                if (control.CssClass.Contains(" pop"))
                {
                    control.CssClass = control.CssClass.Replace(" pop", "");
                }
            }
            else
            {
                if (!control.CssClass.Contains(" pop"))
                {
                    control.CssClass += " pop";
                }
                control.Attributes.Add("data-container", "body");
                control.Attributes.Add("data-toggle", "popover");
                control.Attributes.Add("data-placement", "bottom");
                control.Attributes.Add("data-content", text);
            }
        }
        #endregion
    }
}
