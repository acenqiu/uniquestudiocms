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
    public partial class editpermission : System.Web.UI.Page
    {
        private int permissionId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["permissionId"] != null)
                {
                    permissionId = Convert.ToInt32(Request.QueryString["permissionId"]);

                    PermissionManager manager = new PermissionManager();
                    PermissionInfo permission = manager.GetPermissionInfo(permissionId);
                    ltlPermissionName.Text = permission.PermissionName;
                    txtPermissionName.Text = permission.PermissionName;
                    txtDescription.Text = permission.Description;
                    txtProvider.Text = permission.Provider;
                }
                else
                {
                    if (Request.QueryString["ret"] != null)
                    {
                        Response.Redirect("permissionlist.aspx?ret=" + HttpUtility.HtmlDecode(Request.QueryString["ret"]));
                    }
                    else
                    {
                        Response.Redirect("permissionlist.aspx");
                    }
                }
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ret"] != null)
            {
                Response.Redirect("permissionlist.aspx?" + HttpUtility.UrlDecode(Request.QueryString["ret"]));
            }
            else
            {
                Response.Redirect("permissionlist.aspx");
            }
        }
    }
}
