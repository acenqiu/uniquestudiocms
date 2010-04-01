using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Module;

namespace UniqueStudio.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Module runat=server></{0}:Module>")]
    public class Module : WebControl
    {
        [Bindable(true)]
        [DefaultValue("")]
        [Localizable(true)]
        public string ModuleName
        {
            get
            {
                String s = (String)ViewState["ModuleName"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["ModuleName"] = value;
            }
        }

        [DefaultValue("<%= HttpUtility.UrlEncode(Request.Url.Query) %>")]
        public string QueryString
        {
            get
            {
                String s = (String)ViewState["QueryString"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["QueryString"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            int SiteId = 0;
            if (Context.Session[GlobalConfig.SESSION_SITEID] != null)
            {
                SiteId = (int)Context.Session[GlobalConfig.SESSION_SITEID];
            }

            try
            {
                IModule module = ModuleManager.GetInstance(ModuleName);
                if (module == null)
                {
                    writer.Write(string.Empty);
                }
                else
                {
                    if (ModuleControlManager.IsEnabled(SiteId, ID))
                    {
                        writer.Write(module.RenderContents(SiteId, ID, Context.Request.QueryString));
                    }
                    else
                    {
                        writer.Write(string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                writer.Write(string.Format("控件解析过程中出现了异常：{0}", ex.Message));
            }
        }
    }
}
