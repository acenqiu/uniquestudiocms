using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;

namespace  UniqueStudio.Common.PageParser.Controls
{
    class SelectList:Control
    {
        static string prototype = "<div class='form-item'><span class='form-item-label [%description%]' [%title%]>[%label%]</span><span class='form-item-input'><select [%name%] class='input-select'>";
        static string optionPrototype = "<option [%value%] [%selected%]>[%label%]</option>";
        public SelectList(Hashtable attributes)
            : base(attributes)
        {
        }
        public override string ToHTML()
        {
            StringBuilder sb = new StringBuilder(ToHTML(prototype));
            List<Hashtable> options = attributes["options"] as List<Hashtable>;
            foreach (Hashtable item in options)
            {
                if (item["value"].Equals(attributes["value"]))
                {
                    item.Add("selected", "selected");
                }
                sb.Append(ToHTML(optionPrototype, item));
            }
            sb.Append("</select>");
            sb.Append("</span> </div>");
            return sb.ToString();

        }
    }
}
