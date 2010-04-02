﻿using System;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.ComContent.PL
{
    public partial class postlist : Controls.AdminBasePage
    {
        private PostManager bll;
        private PostListType postListType = PostListType.Both;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int pageIndex = Converter.IntParse(Request.QueryString["page"], 1);
                PostCollection collection = (new PostManager()).GetPostList(CurrentUser, SiteId, pageIndex, 10, false, postListType, true);
                if (collection == null)
                {
                    rptList.DataSource = null;
                }
                else
                {
                    if (collection.Count == 0)
                    {
                        message.SetSuccessMessage("当前没有文章");
                    }
                    else
                    {
                        rptList.DataSource = collection;
                        rptList.DataBind();

                        pagination.Count = collection.PageCount;
                        pagination.CurrentPage = collection.PageIndex;
                    }
                }
            }
        }
    }
}