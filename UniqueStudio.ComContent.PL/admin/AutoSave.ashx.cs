using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;

namespace UniqueStudio.ComContent.PL.admin
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AutoSave : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Form["uri"] != null & context.Request.Form["content"] != null & context.Request.Form["userid"] != null)
            {
                AutoSaveManager am = new AutoSaveManager();
                PostInfo post = new PostInfo();
                Guid userid = new Guid(context.Request.Form["userid"].ToString());
                Int64 userUri = Convert.ToInt64(context.Request.Form["uri"]);
                post.Content = context.Request.Form["content"].ToString();
                post.Summary = post.SubTitle = post.Title = post.AddUserName = post.Author = string.Empty;
                if (context.Request.Form["summary"] != null)
                {
                    post.Summary = context.Request.Form["summary"].ToString();
                }
                if (context.Request.Form["title"] != null)
                {
                    post.Title = context.Request.Form["title"].ToString();
                }
                if (context.Request.Form["author"] != null)
                {
                    post.Author = post.AddUserName = context.Request.Form["author"].ToString();
                }
                if (context.Request.Form["subTitle"] != null)
                {
                    post.SubTitle = context.Request.Form["subTitle"].ToString();
                }
                try
                {
                    if (am.AutoSaveFile(userid, post, userUri))
                    {
                        context.Response.Write("自动保存成功于" + DateTime.Now.ToString("yyyy/mm/dd hh:mm"));
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
