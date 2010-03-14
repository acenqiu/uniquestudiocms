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

using UniqueStudio.Core.Compenent;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class installcompenent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            CompenentManager manager = new CompenentManager();
            CompenentInfo compenent = manager.ReadInstallFile(new UserInfo(), txtPath.Text);
            ltlCompenentName.Text = compenent.CompenentName;
            ltlDisplayName.Text = compenent.DisplayName;
            ltlCompenentAuthor.Text = compenent.CompenentAuthor;
            ltlDescription.Text = compenent.Description;
            pnlInfo.Visible = true;

            ViewState["CompenentInfo"] = compenent;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];

            CompenentManager manager = new CompenentManager();
            CompenentInfo compenent = null;
            if (ViewState["CompenentInfo"] != null)
            {
                compenent = (CompenentInfo)ViewState["CompenentInfo"];
            }
            else
            {
                compenent = (CompenentInfo)manager.ReadInstallFile(currentUser, txtPath.Text);
            }
            compenent.InstallFilePath = txtPath.Text;
            if (manager.InstallCompenent(currentUser,compenent,txtPath.Text) != null)
            {
                message.SetSuccessMessage("组件安装成功!");
            }
            else
            {
                message.SetErrorMessage("组件安装失败！");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlInfo.Visible = false;
            txtPath.Text = "";
            if (ViewState["CompenentInfo"] != null)
            {
                ViewState["CompenentInfo"] = null;
            }
        }
    }
}
