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

using UniqueStudio.Common.Config;

namespace UniqueStudio.Admin.admin.background
{
    public partial class websiteconfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                config.ConfigDocument = (new WebSiteConfig()).GetXmlConfig().OuterXml;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                (new WebSiteConfig()).SaveXmlConfig(config.GetConfigString());
                Response.Redirect("websiteconfig.aspx?msg="+HttpUtility.UrlEncode("网站配置保存成功！"));
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
