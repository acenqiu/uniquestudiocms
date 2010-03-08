using System;
using System.Collections;

using System.Text;

namespace  UniqueStudio.Common.PageParser.Controls
{
    class Rich:Control
    {
        static string prototype = "<div class='form-item' ><span class='form-item-label [%description%]' [%title%] >[%label%]</span><span class='form-item-input'>";
        static string optionProtptype = "<span><input type='checkbox' class='input-checkbox' [%value%] [%name%] [%check%] />[%label%]</span>";
        public Rich(Hashtable attributes)
            : base(attributes)
        {
        }
        public override string ToHTML()
        {
            return base.ToHTML(prototype, optionProtptype, attributes);
        }
    }
}
