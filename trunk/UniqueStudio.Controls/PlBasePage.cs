using System;

using UniqueStudio.Common.Config;

namespace UniqueStudio.Controls
{
    public class PlBasePage : System.Web.UI.Page
    {
        protected int SiteId = 0;
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
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.Session[GlobalConfig.SESSION_SITEID] != null)
            {
                SiteId = (int)this.Session[GlobalConfig.SESSION_SITEID];
            }
            base.OnLoad(e);
        }
    }
}
