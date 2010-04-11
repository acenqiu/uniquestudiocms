//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：服务器配置页面。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
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
                    message.SetErrorMessage("配置信息读取失败：" + ex.Message);
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
                message.SetErrorMessage("配置信息保存失败：" + ex.Message);
            }
        }
    }
}
