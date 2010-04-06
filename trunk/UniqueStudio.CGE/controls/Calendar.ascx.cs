using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.ComCalendar.BLL;
namespace UniqueStudio.CGE.controls
{
    public partial class CalendarEditor : System.Web.UI.UserControl
    {
        private CalNoticeManager cnm1 = new CalNoticeManager();
        private static List<DateTime> dates;
        private string target;
        public string Target
        {
            get
            {
                if (String.IsNullOrEmpty(target))
                {
                    target = "_self";
                }
                return target;
            }
            set
            {
                target = value;
            }
        }
        public Calendar MyCalendar
        {
            get
            {
                return CalNotice;
            }
        }
        private string linkPrototype = "<a href='calendar.aspx?date={0}' class='{2}' target='{3}'>{1}</a>";
        protected void Page_Load(object sender, EventArgs e)
        {
            dates = cnm1.GetAllCalNoticeDate();

        }

        protected void CalendarNotice_SelectionChanged(object sender, EventArgs e)
        {
            if (cnm1.GetNoticesByDate(CalNotice.SelectedDate) != null)
            {
                //Response.Redirect("");
                //TODO:定位到特定日期的notice页面
                Session["caldate"] = CalNotice.SelectedDate.ToString();
                Response.Redirect("calendar.aspx");
            }
        }
        protected void CalNotice_DayRender(object sender, DayRenderEventArgs e)
        {

            e.Cell.Controls.Clear();
            Literal link = new Literal();
            string linkClass;

            if (!e.Day.IsOtherMonth)
            {
                foreach (DateTime date in dates)
                {
                    if (e.Day.Date == date)
                    {
                        e.Cell.BackColor = System.Drawing.Color.DarkOrange;
                    }
                }
                linkClass = "currentMonth";
            }
            else
            {
                linkClass = "otherMonth";

            }
            link.Text = String.Format(linkPrototype, new string[] { e.Day.Date.ToShortDateString(), Convert.ToString(e.Day.Date.Day), linkClass, Target });

            e.Cell.Controls.Add(link);
        }


    }
}