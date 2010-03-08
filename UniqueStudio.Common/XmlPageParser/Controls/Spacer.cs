using System;
using System.Collections;

using System.Text;

namespace  UniqueStudio.Common.PageParser.Controls
{
    class Spacer:Control
    {
        private static new  string prototype = "<div class='form-item'> <hr /> </div>"; 
        public Spacer(Hashtable attributes)
            : base(attributes)
        {
        }
        public override string ToHTML()
        {
            return base.ToHTML(prototype);
        }
    }
}
