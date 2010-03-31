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

namespace UniqueStudio.ComContent.PL.admin
{
    public partial class calendar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["caldate"] != null)
            {
                Session["caldate"] = Request.QueryString["caldate"].ToString();
            }
            else
            {
                Session["caldate"] = DateTime.Today.ToString();
            }
        }
    }
}
