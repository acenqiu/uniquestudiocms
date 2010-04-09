using System;

using UniqueStudio.Common.Utilities;

namespace UniqueStudio.ComCalendar.Admin
{
    public partial class calendar : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            calendar1.SiteId = SiteId;
            
            if (!IsPostBack)
            {
                calendar1.MyCalendar.VisibleDate = Converter.DatetimeParse(Request.QueryString["date"], DateTime.Today);
            }
        }
    }
}
