using System;
using System.Web;
using System.Web.UI;

using UniqueStudio.Core.Site;
using UniqueStudio.Common.Config;

namespace UniqueStudio.ComContent.PL
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

                plIndex1.SiteId = SiteId;
                plIndex2.SiteId = SiteId;
                plIndex3.SiteId = SiteId;
                plIndex4.SiteId = SiteId;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("search.aspx?title=" + HttpUtility.UrlEncode(txtSearch.Text.Trim()));
        }
    }
}
