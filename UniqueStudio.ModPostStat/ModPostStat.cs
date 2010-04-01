using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Module;

namespace UniqueStudio.ModPostStat
{
    public class ModPostStat : IModule
    {
        private const string COLUMN_TITLE = "ColumnTitle";
        private const string CATEGORY_IDS = "CategoryIds";
        private const string POST_STAT_BY_YEAR = "PostStatByYear";
        //{0}：条目
        private const string MAIN = "<div class=\"column-head\"><span>{0}</span></div>\r\n"
                                                    + "<div class=\"column-content\">\r\n<ul>\r\n{1}</ul>\r\n</div>\r\n";
        //{0}：分类ID
        //{1}：起始时间（年）
        //{2}：终止时间
        //{3}：数量
        private const string ITEM_BY_YEAR = "<li><a href='search.aspx?catId={0}&start={1}-1&end={2}-1'>{1}年（{3}）</a></li>\r\n";
        //{0}：分类ID
        //{1}：年
        //{2}：月
        //{3}：终止时间
        //{4}：数量
        private const string ITEM_BY_MONTH = "<li><a href='search.aspx?catId={0}&start={1}-{2}&end={3}'>{1}年{2}月（{4}）</a></li>\r\n";

        #region IModule Members

        public string RenderContents(int siteId, string controlName, NameValueCollection queryString)
        {
            try
            {
                string columnTitle = ModuleControlManager.GetConfigValue(siteId, controlName, COLUMN_TITLE);
                string categoryIds = ModuleControlManager.GetConfigValue(siteId, controlName, CATEGORY_IDS) + ",";
                bool isByYear = Converter.BoolParse(ModuleControlManager.GetConfigValue(siteId, controlName, POST_STAT_BY_YEAR), false);

                int categoryId = Converter.IntParse(queryString["catId"], 0);
                if (categoryId == 0 || (categoryIds != "*," && categoryIds.IndexOf(queryString["catId"] + ",") < 0))
                {
                    return string.Empty;
                }

                StringBuilder items = new StringBuilder();
                PostStatCollection collection = (new PostManager()).GetPostStat(categoryId, isByYear);
                if (collection != null)
                {
                    if (isByYear)
                    {
                        foreach (PostStatInfo item in collection)
                        {
                            items.Append(string.Format(ITEM_BY_YEAR, categoryId
                                                                                , item.Year
                                                                                , item.Year + 1
                                                                                , item.Count));
                        }
                    }
                    else
                    {
                        foreach (PostStatInfo item in collection)
                        {
                            items.Append(string.Format(ITEM_BY_MONTH, categoryId
                                                                                , item.Year
                                                                                , item.Month
                                                                                , item.Month == 12 ? (item.Year + 1) + "-1" : item.Year + "-" + (item.Month + 1)
                                                                                , item.Count));
                        }
                    }
                }

                return string.Format(MAIN, columnTitle, items.ToString());
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        public ModPostStat()
        {
        }
    }
}
