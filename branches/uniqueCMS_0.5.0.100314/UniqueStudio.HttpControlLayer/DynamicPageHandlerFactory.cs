using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.SiteMap;

namespace UniqueStudio.HttpControlLayer
{
    /// <summary>
    /// 动态页面解析工厂类
    /// </summary>
    public class DynamicPageHandlerFactory : IHttpHandlerFactory
    {
        /// <summary>
        /// 返回一个单独的方法，可调用该方法来顺序调用指定事件的所有处理程序
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestType"></param>
        /// <param name="url"></param>
        /// <param name="pathTranslated"></param>
        /// <returns></returns>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (!GlobalConfig.EnableUrlRewrite)
            {
                return PageParser.GetCompiledPageInstance(url, pathTranslated, context);
            }

            string sendToUrl = url;
            string filePath = pathTranslated;

            SiteMapManager manager = new SiteMapManager();
            PageCollection pages = manager.GetAllPages();

            for (int i = 0; i < pages.Count; i++)
            {
                string lookFor = "^" + RewriterUtils.ResolveUrl(context.Request.ApplicationPath, pages[i].UrlRegex) + "$";
                Regex r = new Regex(lookFor, RegexOptions.IgnoreCase);
                if (r.IsMatch(url))
                {
                    
                    sendToUrl = RewriterUtils.ResolveUrl(context.Request.ApplicationPath, r.Replace(url, pages[i].PagePath+"?"+pages[i].Parameters));

                    string sendToUrlLessQString;
                    RewriterUtils.RewriteUrl(context, sendToUrl, out sendToUrlLessQString, out filePath);

                    if (pages[i].ProcessType == ProcessType.ByAspnet)
                    {
                        return PageParser.GetCompiledPageInstance(sendToUrlLessQString, filePath, context);
                    }
                    else if (pages[i].ProcessType == ProcessType.ByEngine)
                    {
                        DynamicPageHandler dynamicPageHandler = new DynamicPageHandler();
                        return (IHttpHandler)dynamicPageHandler;
                    }
                    else
                    {
                        //未知处理方式
                        return PageParser.GetCompiledPageInstance(sendToUrlLessQString, filePath, context);
                    }
                }
            }
            return PageParser.GetCompiledPageInstance(url, filePath, context);
        }

        /// <summary>
        /// 使工厂可以重用现有的处理程序实例
        /// </summary>
        /// <param name="handler">要重用的 IHttpHandler 对象</param>
        public virtual void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
