using System;
using System.Web;

namespace UniqueStudio.HttpControlLayer
{
    public class UrlRewriter : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.UrlRewriter_AuthorizeRequest);
        }

        private void UrlRewriter_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            if (app.Context.Request.FilePath.IndexOf(".aspx") > 0)
            {
                //RewriterUtils.RewriteUrl(app.Context, "~/compenents/com_content/"+app.Context.Request.FilePath.Substring(1));
            }
        }

        #endregion
    }
}
