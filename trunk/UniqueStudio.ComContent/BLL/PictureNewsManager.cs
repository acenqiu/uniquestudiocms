using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Model;

namespace UniqueStudio.ComContent.BLL
{
    public class PictureNewsManager
    {
        private static string itemPrototype = "<item title=\"{0}\" img=\"{1}\" url=\"{2}\" target='_blank' />";
        private static string XML_PATH = Path.Combine(GlobalConfig.BasePhysicalPath, @"xml\viewerData.xml");
        private static string XML_ROOT = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ArrayOfErrorInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></ArrayOfErrorInfo>";

        public static int CATEGOEY_ID = 3;

        public static void UpdatePictureNews(string url)
        {
            WriteXMLContent(url, GetXMLContent());
        }

        public static void UpdatePictureNews()
        {
            UpdatePictureNews(XML_PATH);
        }

        public static string GetXMLContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version='1.0' encoding='utf-8'?>").Append("\r\n");
            sb.Append("<viewer interval='4000' isRandom='1'>").Append("\r\n");
            PostManager postManager = new PostManager();

            List<PostInfo> postList = postManager.GetPostListByCatId(1, 1, 3, false, PostListType.PublishedOnly, CATEGOEY_ID);


            foreach (PostInfo post in postList)
            {
                if (post.NewsImage != null && post.NewsImage != string.Empty)
                {
                    string[] param = { post.Title, post.NewsImage, "view.aspx?uri=" + post.Uri };
                    sb.Append(String.Format(itemPrototype, param)).Append("\r\n");
                }
            }
            sb.Append("</viewer>").Append("\r\n");
            return sb.ToString();
        }

        public static void WriteXMLContent(string url, string content)
        {
            XmlManager manager = new XmlManager();
            manager.SaveXml(url, content);
        }

        public static void LogError(Exception e)
        {
            LogError(e, string.Empty);
        }

        public static void LogError(Exception e, string remarks)
        {
            XmlManager manager = new XmlManager();
            if (!File.Exists(XML_PATH))
            {
                try
                {
                    manager.SaveXml(XML_PATH, XML_ROOT);
                }
                catch
                {
                    return;
                }
            }
            XmlDocument doc = manager.LoadXml(XML_PATH);

            ErrorInfo error = new ErrorInfo();
            error.ExceptionType = e.GetType().FullName;
            error.ErrorMessage = e.Message;
            if (e.InnerException != null)
            {
                error.InnerExceptionType = e.InnerException.GetType().FullName;
                error.InnerErrorMessage = e.InnerException.Message;
            }
            error.RepresentationString = e.ToString();
            error.Time = DateTime.Now;
            error.Remarks = remarks;

            manager.InsertNode(doc, "/ArrayOfErrorInfo", error, typeof(ErrorInfo));
            doc.Save(XML_PATH);
        }

        public static void LogError(string errorType, string message, string remarks)
        {
            XmlManager manager = new XmlManager();
            if (!File.Exists(XML_PATH))
            {
                try
                {
                    manager.SaveXml(XML_PATH, XML_ROOT);
                }
                catch
                {
                    return;
                }
            }
            XmlDocument doc = manager.LoadXml(XML_PATH);

            ErrorInfo error = new ErrorInfo();
            doc.Save(XML_PATH);
        }

        public static List<PictureNewsItem> GetAllNews()
        {
            XmlManager manager = new XmlManager();
            if (!File.Exists(XML_PATH))
            {
                try
                {
                    manager.SaveXml(XML_PATH, XML_ROOT);
                    return new List<PictureNewsItem>();
                }
                catch
                {
                    return null;
                }
            }
            XmlDocument doc = manager.LoadXml(XML_PATH);
            XmlNodeList list = doc.GetElementsByTagName("item");
            List<PictureNewsItem> picList = new List<PictureNewsItem>();
            foreach (XmlNode node in list)
            {
                PictureNewsItem item = new PictureNewsItem() { Title = node.Attributes["title"].Value, Url = node.Attributes["url"].Value, ImageUrl = node.Attributes["img"].Value };
                picList.Add(item);
            }
            return picList;
        }
    }
}
