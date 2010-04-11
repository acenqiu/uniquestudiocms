//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：子网站路由页面。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
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
