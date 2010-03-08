using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;

namespace  UniqueStudio.Common.PageParser.Controls
{
    abstract class Control
    {
        public static  string attribPattern = @"\[\%.*?\%\]";
        
        public Control(Hashtable attributes)
        {
            this.attributes=attributes;

        }
        public static  string prototype = "";
        public Hashtable attributes;
       
        public virtual string ToHTML()
        {
            return ToHTML(prototype);
        }
        public  string  ToHTML(string prototype)
        {
            return ToHTML(prototype, attributes);
        }
        public string ToHTML(string prototype,Hashtable attributes)
        {
            StringBuilder sb = new StringBuilder(prototype);
            sb = sb.Replace("[%label%]", (string)attributes["label"]);
            if (attributes.ContainsKey("title"))
            {
                sb = sb.Replace("[%description%]", "description");
            }
            else
            {
                sb = sb.Replace("[%description%]", "");
            }
            foreach (DictionaryEntry de in attributes)
            {
                sb = sb.Replace("[%" + de.Key + "%]", de.Key + "='" + de.Value + "'");
            }
            MatchCollection matchs = Regex.Matches(sb.ToString(), attribPattern);
            if (matchs!=null)
            {
                foreach (Match match in matchs)
                {
                    sb = sb.Replace(match.Value, "");
                }
            }
            return sb.ToString();
        }
        public string ToHTML(string prototype, string optionPrototype, Hashtable attributes)
        {
            StringBuilder sb = new StringBuilder(ToHTML(prototype));
            List<Hashtable> options = attributes["options"] as List<Hashtable>;
            if (options != null)
            {
                foreach (Hashtable item in options)
                {
                    sb.Append(ToHTML(optionPrototype, item));
                }
            }
            sb.Append("</span> </div>");
            return sb.ToString();
        }
    }
}
