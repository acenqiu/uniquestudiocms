using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin
{
    public partial class top : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
                if (user != null)
                {
                    ltlUserName.Text = user.UserName;
                    ltlEnableTime.Visible = SiteManager.Config(1).IsDisplayTime;
                }
            }
        }
    }
}
