using System;
using System.Web;
using System.Web.UI;

using UniqueStudio.Core.Site;
using UniqueStudio.Common.Config;

namespace UniqueStudio.CGE
{
    public partial class Default : Controls.PlBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Header.Title = SiteManager.Config(SiteId).WebName;

                if (Request.Cookies[GlobalConfig.COOKIE] == null)
                {
                    Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_SITEID] = SiteId.ToString();
                }
                else
                {
                    foreach (string key in Request.Cookies[GlobalConfig.COOKIE].Values.AllKeys)
                    {
                        Response.Cookies[GlobalConfig.COOKIE][key] = Request.Cookies[GlobalConfig.COOKIE].Values[key];
                    }
                    Response.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_SITEID] = SiteId.ToString();
                }
            }
        }
    }
}
