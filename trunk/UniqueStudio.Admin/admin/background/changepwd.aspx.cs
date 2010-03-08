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

using UniqueStudio.Core.User;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class changepwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            UserInfo user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
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
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
