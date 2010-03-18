using System;
using System.Web;

using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin.background
{
    public partial class websiteconfig : Controls.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int siteId = 1;
                config.ConfigDocument = (new SiteManager()).LoadConfig(siteId);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int siteId = 1;
                string content = config.GetConfigString();
                if ((new SiteManager()).SaveConfig(CurrentUser, siteId, content))
                {
                    Response.Redirect("websiteconfig.aspx?msg=" + HttpUtility.UrlEncode("网站配置保存成功！"));
                }
                else
                {
                    message.SetErrorMessage("配置信息保存失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
