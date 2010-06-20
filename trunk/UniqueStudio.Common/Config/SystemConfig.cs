//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：系统配置（基类）。
// 完成日期：2010年04月10日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System.Reflection;
using System.Xml;

using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 系统配置（基类）。
    /// </summary>
    public class SystemConfig
    {
        protected string path = null;

        /// <summary>
        /// 返回xml格式的配置文件。
        /// </summary>
        /// <returns>配置文件xml文档。</returns>
        public XmlDocument GetXmlConfig()
        {
            return XmlManager.LoadXml(PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, path));
        }

        /// <summary>
        /// 从XML文件中载入配置信息，并填充到该类。
        /// </summary>
        public void LoadConfig()
        {
            XmlDocument doc = XmlManager.LoadXml(PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, path));
            LoadConfig(doc);
        }

        /// <summary>
        /// 从XML文件中载入配置信息，并填充到该类。
        /// </summary>
        /// <param name="content">xml格式配置文件内容。</param>
        public void LoadConfig(string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            LoadConfig(doc);
        }

        private void LoadConfig(XmlDocument doc)
        {
            XmlNodeList nodes = doc.SelectNodes("//param");
            if (nodes == null)
            {
                return;
            }
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
                        case "Int64":
                            long lTemp;
                            if (long.TryParse(node.Attributes["value"].Value, out lTemp))
                            {
                                oValue = lTemp;
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
            }//end of foreach
        }

        /// <summary>
        /// 将配置信息保存到xml文档。
        /// </summary>
        /// <param name="content">配置信息xml格式内容。</param>
        public void SaveXmlConfig( string content)
        {
            XmlManager.SaveXml(PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, path), content);
            LoadConfig();
        }
    }
}
