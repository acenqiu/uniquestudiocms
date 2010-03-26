using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 系统配置（基类）
    /// </summary>
    public class SystemConfig
    {
        protected string path = null;

        /// <summary>
        /// 返回xml格式的配置文件
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXmlConfig()
        {
            XmlManager manager = new XmlManager();
            return manager.LoadXml(PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, path));
        }

        /// <summary>
        /// 从XML文件中载入配置信息
        /// </summary>
        public void LoadConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, path));
            LoadConfig(doc);
        }

        /// <summary>
        /// 从XML文件中载入配置信息
        /// </summary>
        /// <param name="content">xml格式配置文件内容</param>
        public void LoadConfig(string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            LoadConfig(doc);
        }

        private void LoadConfig(XmlDocument doc)
        {
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

        /// <summary>
        /// 将配置信息保存到xml文档
        /// </summary>
        /// <param name="content">配置信息xml格式内容</param>
        public void SaveXmlConfig(string content)
        {
            (new XmlManager()).SaveXml(PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, path), content);
            LoadConfig();

        }
    }
}
