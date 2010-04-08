using System;
using System.Text;

using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Site;

namespace UniqueStudio.ComContent.BLL
{
    public class PictureNewsManager
    {
        private const string ITEM_PROTOTYPE = "<item title=\"{0}\" img=\"{1}\" url=\"{2}\" target='_blank' />\r\n";

        public static void UpdatePictureNews(int siteId)
        {
            try
            {
                string path = PathHelper.PathCombine(SiteManager.BasePhysicalPath(siteId), @"xml\viewerData.xml");

                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
                sb.Append(string.Format("<viewer interval=\"{0}\" isRandom=\"{1}\">\r\n", ConfigAdapter.Config(siteId).PictureNewsInterval
                                                                                                                                   , ConfigAdapter.Config(siteId).PictureNewsIsRandom ? "1" : "0"));
                PostManager postManager = new PostManager();
                PostCollection postList = postManager.GetPostListByCatId(1
                                                                                                        , ConfigAdapter.Config(siteId).PictureNewsNumber * 2
                                                                                                        , ConfigAdapter.Config(siteId).PictureNewsCategoryId
                                                                                                        , false);
                int count = 0;
                foreach (PostInfo post in postList)
                {
                    if (!string.IsNullOrEmpty(post.NewsImage))
                    {
                        string[] param = { post.Title, post.NewsImage, "view.aspx?uri=" + post.Uri };
                        sb.Append(String.Format(ITEM_PROTOTYPE, param));
                        count++;
                        if (count == ConfigAdapter.Config(siteId).PictureNewsNumber)
                        {
                            break;
                        }
                    }
                }
                sb.Append("</viewer>\r\n");

                (new XmlManager()).SaveXml(path, sb.ToString());
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
            }
        }
    }
}
