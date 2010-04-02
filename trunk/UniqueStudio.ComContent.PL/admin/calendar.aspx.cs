using System;

namespace UniqueStudio.ComCalendar.Admin
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
