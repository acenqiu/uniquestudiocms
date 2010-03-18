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

using UniqueStudio.Core.Site;

namespace UniqueStudio.ComContent.PL
{
    public partial class Default : System.Web.UI.Page
    {
        protected DateTime dt;

        protected override void OnPreInit(EventArgs e)
        {
            dt = DateTime.Now;
            base.OnPreInit(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            TimeSpan ts = DateTime.Now - dt;
            Response.Write("<!--query time:" + ts.TotalMilliseconds+"-->");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Header.Title = SiteManager.Config(1).WebName;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("search.aspx?title=" + HttpUtility.UrlEncode(txtSearch.Text.Trim()));
        }
    }
}
