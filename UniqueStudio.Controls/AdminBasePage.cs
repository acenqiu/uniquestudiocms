using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Controls
{
    public class AdminBasePage : System.Web.UI.Page
    {
        protected UserInfo CurrentUser;
        protected int SiteId;

#if DEBUG
        protected DateTime dt;

        protected override void OnPreInit(EventArgs e)
        {
            dt = DateTime.Now;
            base.OnPreInit(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            TimeSpan ts = DateTime.Now - dt;
            //Response.Write("<!--query time:" + ts.TotalMilliseconds + "-->");
            //Common.ErrorLogging.ErrorLogger.LogError("QueryTime", Request.Url.PathAndQuery, ts.TotalMilliseconds.ToString());
        }
#endif

        protected override void OnLoad(EventArgs e)
        {
            CurrentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            SiteId = Converter.IntParse(Request.QueryString["siteId"], 0);
            base.OnLoad(e);
        }
    }
}
