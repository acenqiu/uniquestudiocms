using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Controls
{
    public class AdminBasePage : System.Web.UI.Page
    {
        protected UserInfo CurrentUser;
        protected int SiteId;

        protected override void OnLoad(EventArgs e)
        {
            CurrentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            SiteId = Converter.IntParse(Request.QueryString["siteId"], 0);
            base.OnLoad(e);
        }
    }
}
