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
