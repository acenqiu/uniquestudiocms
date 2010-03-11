using System;
using System.Collections.Generic;

using UniqueStudio.Core.Permission;
using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class rolelist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];

                GetData();
                PermissionManager manager = new PermissionManager();
                cblPermissions.DataSource = manager.GetAllPermissions(currentUser);
                cblPermissions.DataTextField = "PermissionName";
                cblPermissions.DataValueField = "PermissionName";
                cblPermissions.DataBind();
            }
        }

        private void GetData()
        {
            UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            RoleManager manager = new RoleManager();
            rptList.DataSource = manager.GetAllRoles(currentUser);
            rptList.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            RoleManager manager = new RoleManager();
            RoleInfo role = new RoleInfo();
            role.RoleName = txtRoleName.Text.Trim();
            role.Description = txtDescription.Text.Trim();
            role.Permissions = new PermissionCollection();

            for (int i = 0; i < cblPermissions.Items.Count; i++)
            {
                if (cblPermissions.Items[i].Selected)
                {
                    role.Permissions.Add(new PermissionInfo(cblPermissions.Items[i].Value));
                }
            }

            role = manager.CreateRole(currentUser, role);
            if (role != null)
            {
                message.SetSuccessMessage("角色创建成功");
                GetData();
                txtRoleName.Text = "";
                txtDescription.Text = "";
            }
            else
            {
                message.SetErrorMessage("角色创建失败");
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            RoleManager manager = new RoleManager();
            List<int> list = new List<int>();
            if (Request.Form["chkSelected"] != null)
            {
                string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    list.Add(Utility.IntParse(ids[i], 0));
                }
            }
            else
            {
                return;
            }

            if (ddlOperation.SelectedValue == "delete")
            {
                try
                {
                    if (manager.DeleteRoles(currentUser, list.ToArray()))
                    {
                        message.SetSuccessMessage("指定角色已删除！");
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage(ex.Message);
                }
                GetData();
            }
        }
    }
}
