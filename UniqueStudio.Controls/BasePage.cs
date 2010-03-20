using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Controls
{
    public class BasePage : System.Web.UI.Page
    {
        protected UserInfo CurrentUser;
        protected int SiteId;

        protected override void OnLoad(EventArgs e)
        {
            CurrentUser = (UserInfo)this.Session[Common.Config.GlobalConfig.SESSION_USER];
            SiteId = Converter.IntParse(Request.QueryString["siteId"], 0);
            base.OnLoad(e);
        }
    }
}
