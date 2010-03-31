//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：控件配置页面。
// 完成日期：2010年03月30日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Module;
using System.Web;

namespace UniqueStudio.Admin.admin.background
{
    public partial class controlconfig : Controls.AdminBasePage
    {
        protected int ControlId;

        protected void Page_Load(object sender, EventArgs e)
        {
            ControlId = Converter.IntParse(Request.QueryString["controlId"], 0);
            if (ControlId == 0)
            {
                Response.Redirect("controllist.aspx?siteId=" + SiteId);
            }

            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            try
            {
                ModuleControlInfo control = (new ModuleControlManager()).GetModuleControl(CurrentUser, ControlId);
                if (control == null)
                {
                    message.SetErrorMessage("数据读取失败：指定控件不存在！");
                    return;
                }

                txtControlName.Text = control.ControlName;
                hfOldControlName.Value = control.ControlName;
                chkIsEnabled.Checked = control.IsEnabled;
                config.ConfigDocument = control.Config;
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ModuleControlInfo control = new ModuleControlInfo();
                control.ControlId = ControlId;
                control.SiteId = SiteId;
                control.ControlName = txtControlName.Text.Trim();
                control.IsEnabled = chkIsEnabled.Checked;
                control.Config = config.GetConfigString();

                if ((new ModuleControlManager()).UpdateModuleControl(CurrentUser, control, hfOldControlName.Value))
                {
                    Response.Redirect(string.Format("controlconfig.aspx?msg={0}&siteId={1}&controlId={2}",
                                                                            HttpUtility.UrlEncode("控件信息保存成功！"),
                                                                            SiteId, ControlId));
                }
                else
                {
                    message.SetErrorMessage("控件信息保存失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("控件信息保存失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("controllist.aspx?siteId=" + SiteId);
        }
    }
}
