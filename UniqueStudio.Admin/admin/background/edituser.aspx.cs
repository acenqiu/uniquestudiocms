//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：编辑用户信息页面。
// 完成日期：2010年03月16日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Permission;
using UniqueStudio.Core.User;

namespace UniqueStudio.Admin.admin.background
{
    public partial class edituser : Controls.AdminBasePage
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
            try
            {
                UserInfo user = (new UserManager(CurrentUser)).GetEntireUserInfo(userId);
                ltlEmail.Text = user.Email;
                ltlUserName.Text = user.UserName;
                ltlCreateTime.Text = user.CreateDate.ToString("yyyy-MM-dd");
                ltlLastActivityDate.Text = user.LastActivityDate.ToString("yyyy-MM-dd");
                chkIsApproved.Checked = user.IsApproved;
                chkIsLockedOut.Checked = user.IsLockedOut;
                chkIsOnline.Checked = user.IsOnline;
                btnLock.Text = user.IsLockedOut ? "解除锁定" : "锁定";

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
            catch (Exception ex)
            {
                message.SetErrorMessage("数据获取失败：" + ex.Message);
            }
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

        protected void btnLock_Click(object sender, EventArgs e)
        {
            UserManager manager = new UserManager(CurrentUser);
            try
            {
                if (btnLock.Text == "锁定")
                {
                    if (manager.LockUser(userId))
                    {
                        message.SetSuccessMessage("该用户锁定成功！");
                        chkIsLockedOut.Checked = true;
                    }
                    else
                    {
                        message.SetErrorMessage("该用户锁定失败！");
                    }
                }
                else
                {
                    if (manager.UnLockUser(userId))
                    {
                        message.SetSuccessMessage("该用户解除锁定成功！");
                        chkIsLockedOut.Checked = false;
                    }
                    else
                    {
                        message.SetErrorMessage("该用户解除锁定失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(string.Format("该用户{0}失败：{1}", btnLock.Text ,ex.Message));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInfo user = new UserInfo(userId);
            user.Roles = new RoleCollection();
            if (Request.Form["chkSelected_r"] != null)
            {
                string[] ids = Request.Form["chkSelected_r"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    user.Roles.Add(new RoleInfo(Converter.IntParse(ids[i], 0)));
                }
            }

            try
            {
                RoleManager manager = new RoleManager();
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
                message.SetErrorMessage("用户更新失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlist.aspx");
        }
    }
}
