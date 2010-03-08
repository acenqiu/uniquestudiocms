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
            Response.Cookies[GlobalConfig.COOKIE].Expires = DateTime.Now.AddDays(100d);

            Response.Redirect("login.aspx");
        }
    }
}
