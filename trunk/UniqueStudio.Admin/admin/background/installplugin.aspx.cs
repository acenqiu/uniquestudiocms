//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：安装插件页面。
// 完成日期：2010年03月24日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.IO;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.PlugIn;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin.background
{
    public partial class installplugin : Controls.AdminBasePage
    {
        private const string PLUGINS_FOLDER = "admin/plugins/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            try
            {
                ddlFolders.Items.Clear();
                string[] directories = Directory.GetDirectories(Server.MapPath("~/" + PLUGINS_FOLDER));
                foreach (string directory in directories)
                {
                    DirectoryInfo info = new DirectoryInfo(directory);
                    ddlFolders.Items.Add(new ListItem(info.Name, info.Name));
                }

                SiteManager manager = new SiteManager();
                ddlSites.Items.Clear();
                ddlSites.Items.Add(new ListItem("所有网站", "0"));
                SiteCollection sites = manager.GetAllSites();
                foreach (SiteInfo site in sites)
                {
                    ddlSites.Items.Add(new ListItem(site.SiteName, site.SiteId.ToString()));
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("目录信息读取失败：" + ex.Message);
            }
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            string path = PathHelper.PathCombine(PLUGINS_FOLDER, ddlFolders.SelectedItem.Value);

            try
            {
                PlugInInfo plugin = (new PlugInManager()).ReadInstallFile(CurrentUser, path);
                ltlPlugInName.Text = plugin.PlugInName;
                ltlDisplayName.Text = plugin.DisplayName;
                ltlPlugInAuthor.Text = plugin.PlugInAuthor;
                ltlDescription.Text = plugin.Description;
                ltlAssembly.Text = plugin.Assembly;
                ltlClassPath.Text = plugin.ClassPath;
                pnlInfo.Visible = true;
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("安装文件读取失败：" + ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            string path = PathHelper.PathCombine(PLUGINS_FOLDER, ddlFolders.SelectedItem.Value);
            int siteId = Converter.IntParse(ddlSites.SelectedValue, 0);

            try
            {
                if ((new PlugInManager()).InstallPlugIn(CurrentUser, path, siteId))
                {
                    message.SetSuccessMessage("插件安装成功！");
                }
                else
                {
                    message.SetErrorMessage("插件安装失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("插件安装失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlInfo.Visible = false;
        }
    }
}
