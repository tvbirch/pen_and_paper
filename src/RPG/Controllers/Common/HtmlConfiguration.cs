using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace RPG.Controllers.Common
{
    // ReSharper disable InconsistentNaming
    public static class HtmlConfiguration
    {
        public static object TextBoxFor_Text = new
        {
            @class = "form-control",
            @type = "text"
        };

        public static object TextBoxFor_File = new
        {
            @class = "form-control",
            @type = "file"
        };

        public static object TextBoxFor_Common_ElementId_Name = new
        {
            @class = "form-control",
            @type = "text",
            @maxlength = "250",
            @required = "required",
            @data_val = true,
            @data_val_required="The Name field is required.",
        };
        public static object TextBoxFor_Common_ElementId_Description = new
        {
            @class = "form-control",
            @type = "text",
            @maxlength = "4000"
        };

        public static object TextBoxFor_Number = new
        {
            @class = "form-control",
            @type = "number"
        };
        public static object TextArea1R = new
        {
            @class = "form-control",
            @rows = "1"
        };
        public static object TextArea5R = new
        {
            @class = "form-control",
            @rows = "5"
        };
        public static object TextArea3R = new
        {
            @class = "form-control",
            @rows = "3"
        };
        public static object TextArea10R = new
        {
            @class = "form-control",
            @rows = "10"
        };
        public static object TextArea14R = new
        {
            @class = "form-control",
            @rows = "14"
        };

        public static object TextBoxFor_NumberWithOnChange(string onChangeFuntion)
        {
            return new
            {
                @class = "form-control",
                @type = "number",
                @onChange = onChangeFuntion
            };
        }

        public static object TextBoxFor_ReadOnly = new
        {
            @class = "form-control",
            @readonly = true,
            @tabindex = -1,
        };

        public static object ActionLink_Button = new
        {
            @class = "btn btn-primary"
        };
        public static object ActionLink_ButtonBlock = new
        {
            @class = "btn btn-primary btn-block"
        };

        public static object DropDownListFor = new
        {
            @class = "form-control"
        };
        public static object DropDownListForWithOnChange(string onChangeFuntion)
        {
            return new
            {
                @class = "form-control",
                @onChange = onChangeFuntion
            };
        }
        public static object ListBoxFor_SelectMultiple = new
        {
            @class = "form-control chosen-select",
            @data_placeholder = "Select values..."
        };
        
        public static object ListBoxFor_SelectSingle = new
        {
            @class = "form-control chosen-select-max1",
            @data_placeholder = "Select values..."
        };

        public static object ListBoxFor_Select2Multiple = new
        {
            @class = "js-select2-basic-multiple",
            @multiple = "multiple"
        };
        public static object ListBoxFor_Select2MultipleConditionsList = new
        {
            @class = "js-select2-basic-multiple-conditionlist",
            @multiple = "multiple"
        };


        public static object ListBoxFor_SelectSingleWithOnChange(string onChangeFuntion)
        {
            return new
            {
                @class = "form-control chosen-select-max1",
                @data_placeholder = "Select values...",
                @onChange = onChangeFuntion
            };
        }

        
    }
    // ReSharper restore InconsistentNaming
}