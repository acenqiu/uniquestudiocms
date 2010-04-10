//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供对Xml的操作方法。
// 完成日期：2010年04月10日
// 版本：v1.0
// 作者：邱江毅
//=================================================================
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Common.XmlHelper
{
    /// <summary>
    /// 提供对Xml的操作方法。
    /// </summary>
    /// <remarks>该类不捕捉异常；非静态类将把操作结果
    /// 保存在内部的XmlDocument中，并且其他操作都将在这个文档的基础上进行。</remarks>
    public class XmlManager
    {
        private XmlDocument doc = null;

        /// <summary>
        /// 获取或设置内部xml文档。
        /// </summary>
        public XmlDocument Doc
        {
            get { return doc; }
            set { doc = value; }
        }

        /// <summary>
        /// 初始化<see cref="XmlManager"/>类的实例。
        /// </summary>
        public XmlManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以xml文档路径初始化<see cref="XmlManager"/>类的实例。
        /// </summary>
        /// <param name="fileName">xml文档路径。</param>
        public XmlManager(string fileName)
        {
            Validator.CheckStringNull(fileName, "fileName");

            doc = LoadXml(fileName);
        }

        /// <summary>
        /// 以xml文档初始化<see cref="XmlManager"/>类的实例。
        /// </summary>
        /// <param name="doc">xml文档。</param>
        public XmlManager(XmlDocument doc)
        {
            Validator.CheckNull(doc, "doc");
            this.doc = doc;
        }

        #region Static Methods

        /// <summary>
        /// 将xml文档反序列化为对象。
        /// </summary>
        /// <param name="content">xml文档内容。</param>
        /// <param name="t">对象类型。</param>
        /// <param name="xPath">xPath,用于指定将哪个节点反序列化为对象。</param>
        /// <returns>反序列化后的对象。</returns>
        public static object ConvertToEntity(string content, Type t, string xPath)
        {
            Validator.CheckNull(t, "t");

            return ConvertToEntity(content, t, t.Name, xPath);
        }

        /// <summary>
        /// 将xml文档反序列化为对象。
        /// </summary>
        /// <param name="content">xml文档内容。</param>
        /// <param name="t">对象类型。</param>
        /// <param name="typeName">对象类型名称（对于有些继承类型，通过反射
        /// 得到的类型名可能和声明的不同）。</param>
        /// <param name="xPath">xPath,用于指定将哪个节点反序列化为对象。</param>
        /// <returns>反序列化后的对象。</returns>
        public static object ConvertToEntity(string content, Type t, string typeName, string xPath)
        {
            Validator.CheckStringNull(content, "content");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            return ConvertToEntity(doc, t, typeName, xPath);
        }

        /// <summary>
        /// 将xml文档反序列化为对象。
        /// </summary>
        /// <param name="doc">xml文档。</param>
        /// <param name="t">对象类型。</param>
        /// <param name="xPath">xPath,用于指定将哪个节点反序列化为对象。</param>
        /// <returns>反序列化后的对象。</returns>
        public static object ConvertToEntity(XmlDocument doc, Type t, string xPath)
        {
            Validator.CheckNull(t, "t");

            return ConvertToEntity(doc, t, t.Name, xPath);
        }

        /// <summary>
        /// 将xml文档反序列化为对象。
        /// </summary>
        /// <param name="doc">xml文档。</param>
        /// <param name="t">对象类型。</param>
        /// <param name="typeName">对象类型名称（对于有些继承类型，通过反射
        /// 得到的类型名可能和声明的不同）。</param>
        /// <param name="xPath">xPath,用于指定将哪个节点反序列化为对象。</param>
        /// <returns>反序列化后的对象。</returns>
        public static object ConvertToEntity(XmlDocument doc, Type t, string typeName, string xPath)
        {
            Validator.CheckNull(doc, "doc");
            Validator.CheckNull(t, "t");
            Validator.CheckStringNull(typeName, "typeName");

            if (!string.IsNullOrEmpty(xPath))
            {
                doc = SubXmlDocument(doc, xPath, typeName);
            }

            byte[] buffer = Encoding.UTF8.GetBytes(doc.OuterXml);
            MemoryStream memStream = new MemoryStream(buffer);
            XmlSerializer xs = new XmlSerializer(t);
            object o = xs.Deserialize(memStream);
            memStream.Close();
            return o;
        }

        /// <summary>
        /// 将xml文档反序列化为对象。
        /// </summary>
        /// <param name="stream">xml文档流。</param>
        /// <param name="t">对象类型。</param>
        /// <returns>反序列化后的对象。</returns>
        public static object ConvertToEntity(Stream stream, Type t)
        {
            Validator.CheckNull(stream, "stream");

            XmlSerializer xs = new XmlSerializer(t);
            object o = xs.Deserialize(stream);
            stream.Close();
            return o;
        }

        /// <summary>
        /// 将对象序列化为xml文档。
        /// </summary>
        /// <param name="source">待序列化对象。</param>
        /// <param name="t">对象类型。</param>
        /// <returns>xml文档。</returns>
        public static XmlDocument ConvertToXml(object source, Type t)
        {
            Validator.CheckNull(source, "source");
            Validator.CheckNull(t, "t");

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
        /// 返回xml文档内容。
        /// </summary>
        /// <param name="fileName">文档路径。</param>
        /// <returns>文档内容。</returns>
        public static string GetXmlContent(string fileName)
        {
            Validator.CheckStringNull(fileName, "fileName");

            XmlDocument doc = LoadXml(fileName);
            return doc.OuterXml;
        }

        /// <summary>
        /// 向指定的xml文档插入节点。
        /// </summary>
        /// <param name="doc">xml文档。</param>
        /// <param name="xPath">xPath,用于选取在哪些节点内插入新节点。</param>
        /// <param name="innerXml">待插入节点的xml格式内容（不含节点名）。</param>
        /// <param name="nodeName">待插入节点名。</param>
        public static void InsertNode(XmlDocument doc, string xPath, string innerXml, string nodeName)
        {
            Validator.CheckNull(doc, "doc");
            Validator.CheckStringNull(nodeName, "nodeName");
            if (!ValidxPathSyntax(xPath))
            {
                throw new Exception("指定的xPath格式不正确！");
            }

            XmlNodeList list = doc.SelectNodes(xPath);
            if (list != null)
            {
                foreach (XmlNode node in list)
                {
                    XmlElement newNode = doc.CreateElement(nodeName);
                    newNode.InnerXml = innerXml;
                    node.AppendChild(newNode);
                }
            }
        }

        /// <summary>
        /// 向指定的xml文档插入节点。
        /// </summary>
        /// <remarks>节点名称为类型的名称。</remarks>
        /// <param name="doc">xml文档。</param>
        /// <param name="xPath">xPath,用于选取在哪些节点中插入新节点。</param>
        /// <param name="source">待插入的对象（将序列化为xml节点）。</param>
        /// <param name="t">待插入对象的类型。</param>
        public static void InsertNode(XmlDocument doc, string xPath, object source, Type t)
        {
            Validator.CheckNull(t, "t");
            InsertNode(doc, xPath, source, t, t.Name);
        }

        /// <summary>
        /// 向指定的xml文档插入节点。
        /// </summary>
        /// <param name="doc">xml文档。</param>
        /// <param name="xPath">xPath,用于选取在哪些节点中插入新节点。</param>
        /// <param name="source">待插入的对象（将序列化为xml节点）。</param>
        /// <param name="t">待插入对象的类型。</param>
        /// <param name="nodeName">待插入节点名。</param>
        public static void InsertNode(XmlDocument doc, string xPath, object source, Type t, string nodeName)
        {
            Validator.CheckNull(doc, "doc");
            Validator.CheckNull(source, "source");
            Validator.CheckNull(t, "t");
            Validator.CheckStringNull(nodeName, "nodeName");

            XmlDocument nodeDoc = ConvertToXml(source, t);
            XmlNode root = nodeDoc.ChildNodes[1];
            InsertNode(doc, xPath, root.InnerXml, nodeName);
        }

        /// <summary>
        /// 载入xml文档。
        /// </summary>
        /// <param name="fileName">文档路径。</param>
        /// <returns>xml文档。</returns>
        public static XmlDocument LoadXml(string fileName)
        {
            Validator.CheckStringNull(fileName, "fileName");

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            return doc;
        }

        /// <summary>
        /// 将xml格式的内容保存成xml文档。
        /// </summary>
        /// <param name="fileName">文档路径。</param>
        /// <param name="content">xml格式的内容。</param>
        public static void SaveXml(string fileName, string content)
        {
            Validator.CheckStringNull(fileName, "fileName");
            Validator.CheckStringNull(content, "content");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            doc.Save(fileName);
        }

        /// <summary>
        /// 生成xml文档的子文档。
        /// </summary>
        /// <remarks>默认的根节点为root。</remarks>
        /// <param name="doc">xml文档。</param>
        /// <param name="xPath">xPath（用于选取由哪些节点构成新的文档）。</param>
        /// <returns>生成的新的xml文档。</returns>
        public static XmlDocument SubXmlDocument(XmlDocument doc, string xPath)
        {
            return SubXmlDocument(doc, xPath, "root");
        }

        /// <summary>
        /// 生成xml文档的子文档。
        /// </summary>
        /// <param name="doc">xml文档。</param>
        /// <param name="xPath">xPath（用于选取由哪些节点构成新的文档）。</param>
        /// <param name="rootName">生成的文档的根节点名。</param>
        /// <returns>生成的新的xml文档。</returns>
        public static XmlDocument SubXmlDocument(XmlDocument doc, string xPath, string rootName)
        {
            Validator.CheckNull(doc, "doc");
            Validator.CheckStringNull(rootName, "rootName");
            if (!ValidxPathSyntax(xPath))
            {
                throw new Exception("指定的xPath格式不正确！");
            }

            //如果是“/"，选取到的是整个文档，直接返回
            if (xPath.Trim() == "/")
            {
                return doc;
            }

            XmlDocument subDoc = new XmlDocument();
            XmlDeclaration declaration = subDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            subDoc.AppendChild(declaration);
            XmlElement root = subDoc.CreateElement(rootName);
            XmlNodeList list = doc.SelectNodes(xPath);
            StringBuilder innerXml = new StringBuilder();
            if (list != null)
            {
                foreach (XmlNode node in list)
                {
                    innerXml.Append(node.OuterXml);
                }
            }
            root.InnerXml = innerXml.ToString();
            subDoc.AppendChild(root);
            return subDoc;
        }

        #endregion

        #region Non-Static Methods

        /// <summary>
        /// 将内部xml文档反序列化为对象。
        /// </summary>
        /// <param name="t">对象类型。</param>
        /// <param name="xPath">xPath,用于指定将哪个节点反序列化为对象。</param>
        /// <returns>反序列化后的对象。</returns>
        public object ConvertToEntity(Type t, string xPath)
        {
            Validator.CheckNull(t, "t");

            return ConvertToEntity(this.doc, t, t.Name, xPath);
        }

        /// <summary>
        /// 将内部xml文档反序列化为对象。
        /// </summary>
        /// <param name="t">对象类型。</param>
        /// <param name="typeName">对象类型名称（对于有些继承类型，通过反射
        /// 得到的类型名可能和声明的不同）。</param>
        /// <param name="xPath">xPath,用于指定将哪个节点反序列化为对象。</param>
        /// <returns>反序列化后的对象。</returns>
        public object ConvertToEntity(Type t, string typeName, string xPath)
        {
            return ConvertToEntity(this.doc, t, typeName, xPath);
        }

        /// <summary>
        /// 返回内部xml文档的内容。
        /// </summary>
        /// <returns>文档内容。</returns>
        public string GetXmlContent()
        {
            if (doc != null)
            {
                return doc.OuterXml;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 向内部xml文档插入节点。
        /// </summary>
        /// <param name="xPath">xPath,用于选取在哪些节点内插入新节点。</param>
        /// <param name="innerXml">待插入节点的xml格式内容（不含节点名）。</param>
        /// <param name="nodeName">待插入节点名。</param>
        public void InsertNode(string xPath, string innerXml, string nodeName)
        {
            InsertNode(this.doc, xPath, innerXml, nodeName);
        }

        /// <summary>
        /// 向内部xml文档插入节点。
        /// </summary>
        /// <remarks>节点名称为类型的名称。</remarks>
        /// <param name="xPath">xPath,用于选取在哪些节点中插入新节点。</param>
        /// <param name="source">待插入的对象（将序列化为xml节点）。</param>
        /// <param name="t">待插入对象的类型。</param>
        public void InsertNode(string xPath, object source, Type t)
        {
            Validator.CheckNull(t, "t");
            InsertNode(this.doc, xPath, source, t, t.Name);
        }

        /// <summary>
        /// 向内部xml文档插入节点。
        /// </summary>
        /// <param name="xPath">xPath,用于选取在哪些节点中插入新节点。</param>
        /// <param name="source">待插入的对象（将序列化为xml节点）。</param>
        /// <param name="t">待插入对象的类型。</param>
        /// <param name="nodeName">待插入节点名。</param>
        public void InsertNode(string xPath, object source, Type t, string nodeName)
        {
            InsertNode(this.doc, xPath, source, t, nodeName);
        }

        /// <summary>
        /// 将指定xml文档内容载入内部xml文档。
        /// </summary>
        /// <param name="content">xml内容。</param>
        public void LoadContent(string content)
        {
            doc = new XmlDocument();
            doc.LoadXml(content);
        }

        /// <summary>
        /// 保存内部xml文档。
        /// </summary>
        /// <param name="fileName">文档路径。</param>
        public void SaveXml(string fileName)
        {
            Validator.CheckStringNull(fileName, "fileName");

            doc.Save(fileName);
        }

        /// <summary>
        /// 生成内部xml文档的子文档。
        /// </summary>
        /// <remarks>默认的根节点为root。</remarks>
        /// <param name="xPath">xPath（用于选取由哪些节点构成新的文档）。</param>
        /// <returns>生成的新的xml文档。</returns>
        public XmlDocument SubXmlDocument(string xPath)
        {
            return SubXmlDocument(xPath, "root");
        }

        /// <summary>
        /// 生成内部xml文档的子文档。
        /// </summary>
        /// <param name="xPath">xPath（用于选取由哪些节点构成新的文档）。</param>
        /// <param name="rootName">生成的文档的根节点名。</param>
        /// <returns>生成的新的xml文档。</returns>
        public XmlDocument SubXmlDocument(string xPath, string rootName)
        {
            return SubXmlDocument(this.doc, xPath, rootName);
        }

        #endregion

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
