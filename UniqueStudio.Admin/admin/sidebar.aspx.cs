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
    public partial class sidebar : Controls.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltlAdvancedMenus.Visible = RoleManager.IsUserInRole(CurrentUser.UserId, "超级管理员");
            }
        }
    }
}
