using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.Module;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class installModule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            ModuleManager manager = new ModuleManager();
            ModuleInfo module = manager.ReadInstallFile((UserInfo)this.Session[GlobalConfig.SESSION_USER], txtPath.Text);
            ltlModuleName.Text = module.ModuleName;
            ltlDisplayName.Text = module.DisplayName;
            ltlModuleAuthor.Text = module.ModuleAuthor;
            ltlDescription.Text = module.Description;
            pnlInfo.Visible = true;

            ViewState["ModuleInfo"] = module;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            ModuleManager manager = new ModuleManager();
            ModuleInfo module = null;
            if (ViewState["ModuleInfo"] != null)
            {
                module = (ModuleInfo)ViewState["ModuleInfo"];
            }
            else
            {
                module = (ModuleInfo)manager.ReadInstallFile((UserInfo)this.Session[GlobalConfig.SESSION_USER], txtPath.Text);
            }
            module.InstallFilePath = txtPath.Text;
            if (manager.InstallModule((UserInfo)this.Session[GlobalConfig.SESSION_USER],module,txtPath.Text) != null)
            {
                message.SetSuccessMessage("模块安装成功!");
            }
            else
            {
                message.SetErrorMessage("模块安装失败！");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlInfo.Visible = false;
            txtPath.Text = "";
            if (ViewState["ModuleInfo"] != null)
            {
                ViewState["ModuleInfo"] = null;
            }
        }
    }
}
