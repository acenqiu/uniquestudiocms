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

namespace UniqueStudio.Admin.admin.background
{
    public partial class editrole : System.Web.UI.Page
    {
        private int roleId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["roleId"] != null)
            {
                roleId = Convert.ToInt32(Request.QueryString["roleId"]);
            }
            else
            {
                if (Request.QueryString["ret"] != null)
                {
                    Response.Redirect("rolelist.aspx?" + HttpUtility.UrlDecode(Request.QueryString["ret"]));
                }
                else
                {
                    Response.Redirect("rolelist.aspx");
                }
            }
            if (!IsPostBack)
            {
                UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
                RoleManager manager = new RoleManager();
                RoleInfo role = manager.GetRoleInfo(currentUser, roleId);

                ltlRoleName.Text = role.RoleName;
                txtRoleName.Text = role.RoleName;
                txtDescription.Text = role.Description;

                PermissionManager permissionManager = new PermissionManager();
                rptPermissions.DataSource = role.Permissions;
                rptPermissions.DataBind();

                rptUsers.DataSource = role.Users;
                rptUsers.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ret"] != null)
            {
                Response.Redirect("rolelist.aspx?" + HttpUtility.UrlDecode(Request.QueryString["ret"]));
            }
            else
            {
                Response.Redirect("rolelist.aspx");
            }
        }
    }
}
