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

using UniqueStudio.ComCalendar.Model;
using UniqueStudio.ComCalendar.BLL;
using System.Text;
namespace UniqueStudio.CGE
{
    public partial class calendar : System.Web.UI.Page
    {
        private CalNoticeManager cnm = new CalNoticeManager();
        private string prototypy = "<tr><td class='calendarItem-event'>{0}</td><td class='calendarItem-time'>时间：{1}</td><td class='calendarItem-note'>备注：{2}</td></tr>";
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime date=DateTime.Today;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["date"]))
                {
                    date = Convert.ToDateTime(Request.QueryString["date"]);
                }
            }
            catch(Exception exception)
            {
                date = DateTime.Today;
            }

            calendarDate.Text = date.Year.ToString() + "年" + date.Month.ToString() + "月" + date.Day.ToString() + "日";
            CalendarNoticeCollection calNotice = cnm.GetNoticesByDate(date);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='calendarTable'>");
            foreach (CalendarNotice notice in calNotice)
            {
                sb.Append(String.Format(prototypy, new string[] { notice.Content, notice.Time, notice.Remarks }));
            }
            sb.Append("</table>");
            calendarNotice.Text = sb.ToString();
        }
    }
}
