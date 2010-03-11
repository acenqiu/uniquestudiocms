using System;
using System.Text.RegularExpressions;
using System.Web;

using UniqueStudio.Common.Model;
using UniqueStudio.Core.PageVisit;

namespace UniqueStudio.HttpControlLayer
{
    public class HttpPageVisitModule : IHttpModule
    {
        private static PageVisitManager manager = new PageVisitManager();
        private static Regex r = new Regex("/admin/.*");

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        private void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpRequest request = app.Context.Request;

            if (request.Path.EndsWith(".aspx") && !r.IsMatch(request.Path))
            {
                string urlReferrer = string.Empty;
                if (request.UrlReferrer != null)
                {
                    urlReferrer = request.UrlReferrer.OriginalString;
                }
                PageVisitInfo pv = new PageVisitInfo(request.RawUrl,
                                                                          request.UserHostAddress,
                                                                          request.UserHostName,
                                                                          request.UserAgent,
                                                                          urlReferrer);
                manager.AddPageVisit(pv);
            }
        }
    }
}
