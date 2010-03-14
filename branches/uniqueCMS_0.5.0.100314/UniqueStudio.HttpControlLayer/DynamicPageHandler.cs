using System.Web;

using UniqueStudio.PL.Engine;

namespace UniqueStudio.HttpControlLayer
{
    /// <summary>
    /// 实现调用模板引擎动态解析页面的方法
    /// </summary>
    public class DynamicPageHandler : IHttpHandler
    {
        /// <summary>
        /// 初始化<see cref="DynamicPageHandler"/>类的实例
        /// </summary>
        public DynamicPageHandler()
        {
            //默认构造函数
        }

        /// <summary>
        /// 获取一个值，该值指示其他请求是否可以使用IHttpHandler 实例
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context">HttpContext 对象，
        /// 它提供对用于为 HTTP 请求提供服务的内部服务器对象
        /// （如 Request、Response、Session 和 Server）的引用</param>
        public void ProcessRequest(HttpContext context)
        {
            EngineControler engine = new EngineControler(context);
            engine.ProcessRequest();
        }
    }
}
