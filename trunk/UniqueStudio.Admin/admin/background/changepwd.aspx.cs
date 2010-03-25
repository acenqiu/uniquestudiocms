//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：修改用户密码页面。
// 完成日期：2010年03月19日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.User;

namespace UniqueStudio.Admin.admin.background
{
    public partial class changepwd : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            UserInfo user = new UserInfo();
            user.UserId = CurrentUser.UserId;
            user.Email = txtEmail.Text.Trim();
            user.Password = txtOldPassword.Text.Trim();

            try
            {
                if ((new UserManager()).ChangeUserPassword(user, txtNewPassword.Text.Trim()))
                {
                    message.SetSuccessMessage("密码修改成功！");
                    Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL] = Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL];
                    Response.Cookies[GlobalConfig.COOKIE].Expires = DateTime.Now.AddDays(100d);
                }
                else
                {
                    message.SetErrorMessage("密码修改失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("密码更新失败：" + ex.Message);
            }
        }
    }
}
