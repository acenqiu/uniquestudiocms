using System;
using System.Web;
using System.Web.UI;

using UniqueStudio.Core.Site;

namespace UniqueStudio.ComContent.PL
{
    public partial class Default : Controls.PlBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Header.Title = SiteManager.Config(SiteId).WebName;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("search.aspx?title=" + HttpUtility.UrlEncode(txtSearch.Text.Trim()));
        }
    }
}
