//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：后台管理框架页面。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

using UniqueStudio.Common.Model;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin
{
    public partial class _default : System.Web.UI.Page
    {
        protected int SiteId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SiteCollection sites = (new SiteManager()).GetAllSites();
                if (sites != null && sites.Count > 0)
                {
                    SiteId = sites[0].SiteId;
                }
            }
        }
    }
}
