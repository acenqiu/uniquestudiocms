//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：注销登录状态页面。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

using UniqueStudio.Common.Config;

namespace UniqueStudio.Admin.admin
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //清除SESSION
            this.Session.Clear();
            this.Session.Abandon();

            //清除COOKIE
            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL] = Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_EMAIL];
            Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_SITEID] = Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_SITEID];
            Response.Cookies[GlobalConfig.COOKIE].Expires = DateTime.Now.AddDays(100d);

            Response.Redirect("login.aspx");
        }
    }
}
