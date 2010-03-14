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
    public partial class controllist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
                ModuleManager manager = new ModuleManager();
                ddlModules.DataSource = manager.GetAllModules(currentUser);
                ddlModules.DataTextField = "ModuleName";
                ddlModules.DataValueField = "ModuleId";
                ddlModules.DataBind();

                GetData();
            }
        }

        private void GetData()
        {
            ModuleControlManager manager = new ModuleControlManager();
            rptList.DataSource = manager.GetAllModuleControls();
            rptList.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ModuleControlManager manager = new ModuleControlManager();
            ModuleControlInfo control = new ModuleControlInfo();
            control.ControlId = txtControlId.Text.Trim();
            control.ModuleId = Convert.ToInt32(ddlModules.SelectedValue);
            control.IsEnabled = chkIsEnabled.Checked;

            try
            {
                control = manager.CreateModuleControl((UserInfo)this.Session[GlobalConfig.SESSION_USER], control);
                if (control != null)
                {
                    message.SetSuccessMessage("控件创建成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("控件创建失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
