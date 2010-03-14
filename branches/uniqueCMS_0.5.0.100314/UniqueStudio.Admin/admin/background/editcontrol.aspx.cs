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
using System.Xml;

using UniqueStudio.Core.Module;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class editcontrol : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    ModuleControlManager manager = new ModuleControlManager();
                    ModuleControlInfo control = manager.GetModuleControl((UserInfo)this.Session[GlobalConfig.SESSION_USER],
                                                                                            Request.QueryString["id"]);
                    if (control != null)
                    {
                        ltlControlId.Text = control.ControlId;
                        ltlModuleName.Text = control.ModuleName;
                        btnEnable.Text = control.IsEnabled ? "禁用" : "启用";
                        config.ConfigDocument = control.Parameters;
                    }
                    else
                    {
                        message.SetErrorMessage("指定的控件不存在！");
                    }
                }
                else
                {
                    Response.Redirect("controllist.aspx");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string controlId = ltlControlId.Text;
            string configString = config.GetConfigString();
            ModuleControlManager manager = new ModuleControlManager();
            UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            try
            {
                if (manager.UpdateControlParameters(currentUser, controlId, configString))
                {
                    Response.Redirect("controllist.aspx?msg=" + HttpUtility.UrlEncode("控件修改成功！"));
                }
                else
                {
                    message.SetErrorMessage("控件修改失败，请重试！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
