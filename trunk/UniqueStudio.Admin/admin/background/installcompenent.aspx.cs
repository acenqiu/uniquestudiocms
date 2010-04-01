using System;
using System.IO;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Compenent;
using UniqueStudio.Core.Site;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Admin.admin.background
{
    public partial class installcompenent : Controls.AdminBasePage
    {
        private const string COMPENENTS_FOLDER = "admin/compenents/";

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
                string[] directories = Directory.GetDirectories(Server.MapPath("~/" + COMPENENTS_FOLDER));
                foreach (string directory in directories)
                {
                    DirectoryInfo info = new DirectoryInfo(directory);
                    ddlFolders.Items.Add(new ListItem(info.Name, info.Name));
                }

                SiteManager manager = new SiteManager();
                ddlSites.DataSource = manager.GetAllSites();
                ddlSites.DataTextField = "SiteName";
                ddlSites.DataValueField = "SiteId";
                ddlSites.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("信息读取失败：" + ex.Message);
            }
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            string path = PathHelper.PathCombine(COMPENENTS_FOLDER, ddlFolders.SelectedItem.Value);

            try
            {
                CompenentInfo compenent = (new CompenentManager()).ReadInstallFile(CurrentUser, path);
                ltlCompenentName.Text = compenent.CompenentName;
                ltlDisplayName.Text = compenent.DisplayName;
                ltlCompenentAuthor.Text = compenent.CompenentAuthor;
                ltlDescription.Text = compenent.Description;
                ltlClassPath.Text = compenent.ClassPath;
                ltlAssembly.Text = compenent.Assembly;
                pnlInfo.Visible = true;
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("安装文件读取失败：" + ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            string path = PathHelper.PathCombine(COMPENENTS_FOLDER, ddlFolders.SelectedItem.Value);

            try
            {
                int siteId = Converter.IntParse(ddlSites.SelectedValue, 0);
                if ((new CompenentManager()).InstallCompenent(CurrentUser, path, siteId))
                {
                    message.SetSuccessMessage("组件安装成功!");
                }
                else
                {
                    message.SetErrorMessage("组件安装失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("组件安装失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlInfo.Visible = false;
        }
    }
}
