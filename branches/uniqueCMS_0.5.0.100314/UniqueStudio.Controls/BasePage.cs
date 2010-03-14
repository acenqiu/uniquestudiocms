using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

using UniqueStudio.Common.Model;

namespace UniqueStudio.Controls
{
    public class BasePage : System.Web.UI.Page
    {
        protected UserInfo CurrentUser;

        protected override void OnLoad(EventArgs e)
        {
            CurrentUser = (UserInfo)this.Session[Common.Config.GlobalConfig.SESSION_USER];
            base.OnLoad(e);
        }
    }
}
