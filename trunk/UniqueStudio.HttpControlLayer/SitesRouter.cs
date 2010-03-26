using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Site;

namespace UniqueStudio.HttpControlLayer
{
    public class SitesRouter : IHttpModule
    {

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            if (!context.Request.Path.EndsWith(".aspx") || context.Request.Path.StartsWith("/admin/"))
            {
                return;
            }

            SiteCollection sites = null;
            if (context.Cache[GlobalConfig.CACHE_SITES] != null)
            {
                sites = (SiteCollection)context.Cache[GlobalConfig.CACHE_SITES];
            }
            else
            {
                sites = (new SiteManager()).GetAllSites();
                context.Cache.Insert(GlobalConfig.CACHE_SITES, sites);
            }

            int siteId = 1;
            foreach (SiteInfo site in sites)
            {
                //如果该网站处于根目录，先记下它的ID
                if (string.IsNullOrEmpty(site.RelativePath))
                {
                    siteId = site.SiteId;
                }
                else
                {
                    if (context.Request.Path.StartsWith("/" + site.RelativePath + "/"))
                    {
                        context.Session[GlobalConfig.SESSION_SITEID] = site.SiteId;
                        return;
                    }
                }
            }
            //如果其他都不匹配，那么就是根目录
            context.Session[GlobalConfig.SESSION_SITEID] = siteId;
        }

        #endregion
    }
}
