using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace UniqueStudio.Common.XmlHelper
{
    /// <summary>
    /// 提供对XML的读、写方法
    /// </summary>
    public class XmlManager
    {
        private XmlDocument doc = new XmlDocument();

        /// <summary>
        /// 初始化<see cref="XmlManager"/>类的实例。
        /// </summary>
        public XmlManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 载入xml文档
        /// </summary>
        /// <param name="fileName">文档路径</param>
        /// <returns>xml文档</returns>
        /// <exception cref="FileNotFoundException">当指定的xml文件不存在时抛出该异常</exception>
        public XmlDocument LoadXml(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            return doc;
        }

        /// <summary>
        /// 载入xml文档内容
        /// </summary>
        /// <param name="fileName">文档路径</param>
        /// <returns>文档内容</returns>
        public string LoadXmlContent(string fileName)
        {
            XmlDocument doc = LoadXml(fileName);
            return doc.OuterXml;
        }

        /// <summary>
        /// 生成xml文档的子文档
        /// </summary>
        /// <param name="fileName">文档路径</param>
        /// <param name="xPath">xpath（用于选取由哪些节点构成新的文档）</param>
        /// <returns>生成的新的xml文档</returns>
        public XmlDocument ConstructSubXmlDocument(string fileName, string xPath)
        {
            XmlDocument doc = LoadXml(fileName);
            return ConstructSubXmlDocument(doc, xPath);
        }

        /// <summary>
        /// 生成xml文档的子文档
        /// </summary>
        /// <param name="fileName">文档路径</param>
        /// <param name="xPath">xpath（用于选取由哪些节点构成新的文档）</param>
        /// <param name="rootName">生成的文档的根节点名</param>
        /// <returns>生成的新的xml文档</returns>
        public XmlDocument ConstructSubXmlDocument(string fileName, string xPath, string rootName)
        {
            XmlDocument doc = LoadXml(fileName);
            return ConstructSubXmlDocument(doc, xPath, rootName);
        }

        /// <summary>
        /// 生成xml文档的子文档
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="xPath">xpath（用于选取由哪些节点构成新的文档）</param>
        /// <returns>生成的新的xml文档</returns>
        public XmlDocument ConstructSubXmlDocument(XmlDocument doc, string xPath)
        {
            return ConstructSubXmlDocument(doc, xPath, "root");
        }

        /// <summary>
        /// 生成xml文档的子文档
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="xPath">xpath（用于选取由哪些节点构成新的文档）</param>
        /// <param name="rootName">生成的文档的根节点名</param>
        /// <returns>生成的新的xml文档</returns>
        public XmlDocument ConstructSubXmlDocument(XmlDocument doc, string xPath, string rootName)
        {
            if (!ValidxPathSyntax(xPath))
            {
                throw new Exception();
            }

            XmlDocument subDoc = new XmlDocument();
            XmlDeclaration dec = subDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            subDoc.AppendChild(dec);
            XmlElement root = subDoc.CreateElement(rootName);

            //如果是“/"，以下代码会出错
            if (xPath.Trim() == "/")
            {

            }
            XmlNodeList list = doc.SelectNodes(xPath);
            StringBuilder innerXml = new StringBuilder();
            foreach (XmlNode node in list)
            {
                innerXml.Append(node.OuterXml);
            }
            root.InnerXml = innerXml.ToString();
            subDoc.AppendChild(root);
            return subDoc;
        }
        /// <summary>
        /// 向指定xml文档插入节点
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="xPath">xpath,用于选取在哪些节点中插入新节点</param>
        /// <param name="innerXml">待插入节点的xml格式内容</param>
        /// <param name="nodeName">待插入节点名</param>
        public void InsertNode(XmlDocument doc, string xPath, string innerXml, string nodeName)
        {
            if (!ValidxPathSyntax(xPath))
            {
                throw new Exception();
            }

            //TODO:测试在选取到多个节点时是否会出现异常
            XmlNodeList list = doc.SelectNodes(xPath);
            XmlElement newNode = doc.CreateElement(nodeName);
            newNode.InnerXml = innerXml;
            foreach (XmlNode node in list)
            {
                node.AppendChild(newNode);
            }
        }

        /// <summary>
        /// 向指定xml文档插入节点
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="xPath">xpath,用于选取在哪些节点中插入新节点</param>
        /// <param name="source">待插入的对象（将序列化为xml节点）</param>
        /// <param name="t">待插入对象的类型</param>
        public void InsertNode(XmlDocument doc, string xPath, object source, Type t)
        {
            InsertNode(doc, xPath, source, t, t.Name);
        }
        /// <summary>
        /// 向指定xml文档插入节点
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="xPath">xpath,用于选取在哪些节点中插入新节点</param>
        /// <param name="source">待插入的对象（将序列化为xml节点）</param>
        /// <param name="t">待插入对象的类型</param>
        /// <param name="nodeName">待插入节点名</param>
        public void InsertNode(XmlDocument doc, string xPath, object source, Type t, string nodeName)
        {
            XmlDocument nodeDoc = ConvertToXml(source, t);
            XmlNode root = nodeDoc.ChildNodes[1];
            InsertNode(doc, xPath, root.InnerXml, nodeName);
        }

        /// <summary>
        /// 将对象序列化为xml文档
        /// </summary>
        /// <param name="source">待序列化对象</param>
        /// <param name="t">对象类型</param>
        /// <returns>xml文档</returns>
        public XmlDocument ConvertToXml(object source, Type t)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(t);
            xs.Serialize(stream, source);
            byte[] buffer = stream.ToArray();
            stream.Close();

            if (buffer != null)
            {
                string outerXml = Encoding.UTF8.GetString(buffer);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(outerXml);
                return doc;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 将xml文档反序列化为对象
        /// </summary>
        /// <param name="content">xml文档内容</param>
        /// <param name="t">对象类型</param>
        /// <param name="xPath">xpath,用于指定将那个节点反序列化为对象</param>
        /// <returns>反序列化后的对象</returns>
        public object ConvertToEntity(string content, Type t, string xPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            return ConvertToEntity(doc, t, xPath);
        }
        /// <summary>
        /// 将xml文档反序列化为对象
        /// </summary>
        /// <param name="doc">xml文档类型</param>
        /// <param name="t">对象类型</param>
        /// <param name="xPath">xpath,用于指定将那个节点反序列化为对象</param>
        /// <returns>反序列化后的对象</returns>
        public object ConvertToEntity(XmlDocument doc, Type t, string xPath)
        {
            if (!string.IsNullOrEmpty(xPath))
            {
                if (!ValidxPathSyntax(xPath))
                {
                    throw new Exception();
                }
                else
                {
                    doc = ConstructSubXmlDocument(doc, xPath, t.Name);
                }
            }
            byte[] buffer = Encoding.UTF8.GetBytes(doc.OuterXml);
            MemoryStream memStream = new MemoryStream(buffer);
            XmlSerializer xs = new XmlSerializer(t);
            object o = xs.Deserialize(memStream);
            memStream.Close();
            return o;
        }

        /// <summary>
        /// 将xml文档反序列化为对象
        /// </summary>
        /// <param name="stream">xml文档流</param>
        /// <param name="t">对象类型</param>
        /// <returns>反序列化后的对象</returns>
        public object ConvertToEntity(Stream stream, Type t)
        {
            XmlSerializer xs = new XmlSerializer(t);
            object o = xs.Deserialize(stream);
            stream.Close();
            return o;
        }

        /// <summary>
        /// 用指定的xsl文件转换指定的xml文档
        /// </summary>
        /// <remarks>该方法暂时无法实现该功能</remarks>
        /// <param name="doc">待转换xml文档</param>
        /// <param name="xslFile">xsl文件</param>
        /// <returns>已转换的doc文档</returns>
        public XmlDocument XslTransform(XmlDocument doc, string xslFile)
        {
            XPathNavigator nav = doc.CreateNavigator();
            nav.Select("/");
            MemoryStream stream = new MemoryStream();
            XmlWriter xw = new XmlTextWriter(stream, null);
            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(xslFile);
            trans.Transform(nav, xw);
            xw.Flush();
            XmlDocument newDoc = new XmlDocument();
            byte[] buffer = stream.ToArray();
            string s = Encoding.Default.GetString(buffer);
            newDoc.Load(stream);
            xw.Close();
            stream.Close();
            return newDoc;
        }

        /// <summary>
        /// 用指定的xsl文件转换指定的xml文档
        /// </summary>
        /// <remarks>该方法暂时无法实现该功能</remarks>
        /// <param name="doc">待转换xml文档</param>
        /// <param name="xslFile">xsl文件</param>
        /// <param name="stream">用于存放生成的xml文档的流</param>
        public void XslTransform(XmlDocument doc, string xslFile, Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将xml格式的内容保存成xml文档
        /// </summary>
        /// <param name="fileName">文档路径</param>
        /// <param name="content">xml格式的内容</param>
        public void SaveXml(string fileName, string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            doc.Save(fileName);
        }

        private static bool ValidxPathSyntax(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
            {
                return false;
            }
            Regex r = new Regex("/.*");
            return r.IsMatch(xPath);
        }
    }
}
