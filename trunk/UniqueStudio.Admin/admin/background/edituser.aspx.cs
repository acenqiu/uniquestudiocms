using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.Permission;
using UniqueStudio.Core.User;
using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class edituser : System.Web.UI.Page
    {
        private UserInfo currentUser;
        private Guid userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                try
                {
                    userId = new Guid(Request["id"]);
                }
                catch
                {
                    message.SetErrorMessage("参数错误！");
                }
            }
            else
            {
                Response.Redirect("userlist.aspx");
            }

            currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];

            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            UserInfo user = (new UserManager(currentUser)).GetEntireUserInfo(userId);
            ltlEmail.Text = user.Email;
            ltlUserName.Text = user.UserName;
            ltlCreateTime.Text = user.CreateDate.ToString("yyyy-MM-dd");
            ltlLastActivityDate.Text = user.LastActivityDate.ToString("yyyy-MM-dd");
            chbIsApproved.Checked = user.IsApproved;
            chbIsLockedOut.Checked = user.IsLockedOut;
            chbIsOnline.Checked = user.IsOnline;

            Dictionary<string, bool> inRoles = new Dictionary<string, bool>(user.Roles.Count);
            foreach (RoleInfo role in user.Roles)
            {
                inRoles.Add(role.RoleName, true);
            }

            RoleCollection roles = (new RoleManager()).GetAllRoles(user);
            cblRoles.DataSource = roles;
            cblRoles.DataTextField = "RoleName";
            cblRoles.DataValueField = "RoleName";
            cblRoles.DataBind();
            foreach (ListItem item in cblRoles.Items)
            {
                item.Selected = inRoles.ContainsKey(item.Value);
            }

            rptPermissions.DataSource = user.Permissions;
            rptPermissions.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            RoleManager manager = new RoleManager();

            UserInfo user = new UserInfo(userId);
            user.Roles = new RoleCollection();
            foreach (ListItem item in cblRoles.Items)
            {
                if (item.Selected)
                {
                    user.Roles.Add(new RoleInfo(item.Value));
                }
            }

            try
            {
                if (manager.UpdateRolesForUser(currentUser, user))
                {
                    message.SetSuccessMessage("用户更新成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("用户更新失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlist.aspx");
        }
    }
}
