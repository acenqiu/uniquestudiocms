using System;
using System.Web;

using UniqueStudio.Common.Config;

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
                    message.SetErrorMessage(ex.Message);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                (new SecurityConfig()).SaveXmlConfig(config.GetConfigString());
                Response.Redirect("securityconfig.aspx?msg=" + HttpUtility.UrlEncode("安全配置保存成功！"));
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
