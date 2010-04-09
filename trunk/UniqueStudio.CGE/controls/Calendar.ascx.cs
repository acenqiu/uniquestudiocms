using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using UniqueStudio.ComCalendar.BLL;
namespace UniqueStudio.CGE.controls
{
    public partial class CalendarEditor : System.Web.UI.UserControl
    {
        private CalNoticeManager cnm = new CalNoticeManager();
        private List<DateTime> dates = null;
        private const string linkPrototype = "<a href='calendar.aspx?date={0}' class='{2}' target='{3}'>{1}</a>";

        private int siteId;
        public int SiteId
        {
            set { siteId = value; }
        }

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


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    dates = cnm.GetAllCalNoticeDate(siteId);
                }
                catch
                {
                }
            }
        }

        protected void CalendarNotice_SelectionChanged(object sender, EventArgs e)
        {
            if (cnm.GetNoticesByDate(siteId, CalNotice.SelectedDate) != null)
            {
                Response.Redirect("calendar.aspx?date=" + CalNotice.SelectedDate.ToString("yyyy-MM-dd"));
            }
        }

        protected void CalNotice_DayRender(object sender, DayRenderEventArgs e)
        {
            if (dates != null)
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
}