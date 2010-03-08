using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.FileAccessHelper;

namespace UniqueStudio.PL.Engine
{
    public class EngineControler
    {
        private string pagePath;
        private string parameters;
        private string staticPagePath;
        private HttpContext context;
        private string basePath = @"E:\Projects\CMS\CMS\UniqueStudio.Admin\ch\theme\";

        private EngineControler()
        {
        }

        public EngineControler(HttpContext context)
        {
            this.context = context;
        }

        //public EngineControler(string pagePath, string parameters)
        //{
        //    this.pagePath = pagePath;
        //    this.parameters = parameters;
        //}

        public void ProcessRequest()
        {
            string responseText = Translate(context.Request.PhysicalPath, context.Request.QueryString);
            context.Response.Write(responseText);
            context.Response.End();
        }

        public string Translate(string pagePath, NameValueCollection parms)
        {
            StringBuilder html = new StringBuilder(FileAccess.ReadFile(pagePath));
            StringBuilder test = new StringBuilder(DateTime.Now.ToString("HH:mm:ss"));

            //{us:include file="xxx.html"}
            Regex rInclude = new Regex("\\{us:include *file=['\"][^'\"\\}]*['\"']\\}");
            Regex rIncludeFile = new Regex("(?<=\\{us:include *file=['\"])[^'\"\\}]*(?=['\"']\\})");

            MatchCollection includes = rInclude.Matches(html.ToString());
            foreach (Match match in includes)
            {
                string filename =basePath + rIncludeFile.Match(match.Value).Value;
                string includeFileContent = FileAccess.ReadFile(filename);
                html.Replace(match.Value, includeFileContent);
            }

            //Regex rDy = new Regex(@"<us:(\w*)[^/>]*>([^<]*(?'Open'<us:\1[^/>]*>)?(?'-Open'</us:\1[^/>]*>)?(<[^<]*)*)*(?(Open)(?!))</us:\1[^/>]*>");
            Regex rDy = new Regex(@"<us:foreach[^/>]*>[\s\S]*?</us:foreach>");
            //test.Append(rDy.Match(html.ToString()).Value);
            MatchCollection dy = rDy.Matches(html.ToString());
            foreach (Match match in dy)
            {
                test.Append(match.Value).Append("==>\n");
            }
            return test.ToString()+DateTime.Now.ToString();
        }
    }
}
