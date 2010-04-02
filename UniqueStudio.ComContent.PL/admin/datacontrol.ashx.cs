using System;
using System.Collections.Generic;
using System.Web;

using UniqueStudio.ComCalendar.BLL;
using UniqueStudio.ComCalendar.Model;

namespace UniqueStudio.Admin
{
    public class datacontrol : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string[] properties = DataControlManager<CalendarNotice>.Properties;
            List<DataControl<CalendarNotice>> controlList = DataControlManager<CalendarNotice>.ControlList;
            DataControl<CalendarNotice> control;
            string test;
            if (context.Request["action"].Equals("update"))
            {
                control = DataControlManager<CalendarNotice>.GetControlByID(context.Request["ID"]);
                for (int i = 0; i < properties.Length; i++)
                {
                    control.SetValue(properties[i], context.Request[properties[i]]);
                }
                test = "update";
                DataControlManager<CalendarNotice>.Update(control);
            }
            else
            {
                if (context.Request["action"].Equals("add"))
                {
                    DateTime date;
                    if (String.IsNullOrEmpty(context.Request["caldate"]))
                    {
                        date = DateTime.Today;
                    }
                    else
                    {
                        date = Convert.ToDateTime(context.Request["caldate"]);
                    }
                    // control = new DataControl<CalendarNotice>(new CalendarNotice(), DataControlManager<CalendarNotice>.Properties);
                    DataControlManager<CalendarNotice>.Add(new CalendarNotice() { Content = "事件", Remarks = "备注", Time = "时间", Date = date });
                    test = DataControlManager<CalendarNotice>.ControlList.Count.ToString();
                }
                else
                {
                    if (context.Request["action"].Equals("delete"))
                    {
                        control = DataControlManager<CalendarNotice>.GetControlByID(context.Request["ID"]);
                        DataControlManager<CalendarNotice>.Delete(control);
                    }
                    test = "delete";
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(test);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
