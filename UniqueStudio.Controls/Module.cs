using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        public int SiteId
        {
            get
            {
                if (ViewState["SiteId"] != null)
                {
                    return (int)ViewState["SiteId"];
                }
                else
                {
                    return 0;
                }
            }

            set
            {
                ViewState["SiteId"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
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
                    writer.Write(module.RenderContents(SiteId, ID));
                }
                else
                {
                    writer.Write(string.Empty);
                }
            }
        }
    }
}
