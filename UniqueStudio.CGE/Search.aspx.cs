﻿using System;
using System.Text.RegularExpressions;
using System.Web;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Site;

namespace UniqueStudio.CGE
{
    public partial class Search : Controls.PlBasePage
    {
        private const string ErrorMessage = "<div class=\"error\">{0}</div>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Header.Title = SiteManager.Config(SiteId).WebName;

                PostCollection collection = null;
                PostManager manager = new PostManager();
                try
                {
                    string key = string.Empty;
                    if (Request.QueryString["title"] != null)
                    {
                        //根据标题搜索
                        key = HttpUtility.UrlDecode(Request.QueryString["title"]);
                        collection = manager.SearchPostsByTitle(SiteId, key);
                        if (collection != null)
                        {
                            Regex r = new Regex("(?<v>" + key + ")");
                            foreach (PostInfo post in collection)
                            {
                                post.Title = r.Replace(post.Title, "<strong>${v}</strong>");
                            }
                        }
                    }
                    else if (Request.QueryString["author"] != null)
                    {
                        //根据作者搜索
                        key = HttpUtility.UrlDecode(Request.QueryString["author"]);
                        collection = manager.SearchPostsByAuthor(SiteId, key);
                    }
                    else if (Request.QueryString["start"] != null && Request.QueryString["end"] != null)
                    {
                        //根据时间搜索
                        DateTime startTime = Converter.DatetimeParse(Request.QueryString["start"], DateTime.MinValue);
                        DateTime endTime = Converter.DatetimeParse(Request.QueryString["end"], DateTime.MinValue);
                        int categoryId = Converter.IntParse(Request.QueryString["catId"], 0);
                        collection = manager.SearchPostsByTime(SiteId, startTime, endTime, categoryId);
                        if (categoryId != 0)
                        {
                            categories.CategoryId = categoryId;
                            divSlider.Visible = true;
                        }
                    }
                    else
                    {
                        ltlMessage.Text = string.Format(ErrorMessage, "搜索条件不正确！");
                    }

                    if (collection != null)
                    {
                        rptList.DataSource = collection;
                        rptList.DataBind();
                        ltlCount.Text = collection.Count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ltlMessage.Text = string.Format(ErrorMessage, ex.Message);
                }
            }
        }
    }
}
