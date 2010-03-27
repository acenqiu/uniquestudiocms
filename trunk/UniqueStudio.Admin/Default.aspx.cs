using System;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteCollection collection = (new SiteManager()).GetAllSites();

            if (collection.Count > 0)
            {
                int siteId = 0;
                if (Request.Cookies[GlobalConfig.COOKIE] != null)
                {
                    siteId = Converter.IntParse(Request.Cookies[GlobalConfig.COOKIE][GlobalConfig.COOKIE_SITEID], 0);
                }
                string relativePath = string.Empty;
                if (siteId != 0)
                {
                    foreach (SiteInfo site in collection)
                    {
                        if (site.SiteId == siteId)
                        {
                            relativePath = site.RelativePath;
                            break;
                        }
                    }
                }
                else
                {
                    relativePath = collection[0].RelativePath;
                }
                if (!string.IsNullOrEmpty(relativePath))
                {
                    Response.Redirect("~/" + relativePath);
                }
            }
        }
    }
}
