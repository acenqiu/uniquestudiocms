using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using UniqueStudio.PL.Engine;

namespace UniqueStudio.HttpControlLayer
{
    /// <summary>
    /// 实现调用模板引擎动态解析页面的方法
    /// </summary>
    public class DynamicPageHandler : IHttpHandler
    {
        public DynamicPageHandler()
        {
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            EngineControler engine = new EngineControler(context);
            engine.ProcessRequest();
        }
    }
}
