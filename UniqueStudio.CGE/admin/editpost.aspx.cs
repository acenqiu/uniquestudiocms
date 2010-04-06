using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Utilities;
using UniqueStudio.ComContent.BLL;

namespace UniqueStudio.ComContent.Admin
{
    public partial class editpost : Controls.AdminBasePage
    {
        protected string PostListQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
            editor.SiteId = SiteId;
            PostListQuery = PathHelper.CleanUrlQueryString(HttpUtility.UrlDecode(Request.QueryString["ret"]),
                                                                                            new string[] { "msg", "msgtype" });
            if (string.IsNullOrEmpty(PostListQuery))
            {
                PostListQuery = "siteId=" + SiteId.ToString();
            }

            long uri = Converter.LongParse(Request.QueryString["uri"], 0);
            editor.Uri = uri;

            if (!IsPostBack && !PostPermissionManager.HasEditPermission(CurrentUser, uri))
            {
                Response.Redirect("PostPermissionError.aspx?Error=编辑文章&Page=" + Request.UrlReferrer.ToString());
            }
        }
    }
}
