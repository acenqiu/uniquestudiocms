using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.ComContent.PL
{
    public partial class postlist : System.Web.UI.Page
    {
        private PostManager bll;
        private PostListType postListType = PostListType.Both;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int pageIndex = Converter.IntParse(Request.QueryString["page"], 1);
                UserInfo user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
                bll = new PostManager();
                PostCollection collection = bll.GetPostList(pageIndex, 10, false, postListType, true, user);

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
