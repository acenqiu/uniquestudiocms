using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using UniqueStudio.ComCalendar.BLL;

namespace UniqueStudio.ComCalendar.Admin
{
    public partial class CalendarEditor : System.Web.UI.UserControl
    {
        private CalNoticeManager cnm = new CalNoticeManager();
        private List<DateTime> dates = null;

        private int siteId;

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        public Calendar MyCalendar
        {
            get
            {
                return CalNotice;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dates = cnm.GetAllCalNoticeDate(siteId);
            }
        }

        protected void CalendarNotice_SelectionChanged(object sender, EventArgs e)
        {
            if (cnm.GetNoticesByDate(siteId, CalNotice.SelectedDate) != null)
            {
                Response.Redirect(string.Format("calendar.aspx?siteId={0}&date={1}"
                                                                                , siteId
                                                                                , CalNotice.SelectedDate.ToString("yyyy-MM-dd")));
            }
        }

        protected void CalNotice_DayRender(object sender, DayRenderEventArgs e)
        {
            if (dates != null && !e.Day.IsOtherMonth)
            {
                foreach (DateTime date in dates)
                {
                    if (e.Day.Date == date)
                    {
                        e.Cell.BackColor = System.Drawing.Color.DarkOrange;
                    }
                }
            }

        }
    }
}