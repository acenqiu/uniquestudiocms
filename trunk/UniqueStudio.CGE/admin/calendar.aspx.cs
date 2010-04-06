using System;

namespace UniqueStudio.ComCalendar.Admin
{
    public partial class calendar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["caldate"] != null)
            {
                calendar1.MyCalendar.VisibleDate = Convert.ToDateTime(Session["caldate"]);
            }
            else
            {
                Session["caldate"] = DateTime.Today.ToString();
            }
        }
    }
}
