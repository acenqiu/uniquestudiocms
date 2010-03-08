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
    public partial class addpost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
                PostPermissionManager ppm = new PostPermissionManager();
                if (!ppm.HasAddPermission(user))
                {
                    //Response.Redirect("PostPermissionError.aspx?Error=添加文章&Page=" + Request.UrlReferrer.ToString());
                }
            }
        }
    }
}
