using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;

namespace UniqueStudio.CGE
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class autosavehandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Form["uri"] != null & context.Request.Form["content"] != null & context.Request.Form["userid"] != null)
            {
                AutoSaveManager am = new AutoSaveManager();
                Guid userid = new Guid(context.Request.Form["userid"]);
                PostInfo post = new PostInfo();
                post.Uri = Convert.ToInt64(context.Request.Form["uri"]);
                post.Summary = post.SubTitle = post.Title = post.AddUserName = post.Author = string.Empty;
                if (context.Request.Form["title"] != null)
                {
                    post.Title = context.Request.Form["title"];
                }
                if (context.Request.Form["subTitle"] != null)
                {
                    post.SubTitle = context.Request.Form["subTitle"];
                }
                if (context.Request.Form["author"] != null)
                {
                    post.Author = post.AddUserName = context.Request.Form["author"];
                }
                if (context.Request.Form["summary"] != null)
                {
                    post.Summary = HttpUtility.UrlDecode(context.Request.Form["summary"]);
                }
                post.Content = HttpUtility.UrlDecode(context.Request.Form["content"]);

                try
                {
                    if (am.AutoSavePost(userid, post))
                    {
                        context.Response.Write("自动保存成功于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    }
                    else
                    {
                        context.Response.Write("自动保存失败");
                    }
                }
                catch (Exception)
                {
                    context.Response.Write("自动保存失败");
                }
            }
            else
            {
                context.Response.Write("自动保存失败");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
