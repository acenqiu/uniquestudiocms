using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.Common.Config
{
    public class SystemConfig
    {
        protected string path = null;

        public XmlDocument GetXmlConfig()
        {
            XmlManager manager = new XmlManager();
            return manager.LoadXml(GlobalConfig.BasePhysicalPath+path);
        }

        public void LoadConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(GlobalConfig.BasePhysicalPath + path);
            XmlNodeList nodes = doc.SelectNodes("//param");
            foreach (XmlNode node in nodes)
            {
                PropertyInfo property = this.GetType().GetProperty(node.Attributes["name"].Value);
                if (property != null)
                {
                    object oValue = null;
                    switch (property.PropertyType.Name)
                    {
                        case "String":
                            oValue = node.Attributes["value"].Value;
                            break;
                        case "Boolean":
                            bool bTemp;
                            if (bool.TryParse(node.Attributes["value"].Value, out bTemp))
                            {
                                oValue = bTemp;
                            }
                            else
                            {
                                oValue = property.GetValue(this, null);
                            }
                            break;
                        case "Int32":
                            int iTemp;
                            if (int.TryParse(node.Attributes["value"].Value, out iTemp))
                            {
                                oValue = iTemp;
                            }
                            else
                            {
                                oValue = property.GetValue(this, null);
                            }
                            break;
                        default:
                            oValue = node.Attributes["value"].Value;
                            break;
                    }
                    property.SetValue(this, oValue, null);
                }
            }
        }

        public void SaveXmlConfig(string content)
        {
            (new XmlManager()).SaveXml(GlobalConfig.BasePhysicalPath+path, content);
            LoadConfig();
        }
    }
}
