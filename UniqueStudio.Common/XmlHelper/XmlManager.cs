using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Xml.XPath;

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

        public string LoadXmlContent(string fileName)
        {
            return FileAccessHelper.FileAccess.ReadFile(fileName);
        }

        public XmlDocument ConstructSubXmlDocument(string fileName, string xPath)
        {
            XmlDocument doc = LoadXml(fileName);
            return ConstructSubXmlDocument(doc, xPath);
        }

        public XmlDocument ConstructSubXmlDocument(string fileName, string xPath, string rootName)
        {
            XmlDocument doc = LoadXml(fileName);
            return ConstructSubXmlDocument(doc, xPath, rootName);
        }

        public XmlDocument ConstructSubXmlDocument(XmlDocument doc, string xPath)
        {
            return ConstructSubXmlDocument(doc, xPath, "root");
        }

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

        public void InsertNode(XmlDocument doc, string xPath, string innerXml,string nodeName)
        {
            if (!ValidxPathSyntax(xPath))
            {
                throw new Exception();
            }

            XmlNodeList list = doc.SelectNodes(xPath);
            XmlElement newNode = doc.CreateElement(nodeName);
            newNode.InnerXml = innerXml;
            foreach (XmlNode node in list)
            {
                node.AppendChild(newNode);
            }
        }

        public void InsertNode(XmlDocument doc, string xPath, object source, Type t)
        {
            InsertNode(doc, xPath, source, t, t.Name);
        }

        public void InsertNode(XmlDocument doc, string xPath, object source, Type t,string nodeName)
        {
            XmlDocument nodeDoc = ConvertToXml(source, t);
            XmlNode root = nodeDoc.ChildNodes[1];
            InsertNode(doc, xPath, root.InnerXml, nodeName);
        }

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

        public object ConvertToEntity(string content, Type t, string xPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            return ConvertToEntity(doc, t, xPath);
        }

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

        public object ConvertToEntity(Stream stream, Type t)
        {
            XmlSerializer xs = new XmlSerializer(t);
            object o = xs.Deserialize(stream);
            stream.Close();
            return o;
        }

        public XmlDocument XslTransform(XmlDocument doc, string xslFile)
        {
            XPathNavigator nav = doc.CreateNavigator();
            nav.Select("/");
            MemoryStream stream = new MemoryStream();
            XmlWriter xw = new XmlTextWriter(stream,null);
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

        public void XslTransform(XmlDocument doc, string xslFile, Stream stream)
        {

        }

        public string SelectAttribute(XmlDocument doc, string xPath, string attributeName)
        {
            throw new NotImplementedException();
        }

        public string SelectInnerText(XmlDocument doc, string xPath)
        {
            throw new NotImplementedException();
        }
        
        public XmlDocument SelectNodes(string fileName, string tagName)
        {
            XmlDocument doc = LoadXml(fileName);
            return SelectNodes(doc, tagName);
        }

        public XmlDocument SelectNodes(string fileName, string tagName, string rootName)
        {
            XmlDocument doc = LoadXml(fileName);
            return SelectNodes(doc, tagName, rootName);
        }

        public XmlDocument SelectNodes(XmlDocument doc, string tagName)
        {
            return SelectNodes(doc, tagName, "root");
        }

        public XmlDocument SelectNodes(XmlDocument doc, string tagName, string rootName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                throw new ArgumentNullException();
            }

            XmlDocument subDoc = new XmlDocument();
            XmlDeclaration dec = subDoc.CreateXmlDeclaration("1.0", null, null);
            subDoc.AppendChild(dec);
            XmlElement root = subDoc.CreateElement(rootName);

            XmlNodeList list = doc.GetElementsByTagName(tagName);
            StringBuilder innerXml = new StringBuilder();
            foreach (XmlNode node in list)
            {
                innerXml.Append(node.OuterXml);
            }
            root.InnerXml = innerXml.ToString();
            subDoc.AppendChild(root);
            return subDoc;
        }

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
