using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.ComContent.BLL;

namespace UniqueStudio.ComContent.PL
{
    public partial class addpost : Controls.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!PostPermissionManager.HasAddPermission(CurrentUser, SiteId))
                {
                    //Response.Redirect("PostPermissionError.aspx?Error=添加文章&Page=" + Request.UrlReferrer.ToString());
                }
            }
        }
    }
}
