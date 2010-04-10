//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：后台管理顶部页面。
// 完成日期：2010年04月10日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin
{
    public partial class top : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUser != null)
                {
                    ltlUserName.Text = CurrentUser.UserName;
                    ltlEnableTime.Visible = ServerConfig.IsDisplayTime;
                }

                rptSiteList.DataSource = (new SiteManager()).GetAllSites();
                rptSiteList.DataBind();
            }
        }
    }
}
