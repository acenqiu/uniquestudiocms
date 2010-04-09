using System;
using System.Text;

using UniqueStudio.ComCalendar.BLL;
using UniqueStudio.ComCalendar.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.CGE
{
    public partial class calendar : Controls.PlBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showcalendar2.SiteId = SiteId;
            if (!IsPostBack)
            {
                try
                {
                    DateTime date = Converter.DatetimeParse(Request.QueryString["date"], DateTime.Today);
                    showcalendar2.MyCalendar.VisibleDate = date;
                    calendarDate.Text = date.ToString("yyyy年MM月dd日");

                    CalendarNoticeCollection calNotice = (new CalNoticeManager()).GetNoticesByDate(SiteId, date);
                    rptNotices.DataSource = calNotice;
                    rptNotices.DataBind();
                }
                catch
                {
                    //
                    Response.Redirect("500.aspx");
                }
            }
        }
    }
}
