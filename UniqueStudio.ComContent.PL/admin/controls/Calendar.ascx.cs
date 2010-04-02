using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using UniqueStudio.ComCalendar.BLL;

namespace UniqueStudio.ComCalendar.Admin
{
    public partial class CalendarEditor : System.Web.UI.UserControl
    {
        private CalNoticeManager cnm = new CalNoticeManager();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CalendarNotice_SelectionChanged(object sender, EventArgs e)
        {
            if (cnm.GetNoticesByDate(CalNotice.SelectedDate) != null)
            {
                //Response.Redirect("");
                //TODO:定位到特定日期的notice页面
                Response.Redirect("calendar.aspx?caldate=" + CalNotice.SelectedDate.ToString());
            }
        }

        protected void CalNotice_DayRender(object sender, DayRenderEventArgs e)
        {
            List<DateTime> dates = cnm.GetAllCalNoticeDate();
            if (!e.Day.IsOtherMonth)
            {
                foreach (DateTime date in dates)
                {
                    if (e.Day.Date == date)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Blue;
                    }
                }
            }

        }
    }
}