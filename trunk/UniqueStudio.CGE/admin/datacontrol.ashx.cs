using System;
using System.Collections.Generic;
using System.Web;

using UniqueStudio.ComCalendar.BLL;
using UniqueStudio.ComCalendar.Model;
using UniqueStudio.Common.Utilities;

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
                    DateTime date = Converter.DatetimeParse(context.Request["date"], DateTime.Today);
                    int siteId = Converter.IntParse(context.Request["siteId"], 0);
                    DataControlManager<CalendarNotice>.Add(new CalendarNotice() { SiteId = siteId, Content = "事件", Remarks = "备注", Time = "时间", Date = date,Link="链接",Place="地点" });
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
