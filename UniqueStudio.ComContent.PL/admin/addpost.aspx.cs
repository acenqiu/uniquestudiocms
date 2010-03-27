using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniqueStudio.DAL.Uri;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.ComContent.BLL;

namespace UniqueStudio.ComContent.PL
{
    public partial class addpost : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            editor.SiteId = SiteId;
            if (!IsPostBack)
            {
                if (!PostPermissionManager.HasAddPermission(CurrentUser, SiteId))
                {
                }
                Session["posturi"] = UriProvider.GetNewUri(ResourceType.Article);
            }
        }
    }
}
