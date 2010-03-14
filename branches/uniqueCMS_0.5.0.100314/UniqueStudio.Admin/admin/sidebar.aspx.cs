using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.Permission;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin
{
    public partial class sidebar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
                if (user != null)
                {
                    ltlAdvancedMenus.Visible = RoleManager.IsUserInRole(user, "超级管理员");
                }
            }
        }
    }
}
