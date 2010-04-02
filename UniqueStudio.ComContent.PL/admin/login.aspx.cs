using System;
using System.Web;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.User;

namespace UniqueStudio.ComContent.Admin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies[GlobalConfig.COOKIE] != null)
                {
                    if (Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_AUTOLOGIN] == "true")
                    {
                        UserLogin(Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL],
                                        Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_PASSWORD], false);
                    }
                    txtEmail.Text = Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL];
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UserLogin(txtEmail.Text.Trim(), txtPassword.Text.Trim(), true);
        }

        private void UserLogin(string email, string password, bool isSetCookies)
        {
            UserManager manager = new UserManager();
            try
            {
                UserInfo user = manager.UserAuthorization(email, password);

                if (user != null)
                {
                    if (isSetCookies)
                    {
                        if (chbAutoLogin.Checked)
                        {
                            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL] = email;
                            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_PASSWORD] = password;
                            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_AUTOLOGIN] = "true";
                        }
                        else
                        {
                            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL] = email;
                        }
                        Response.Cookies[GlobalConfig.COOKIE].Expires = DateTime.Now.AddDays(100d);
                    }

                    user = new UserInfo(user.UserId, user.UserName, this.Session.SessionID);
                    this.Session[GlobalConfig.SESSION_USER] = user;
                    this.Session.Timeout = 60;

                    if (Request.QueryString["ret"] != null)
                    {
                        Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["ret"]));
                    }
                    else
                    {
                        Response.Redirect("default.aspx");
                    }
                }
                else
                {
                    message.SetErrorMessage("邮箱或密码错误");
                    txtPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
