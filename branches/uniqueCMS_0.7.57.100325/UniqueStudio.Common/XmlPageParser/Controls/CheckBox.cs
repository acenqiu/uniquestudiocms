using System;
using System.Collections.Generic;
using System.Collections;

using System.Text;

namespace  UniqueStudio.Common.PageParser.Controls
{
    class CheckBox:Control
    {
        static string prototype = "<div class='form-item'><span class='form-item-label [%description%]' [%title%]>[%label%]</span><span class='form-item-input'>";
        static string optionProtptype = "<span><input type='checkbox' class='input-checkbox' [%value%] [%name%] />[%label%]</span>";
        public CheckBox(Hashtable attributes)
            : base(attributes)
        {
        }
        public override string ToHTML()
        {
            return base.ToHTML(prototype, optionProtptype, attributes);
        }
    }
}
