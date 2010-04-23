//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：实现将特定格式的xml文档转换为html控件的方法。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：葛致良
//=================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace UniqueStudio.Common.PageParser
{
    /// <summary>
    /// 实现将特定格式的xml文档转换为html控件的方法。
    /// </summary>
    public class XmlPageParser
    {
        private XmlTextReader reader = null;

        /// <summary>
        /// 处理xml文档，生成html控件代码。
        /// </summary>
        /// <param name="doc">xml文档。</param>
        /// <returns>html控件代码。</returns>
        public string ProcessXML(XmlDocument doc)
        {
            StringBuilder str = new StringBuilder();
            XmlNodeList list = doc.GetElementsByTagName("param");
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
                Controls.Control control = GetControl(attributes);
                if (control != null)
                {
                    str.Append(control.ToHTML());
                    str.Append("\n\r");
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 处理xml文档，生成html控件代码。
        /// </summary>
        /// <param name="fileName">xml文档路径。</param>
        /// <returns>html控件代码。</returns>
        public String ProcessXML(string fileName)
        {

            reader = new XmlTextReader(fileName);
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
            string type = ((string)attributes["type"]).ToLower();
            switch (type)
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
