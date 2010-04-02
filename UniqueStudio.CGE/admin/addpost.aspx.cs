using System;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.DAL.Uri;

namespace UniqueStudio.ComContent.Admin
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
