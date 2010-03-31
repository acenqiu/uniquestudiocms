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
using System.Reflection;
using System.Text;
using System.Collections.Generic;

using UniqueStudio.ComCalendar.BLL;
using UniqueStudio.ComCalendar.Model;
namespace UniqueStudio.Admin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private List<CalendarNotice> testList = new List<CalendarNotice>();
        private CalNoticeManager cnm = new CalNoticeManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime date;
            if (Session["caldate"] != null)
            {
                date = Convert.ToDateTime(Session["caldate"]);
            }
            else
            {
                date = DateTime.Today;
            }
            testList = cnm.GetNoticesByDate(date);
            if (testList == null)
            {
                testList = new List<CalendarNotice>();
            }
            DataControlManager<CalendarNotice> manager = new DataControlManager<CalendarNotice>(testList, new CalendarDataAccess());
            ControlToInputText textbox = new ControlToInputText();
            string s = manager.ConvertContrlToHtml(new string[] { "Time", "Content", "Remarks" }, textbox);
            Literal1.Text = "<table >" + s + "</table>";
        }

        private class CalendarDataAccess : IDataAccess<CalendarNotice>
        {
            private CalNoticeManager cnm = new CalNoticeManager();
            public void Add(CalendarNotice notice)
            {
                cnm.AddCalNotice(notice);
            }
            public void Update(DataControl<CalendarNotice> control)
            {
                cnm.EditCalNotice(control.Instance);
            }
            public void Delete(DataControl<CalendarNotice> control)
            {
                cnm.DeleteCalendarNoticeByCatId(control.Instance.ID);
            }
        }
    }
}
