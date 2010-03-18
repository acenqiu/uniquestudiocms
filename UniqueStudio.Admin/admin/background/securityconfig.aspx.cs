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
    public partial class securityconfig : System.Web.UI.Page
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
