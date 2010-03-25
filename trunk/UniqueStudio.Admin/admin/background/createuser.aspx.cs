//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：创建用户页面。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

using UniqueStudio.Common.Model;
using UniqueStudio.Core.Permission;
using UniqueStudio.Core.User;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Admin.admin.background
{
    public partial class createuser : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    rptRoles.DataSource = (new RoleManager()).GetAllRoles(CurrentUser);
                    rptRoles.DataBind();
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("数据读取失败：" + ex.Message);
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            UserInfo user = new UserInfo();
            user.Email = txtEmail.Text.Trim();
            user.UserName = txtUserName.Text.Trim();
            user.Password = txtPassword.Text.Trim();
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
                UserManager manager = new UserManager();
                user = manager.CreateUser(CurrentUser, user);
                if (user != null)
                {
                    message.SetSuccessMessage("用户创建成功！");
                    txtEmail.Text = "";
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                }
                else
                {
                    message.SetErrorMessage("用户创建失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("用户创建失败：" + ex.Message);
            }
        }
    }
}
