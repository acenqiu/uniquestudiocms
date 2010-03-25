using System;
using System.Collections.Generic;
using System.Web;

namespace UniqueStudio.ComContent.PL
{
    /// <summary>
    /// Summary description for MetaWeblogAPI
    /// </summary>
    public class MetaWeblogAPI : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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