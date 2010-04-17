using System;
using System.Collections.Generic;

using UniqueStudio.ComCalendar.BLL;
using UniqueStudio.ComCalendar.Model;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.ComCalendar.Admin
{
    public partial class calendernotice : Controls.AdminBasePage
    {
        protected DateTime Date = DateTime.Today;

        protected void Page_Load(object sender, EventArgs e)
        {
            Date = Converter.DatetimeParse(Request.QueryString["date"], DateTime.Today);
            calendarDate.Text = Date.ToString("yyyy年MM月dd日");

            CalendarNoticeCollection list = (new CalNoticeManager()).GetNoticesByDate(SiteId, Date);
            if (list == null)
            {
                list = new CalendarNoticeCollection();
            }
            DataControlManager<CalendarNotice> manager = new DataControlManager<CalendarNotice>(list, new CalendarDataAccess());
            ControlToInputText textbox = new ControlToInputText();
            string s = manager.ConvertControlToHtml(new string[] { "Time", "Content", "Place","Remarks","Link"}, textbox);
            Literal1.Text = "<table >" + s + "</table>";
        }

        private class CalendarDataAccess : IDataAccess<CalendarNotice>
        {
            private CalNoticeManager cnm = new CalNoticeManager();
            public void Add(CalendarNotice notice)
            {
                cnm.AddCalNotice(null, notice);
            }
            public void Update(DataControl<CalendarNotice> control)
            {
                cnm.EditCalNotice(null, control.Instance);
            }
            public void Delete(DataControl<CalendarNotice> control)
            {
                cnm.DeleteCalendarNoticeByCatId(null, control.Instance.ID);
            }
        }
    }
}
