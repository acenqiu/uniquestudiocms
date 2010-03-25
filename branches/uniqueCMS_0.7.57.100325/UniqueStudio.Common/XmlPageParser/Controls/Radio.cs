using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;

namespace  UniqueStudio.Common.PageParser.Controls
{
    class Radio :Control
    {
        static string prototype = "<div class='form-item'><span class='form-item-label [%description%]' [%title%] >[%label%]</span><span class='form-item-input'>";
        static string optionProtptype = "<span><input type='radio' class='input-radio' [%value%] [%name%] [%checked%] />[%label%]</span>";
        public Radio(Hashtable attributes)
            : base(attributes)
        {
         
        }
        public override string ToHTML()
        {
            List<Hashtable> options = attributes["options"] as List<Hashtable>;
            foreach (Hashtable item in options)
            {
                if (item["value"].Equals(attributes["value"]))
                {
                    item.Add("checked", "checked");
                }
                item.Add("name", attributes["name"]);
            }
           return base.ToHTML(prototype, optionProtptype, attributes);

        }
    }
}
