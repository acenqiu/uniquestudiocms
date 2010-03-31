//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：安装模块页面。
// 完成日期：2010年03月28日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.IO;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Module;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin.background
{
    public partial class installModule : Controls.AdminBasePage
    {
        private const string MODULES_FOLDER = "admin/modules/";

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
                string[] directories = Directory.GetDirectories(Server.MapPath("~/" + MODULES_FOLDER));
                foreach (string directory in directories)
                {
                    DirectoryInfo info = new DirectoryInfo(directory);
                    ddlFolders.Items.Add(new ListItem(info.Name, info.Name));
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("目录信息读取失败：" + ex.Message);
            }
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            string path = PathHelper.PathCombine(MODULES_FOLDER, ddlFolders.SelectedItem.Value);

            try
            {
                ModuleInfo module = (new ModuleManager()).ReadInstallFile(CurrentUser, path);
                ltlModuleName.Text = module.ModuleName;
                ltlDisplayName.Text = module.DisplayName;
                ltlModuleAuthor.Text = module.ModuleAuthor;
                ltlDescription.Text = module.Description;
                pnlInfo.Visible = true;
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("安装文件读取失败：" + ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            string path = PathHelper.PathCombine(MODULES_FOLDER, ddlFolders.SelectedItem.Value);

            try
            {
                if ((new ModuleManager()).InstallModule(CurrentUser,path))
                {
                    message.SetSuccessMessage("模块安装成功！");
                }
                else
                {
                    message.SetErrorMessage("模块安装失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("模块安装失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlInfo.Visible = false;
        }
    }
}
