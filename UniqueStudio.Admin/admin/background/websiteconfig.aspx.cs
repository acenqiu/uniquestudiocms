﻿//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：网站配置页面。
// 完成日期：2010年04月06日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
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
                try
                {
                    config.ConfigDocument = (new SiteManager()).LoadConfig(SiteId);
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
                string content = config.GetConfigString();
                if ((new SiteManager()).SaveConfig(CurrentUser, SiteId, content))
                {
                    Response.Redirect("websiteconfig.aspx?msg=" + HttpUtility.UrlEncode("网站配置保存成功！") + "&siteId=" + SiteId);
                }
                else
                {
                    message.SetErrorMessage("配置信息保存失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("配置信息保存失败：" + ex.Message);
            }
        }
    }
}
