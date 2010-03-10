﻿using System;
using System.Text;
using UniqueStudio.Core.Category;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.ComContent.PL
{
    public partial class list : System.Web.UI.Page
    {
        protected DateTime dt;

        protected override void OnPreInit(EventArgs e)
        {
            dt = DateTime.Now;
            base.OnPreInit(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            TimeSpan ts = DateTime.Now - dt;
            Response.Write("<!--query time:" + ts.TotalMilliseconds + "-->");
        }

        protected int CategoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryId = Utility.IntParse(Request.QueryString["catId"], 1);
                int pageIndex = Utility.IntParse(Request.QueryString["page"], 1);
                int pageSize = Utility.IntParse(Request.QueryString["size"], WebSiteConfig.PageSizeOfSectionPostList);


                CategoryManager catManager = new CategoryManager();
                CategoryInfo category = catManager.GetCategoryPath(CategoryId);
                if (category != null)
                {
                    //设置网页标题
                    Page.Header.Title = category.CategoryName + " - " + WebSiteConfig.WebName;

                    categories.CategoryId = category.CategoryId;

                    StringBuilder sb = new StringBuilder();
                    while (category != null)
                    {
                        sb.Append(string.Format("-><a href='list.aspx?catId={0}'>{1}</a>  ", category.CategoryId, category.CategoryName));
                        category = category.ChildCategory;
                    }
                    ltlCategoryLink.Text = sb.ToString();
                }
                else
                {
                    Response.Redirect(WebSiteConfig.BaseAddress + "/404.aspx");
                }

                PostManager postManager = new PostManager();
                PostCollection posts = postManager.GetPostListByCatId(pageIndex, pageSize, false, PostListType.PublishedOnly, CategoryId);
                if (posts != null)
                {
                    rptList.DataSource = posts;
                    rptList.DataBind();

                    pagination.CurrentPage = posts.PageIndex;
                    pagination.Count = posts.PageCount;
                }
            }
        }
    }
}