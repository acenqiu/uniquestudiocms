using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using UniqueStudio.Core.PlugIn;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //配置信息初始化 
            GlobalConfig.BasePhysicalPath = Server.MapPath("~/");

            (new ServerConfig()).LoadConfig();
            (new SecurityConfig()).LoadConfig();

            PlugInManager manager = new PlugInManager();
            ClassCollection collection = manager.GetAllPlugInsForInit();
            if (collection != null)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    try
                    {
                        IPlugIn plugIn = (IPlugIn)Assembly.Load(collection[i].Assembly).CreateInstance(collection[i].ClassPath);
                        if (plugIn != null)
                        {
                            plugIn.Register();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.LogError(ex);
                    }
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}