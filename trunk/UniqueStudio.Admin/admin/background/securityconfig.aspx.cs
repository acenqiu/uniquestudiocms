//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：安全配置页面。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Web;

using UniqueStudio.Common.Config;
using UniqueStudio.Core.Permission;

namespace UniqueStudio.Admin.admin.background
{
    public partial class securityconfig : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    config.ConfigDocument = (new SecurityConfig()).GetXmlConfig().OuterXml;
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("配置信息读取失败：" + ex.Message);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckPermission();
                (new SecurityConfig()).SaveXmlConfig(config.GetConfigString());
                Response.Redirect("securityconfig.aspx?msg=" + HttpUtility.UrlEncode("安全配置保存成功！"));
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("配置信息保存失败：" + ex.Message);
            }
        }

        protected void CheckPermission()
        {
            PermissionManager.CheckPermission(CurrentUser, "EditSystemConfig", "配置系统设置");
        }
    }
}
