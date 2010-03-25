using System;
using System.Web;

using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin.background
{
    public partial class websiteconfig : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                config.ConfigDocument = (new SiteManager()).LoadConfig(SiteId);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string content = config.GetConfigString();
                if ((new SiteManager()).SaveConfig(CurrentUser, SiteId, content))
                {
                    Response.Redirect("websiteconfig.aspx?msg=" + HttpUtility.UrlEncode("网站配置保存成功！")+"&siteId="+SiteId);
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
