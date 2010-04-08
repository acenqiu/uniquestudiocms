using System;
using System.Web;

using UniqueStudio.Common.Config;

namespace UniqueStudio.Admin.admin.background
{
    public partial class serverconfig : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    config.ConfigDocument = (new ServerConfig()).GetXmlConfig().OuterXml;
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
                (new ServerConfig()).SaveXmlConfig(config.GetConfigString());
                Response.Redirect("serverconfig.aspx?msg=" + HttpUtility.UrlEncode("服务器配置保存成功！"));
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
