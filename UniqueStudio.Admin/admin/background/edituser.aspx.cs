using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Core.Permission;
using UniqueStudio.Core.User;

namespace UniqueStudio.Admin.admin.background
{
    public partial class edituser : Controls.BasePage
    {
        private Guid userId;
        private Dictionary<string, bool> inRoles; 

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

            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            UserInfo user = (new UserManager(CurrentUser)).GetEntireUserInfo(userId);
            ltlEmail.Text = user.Email;
            ltlUserName.Text = user.UserName;
            ltlCreateTime.Text = user.CreateDate.ToString("yyyy-MM-dd");
            ltlLastActivityDate.Text = user.LastActivityDate.ToString("yyyy-MM-dd");
            chbIsApproved.Checked = user.IsApproved;
            chbIsLockedOut.Checked = user.IsLockedOut;
            chbIsOnline.Checked = user.IsOnline;

            inRoles = new Dictionary<string, bool>(user.Roles.Count);
            foreach (RoleInfo role in user.Roles)
            {
                inRoles.Add(role.RoleName, true);
            }

            rptRoles.ItemDataBound += new RepeaterItemEventHandler(rptRoles_ItemDataBound);
            rptRoles.DataSource = (new RoleManager()).GetAllRoles(CurrentUser);
            rptRoles.DataBind();

            rptPermissions.DataSource = user.Permissions;
            rptPermissions.DataBind();
        }

        void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (inRoles != null)
            {
                Literal ltlSelected = (Literal)e.Item.FindControl("ltlSelected");
                if (ltlSelected != null)
                {
                    ltlSelected.Text = inRoles.ContainsKey(((RoleInfo)e.Item.DataItem).RoleName) ? "checked='checked'" : string.Empty;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            RoleManager manager = new RoleManager();

            UserInfo user = new UserInfo(userId);
            user.Roles = new RoleCollection();
            if (Request.Form["chkSelected_r"] != null)
            {
                string[] ids = Request.Form["chkSelected_r"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    user.Roles.Add(new RoleInfo(ids[i]));
                }
            }

            try
            {
                if (manager.UpdateRolesForUser(CurrentUser, user))
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
