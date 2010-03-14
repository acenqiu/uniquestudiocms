using System;

using UniqueStudio.Core.PlugIn;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class installplugin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            PlugInManager manager = new PlugInManager();
            PlugInInfo  plugin = manager.ReadInstallFile(new UserInfo(),txtPath.Text);
            ltlPlugInName.Text = plugin.PlugInName;
            ltlDisplayName.Text = plugin.DisplayName;
            ltlPlugInAuthor.Text = plugin.PlugInAuthor;
            ltlDescription.Text = plugin.Description;
            ltlAssembly.Text = plugin.Assembly;
            ltlClassPath.Text = plugin.ClassPath;
            pnlInfo.Visible = true;

            ViewState["PlugInInfo"] = plugin;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            PlugInManager manager = new PlugInManager();
            PlugInInfo plugin = null;
            if (ViewState["PlugInInfo"] != null)
            {
                plugin = (PlugInInfo)ViewState["PlugInInfo"];
            }
            else
            {
                plugin = (PlugInInfo)manager.ReadInstallFile(new UserInfo(), txtPath.Text);
            }
            plugin.InstallFilePath = txtPath.Text;
            if (manager.CreatePlugIn(new UserInfo(), plugin) != null)
            {
                message.SetSuccessMessage("插件安装成功!");
            }
            else
            {
                message.SetErrorMessage("插件安装失败！");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlInfo.Visible = false;
            txtPath.Text = "";
            if (ViewState["PlugInInfo"] != null)
            {
                ViewState["PlugInInfo"] = null;
            }
        }
    }
}
