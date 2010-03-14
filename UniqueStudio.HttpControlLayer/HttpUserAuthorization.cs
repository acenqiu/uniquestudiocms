using System;
using System.Text.RegularExpressions;
using System.Web;

using UniqueStudio.Common.Config;

namespace UniqueStudio.HttpControlLayer
{
    public class HttpUserAuthorization : IHttpModule
    {
        private static Regex r = new Regex("/admin/.*");

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            if (context.Request.Path.EndsWith(".aspx") && !context.Request.Path.EndsWith("login.aspx"))
            {
                if (r.IsMatch(context.Request.Path))
                {
                    if (context.Session[GlobalConfig.SESSION_USER] == null)
                    {
                        
                        context.Response.Redirect("~/admin/login.aspx?ret=" + HttpUtility.UrlEncode(context.Request.RawUrl));
                    }
                }
            }
        }
    }
}
