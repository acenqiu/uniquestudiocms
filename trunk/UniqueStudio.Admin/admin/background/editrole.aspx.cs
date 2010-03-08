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
                RoleManager manager = new RoleManager();
                RoleInfo role = manager.GetRoleInfo(roleId);

                ltlRoleName.Text = role.RoleName;
                txtRoleName.Text = role.RoleName;
                txtDescription.Text = role.Description;

                PermissionManager permissionManager = new PermissionManager();
                rptPermissions.DataSource = role.Permissions;
                rptPermissions.DataBind();

                //for (int i = 0; i < rptPermissions.Items.Count; i++)
                //{
                //    CheckBox chk = (CheckBox)rptPermissions.Items[i].Controls[0];
                //    TextBox txt = (TextBox)rptPermissions.Items[i].Controls[1];

                //    if (role.Permissions != null && txt != null)
                //    {
                //        foreach (PermissionInfo permission in role.Permissions)
                //        {
                //            if (txt.Text == permission.PermissionName)
                //            {
                //                if (chk != null)
                //                {
                //                    chk.Checked = true;
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}

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
