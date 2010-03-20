using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin
{
    public partial class top : Controls.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUser != null)
                {
                    ltlUserName.Text = CurrentUser.UserName;
                    //修改
                    ltlEnableTime.Visible = SiteManager.Config(1).IsDisplayTime;
                }

                rptSiteList.DataSource = (new SiteManager()).GetAllSites();
                rptSiteList.DataBind();
            }
        }
    }
}
