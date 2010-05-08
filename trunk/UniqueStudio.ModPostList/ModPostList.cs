using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Module;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.Site;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.ComContent;

namespace UniqueStudio.ModPostList
{
    public class ModPostList : IModule
    {
        private const string CATEGORY_ID = "CategoryId";
        private const string TIME_FORMAT = "TimeFormat";
        private const string NUMBER = "Number";
        private const string MAX_TITLE_LENGTH = "MaxTitleLength";
        //{0}：分类名称
        //{1}：分类ID
        //{2}：条目
        private const string MAIN = "<div class=\"column-head\"><span>{0}<a href=\"list.aspx?catId={1}\">more...</a></span></div>\r\n"
                                                        + "<div class=\"column-content\">\r\n<ul>\r\n{2}</ul>\r\n</div>";
        //{0}：分类ID
        //{1}：文章URI
        //{2}：文章标题
        //{3}：新文章标志
        //{4}：已截取的文章标题
        //{5}：发表时间
        private const string ITEM = "<li><a href='view.aspx?catId={0}&uri={1}' title='{2}' {3}>{4}</a><span class=\"postdate\">{5}</span></li>\r\n";

        #region IModule Members

        public string RenderContents(int siteId, string controlName, NameValueCollection queryString)
        {
            try
            {
                //获取配置信息
                int categoryId = Converter.IntParse(ModuleControlManager.GetConfigValue(siteId, controlName, CATEGORY_ID), 1);
                int number = Converter.IntParse(ModuleControlManager.GetConfigValue(siteId, controlName, NUMBER), 8);
                string timeFormat = ModuleControlManager.GetConfigValue(siteId, controlName, TIME_FORMAT);
                int maxTitleLength = Converter.IntParse(ModuleControlManager.GetConfigValue(siteId, controlName, MAX_TITLE_LENGTH), 12);

                CategoryInfo category = (new CategoryManager()).GetCategory(categoryId);
                if (category != null)
                {
                    StringBuilder items = new StringBuilder();
                    PostCollection posts = (new PostManager()).GetPostListByCatId(1, number, categoryId, false);
                    if (posts != null)
                    {
                        DateTime earliest = DateTime.Now.AddDays(-ConfigAdapter.Config(siteId).NewImageThreshold);
                        foreach (PostInfo post in posts)
                        {
                            items.Append(string.Format(ITEM, categoryId
                                                                                , post.Uri
                                                                                , post.Title
                                                                                , post.LastEditDate >= earliest ? "class='new'" : ""
                                                                                , post.Title.Length > maxTitleLength ? post.Title.Substring(0, maxTitleLength) : post.Title
                                                                                , post.CreateDate.ToString(timeFormat)));
                        }
                    }
                    return string.Format(MAIN, category.CategoryName, categoryId, items.ToString());
                }
                else
                {
                    return string.Format(MAIN, "分类ID设置错误", "", "");
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        public ModPostList()
        {
        }
    }
}
