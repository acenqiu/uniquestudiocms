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

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Core.Site;

namespace UniqueStudio.ComContent.PL.controls
{
    public partial class PostStat : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PostManager manager = new PostManager();
                try
                {
                    PostStatCollection collection = manager.GetPostStat(SiteManager.Config(1).PostStatByYear);
                    if (SiteManager.Config(1).PostStatByYear)
                    {
                        rptStatByYear.DataSource = collection;
                        rptStatByYear.DataBind();
                    }
                    else
                    {
                        rptStatByMonth.DataSource = collection;
                        rptStatByMonth.DataBind();
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}