using System;

namespace UniqueStudio.ComContent.PL
{
    public partial class calendarframe : Controls.PlBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showcalendar.SiteId = SiteId;
        }
    }
}
