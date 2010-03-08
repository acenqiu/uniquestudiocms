using System;
using System.Collections.Generic;

using UniqueStudio.Core.User;
using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class userlist : System.Web.UI.Page
    {
        private UserInfo currentUser;
        private int pageIndex;
        private int pageSize;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            if (!IsPostBack)
            {
                try
                {
                    Core.Permission.RoleManager manager = new UniqueStudio.Core.Permission.RoleManager();
                    cblRoles.DataSource = manager.GetAllRoles();
                    cblRoles.DataTextField = "RoleName";
                    cblRoles.DataValueField = "RoleName";
                    cblRoles.DataBind();
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage(ex.Message);
                }

                GetData();
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            UserManager manager = new UserManager();
            UserInfo user = new UserInfo();
            user.Email = txtEmail.Text.Trim();
            user.UserName = txtUserName.Text.Trim();
            user.Password = txtPassword.Text.Trim();
            user.Roles = new RoleCollection();

            for (int i = 0; i < cblRoles.Items.Count; i++)
            {
                if (cblRoles.Items[i].Selected)
                {
                    user.Roles.Add(new RoleInfo(cblRoles.Items[i].Value));
                }
            }

            try
            {
                user = manager.CreateUser(currentUser, user);
                if (user != null)
                {
                    message.SetSuccessMessage("用户创建成功！");
                    txtEmail.Text = "";
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("用户创建失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            UserManager manager = new UserManager(currentUser);
            List<Guid> list = new List<Guid>();
            if (Request.Form["chkSelected"] != null)
            {
                string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        list.Add(new Guid(ids[i]));
                    }
                }
                catch (FormatException ex)
                {
                    message.SetErrorMessage("用户ID格式错误");
                    return;
                }
            }
            else
            {
                return;
            }

            try
            {
                switch (ddlOperation.SelectedValue)
                {
                    case "delete":
                        if (manager.DeleteUsers(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选用户已删除！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选用户删除失败，请重试！");
                        }
                        break;
                    case "approve":
                        if (manager.ApproveUsers(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选用户已激活！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选用户激活失败，请重试！");
                        }
                        break;
                    case "lock":
                        if (manager.LockUsers(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选用户已锁定！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选用户锁定失败，请重试！");
                        }
                        break;
                    case "unlock":
                        if (manager.UnLockUsers(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选用户已解锁！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选用户解锁失败，请重试！");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }

        private void GetData()
        {
            UserManager manager = new UserManager(currentUser);

            pageIndex = Utility.IntParse(Request.QueryString["page"], 1);
            pageSize = Utility.IntParse(Request.QueryString["number"], 10);

            try
            {
                UserCollection users = manager.GetUserList(pageIndex, pageSize);
                rptList.DataSource = users;
                rptList.DataBind();

                pagination.Count = users.PageCount;
                pagination.CurrentPage = users.PageIndex;
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
