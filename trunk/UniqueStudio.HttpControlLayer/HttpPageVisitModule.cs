//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：页面访问http Module。
// 完成日期：2010年03月20日
// 版本：v0.3
// 作者：邱江毅
//=================================================================
using System;
using System.Text.RegularExpressions;
using System.Web;

using UniqueStudio.Common.Model;
using UniqueStudio.Core.PageVisit;

namespace UniqueStudio.HttpControlLayer
{
    public class HttpPageVisitModule : IHttpModule
    {
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
                PageVisitManager.AddPageVisit(pv);
            }
        }
    }
}
