using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.ComContent.Admin
{
    public partial class deletepost : Controls.AdminBasePage
    {
        private PostManager bll = new PostManager();
        private long uri;

        protected void Page_Load(object sender, EventArgs e)
        {
            uri = Converter.LongParse(Request.QueryString["uri"], 0);

            if (!IsPostBack)
            {
                if (uri == 0)
                {
                    Return();
                }
                else
                {
                    if (!PostPermissionManager.HasDeletePermission(CurrentUser, uri))
                    {
                        Response.Redirect("PostPermissionError.aspx?Error=删除文章&Page=" + Request.UrlReferrer.ToString());
                    }
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if ((new PostManager()).DeletePost(CurrentUser, uri))
                {
                    Return();
                }
                else
                {
                    message.SetErrorMessage("文章删除失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("文章删除失败：" + ex.Message);
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Return();
        }

        private void Return()
        {
            if (Request.QueryString["ret"] != null)
            {
                string query = PathHelper.CleanUrlQueryString(HttpUtility.UrlDecode(Request.QueryString["ret"]),
                                                                                                new string[] { "msg", "msgtype" });
                Response.Redirect("postlist.aspx?" + query);
            }
            else
            {
                Response.Redirect("postlist.aspx?siteId=" + SiteId);
            }
        }
    }
}
