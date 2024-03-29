﻿//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：登录、注册、找回密码页面。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Web;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.User;

namespace UniqueStudio.Admin.admin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] == null)
                {
                    if (Request.Cookies[GlobalConfig.COOKIE] != null)
                    {
                        if (Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_AUTOLOGIN] == "true")
                        {
                            UserLogin(Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL],
                                            Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_PASSWORD], true);
                        }
                        txtAccount.Text = Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL];
                    }

                    revEmail.Enabled = !SecurityConfig.EnableLoginByUserName;
                    btnRediretToRegister.Visible = SecurityConfig.EnableRegister;
                }
                else if (Request.QueryString["action"] == "register")
                {
                    if (!SecurityConfig.EnableRegister)
                    {
                        Response.Redirect("login.aspx?msgtype=e&msg=" + HttpUtility.UrlEncode("管理员禁用了注册功能！"));
                    }
                    else
                    {
                        divLogin.Visible = false;
                        divRegister.Visible = true;
                    }
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UserLogin(txtAccount.Text.Trim(), txtPassword.Text.Trim(), false);
        }

        private void UserLogin(string account, string password, bool isAutoLogin)
        {
            if (!isAutoLogin)
            {
                //验证码
                if (this.Session["CheckCode"] == null || ((string)this.Session["CheckCode"]).ToLower() != txtCheckCode.Text.ToLower())
                {
                    message.SetErrorMessage("验证码错误，请刷新后重试！");
                    txtCheckCode.Text = "";
                    return;
                }
            }

            UserManager manager = new UserManager();
            try
            {
                UserInfo user = manager.UserAuthorization(account, password);
                if (user != null)
                {
                    if (!isAutoLogin)
                    {
                        if (chbAutoLogin.Checked)
                        {
                            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL] = account;
                            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_PASSWORD] = password;
                            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_AUTOLOGIN] = "true";
                        }
                        else
                        {
                            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL] = account;
                        }
                        Response.Cookies[GlobalConfig.COOKIE].Expires = DateTime.Now.AddDays(100d);
                    }

                    UserInfo sessionUser = new UserInfo(user.UserId, user.UserName, this.Session.SessionID);
                    sessionUser.Permissions = user.Permissions;
                    this.Session[GlobalConfig.SESSION_USER] = sessionUser;
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

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (this.Session["CheckCode"] == null || ((string)this.Session["CheckCode"]).ToLower() != txtCheckCodeR.Text.ToLower())
            {
                messageR.SetErrorMessage("验证码错误，请刷新后重试！");
                txtCheckCode.Text = "";
                return;
            }

            UserManager manager = new UserManager();
            UserInfo user = new UserInfo();
            user.Email = txtEmail.Text.Trim();
            user.UserName = txtUserName.Text.Trim();
            user.Password = txtNewPassword.Text.Trim();

            try
            {
                user = manager.CreateUser(null, user);

                if (user != null)
                {
                    messageR.SetErrorMessage("恭喜您，注册成功！");
                }
                else
                {
                    messageR.SetErrorMessage("注册失败，请重试！");
                }
            }
            catch (Exception ex)
            {
                messageR.SetErrorMessage(ex.Message);
            }
        }

        protected void btnRediretToRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx?action=register");
        }

        protected void btnRedirectToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx");
        }
    }
}
