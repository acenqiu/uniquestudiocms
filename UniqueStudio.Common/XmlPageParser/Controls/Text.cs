using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;

namespace  UniqueStudio.Common.PageParser.Controls
{
    class Text : Control
    {
        private static string prototype = "<div class='form-item'><span class='form-item-label [%description%]' [%title%] >[%label%]</span><span class='form-item-input'><span><input type='text' class='input-text'  [%value%] [%name%]/></span></span> </div>"; 
        public Text(Hashtable attributes):base(attributes)
        {

        }
        public override string ToHTML()
        {
            return base.ToHTML(prototype);
        }
    }
}
