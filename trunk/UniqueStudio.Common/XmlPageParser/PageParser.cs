using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;

namespace  UniqueStudio.Common.PageParser
{
    public class XmlPageParser
    {
        public XmlTextReader reader = null;
        public string ProcessXML(XmlDocument doc)
        {
           StringBuilder str = new StringBuilder();
          XmlNodeList list= doc.GetElementsByTagName("param");
          foreach (XmlNode node in list)
          {
              Hashtable attributes = new Hashtable();
              foreach (XmlAttribute attribute in node.Attributes)
              {
                  attributes.Add(attribute.Name, attribute.Value);
              }
              if (node.HasChildNodes)
              {
                  List<Hashtable> options = new List<Hashtable>();

                  foreach (XmlNode child in node.ChildNodes)
                  {
                          Hashtable option = new Hashtable();
                          option.Add("value", child.Attributes["value"].Value);
                          option.Add("label", child.Attributes["label"].Value);
                          options.Add(option);
                  }
                  attributes.Add("options", options);
              }
              str.Append(GetControl(attributes).ToHTML());
              str.Append("\n\r");
          }
          return str.ToString();
        }
        public String ProcessXML(string url)
        {

            reader = new XmlTextReader(url);
            StringBuilder str = new StringBuilder();
            while (reader.Read())
            {
                XmlNodeType nType = reader.NodeType;
                switch (nType)
                {
                    case XmlNodeType.Element: 
                        Hashtable attributes = new Hashtable();

                        while (reader.MoveToNextAttribute())
                        {
                            
                            attributes.Add(reader.Name, reader.Value);
                        }
                        if (attributes.Contains("type"))
                        {
                            reader.MoveToElement();
                            if (reader.ReadToDescendant("option"))
                            {
                                List<Hashtable> options = new List<Hashtable>();
                                do
                                {
                                    Hashtable option = new Hashtable();
                                    option.Add("value", reader.GetAttribute("value"));
                                    option.Add("label", reader.GetAttribute("label"));
                                    options.Add(option);
                                }
                                while (reader.ReadToNextSibling("option"));
                                attributes.Add("options", options);
                            }

                            str.Append(GetControl(attributes).ToHTML());
                            str.Append("\n\r");
                        }
                        break;
                    default: break;
                }
            }
           return str.ToString();
        }

        private Controls.Control GetControl(Hashtable attributes)
        {
            switch ((string)attributes["type"])
            {
                case "text": return new Controls.Text(attributes);
                case "textarea": return new Controls.TextArea(attributes); 
                case "rich": return new Controls.Rich(attributes); 
                case "list": return new Controls.SelectList(attributes); 
                case "radio": return new Controls.Radio(attributes);
                case "spacer": return new Controls.Spacer(attributes);
                case "checkbox": return new Controls.CheckBox(attributes);
            }
            return null;
        }
    }
}
