using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace  UniqueStudio.Common.PageParser.Controls
{
    class TextArea:Control
    {
        static string prototype = "<div class='form-item'><span class='form-item-label [%description%]'  [%title%] >[%label%]</span><span class='form-item-input' ><span><textarea id='TextArea1' class='input-textarea' [%cols%]  [%rows%]  [%name%]>[%value%]</textarea></span></span> </div>"; 
        public TextArea(Hashtable attributes)
            : base(attributes)
        {
        }
        public override string ToHTML()
        {
            Regex r = new Regex(@"value\='(?<v>[^']*)'");
            string html = base.ToHTML(prototype);
            html = r.Replace(html, "${v}");
            return html;
        }
    }
}
