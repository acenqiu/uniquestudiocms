using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Reflection;
using System.Text;

namespace UniqueStudio.ComCalendar.BLL
{
    public class DataControlManager<T>
    {
        private List<T> dataList = new List<T>();
        private List<DataControl<T>> controlList = new List<DataControl<T>>();
        public static List<DataControl<T>> ControlList = new List<DataControl<T>>();
        public static string[] Properties;
        private static IDataAccess<T> dataAccess;
        public DataControlManager(List<T> tList, IDataAccess<T> iAccess)
        {
            dataList = tList;
            dataAccess = iAccess;
        }
        private void convertDataToControl(string[] property)
        {

            foreach (T dataItem in dataList)
            {
                DataControl<T> controlItem = new DataControl<T>(dataItem, property);
                controlList.Add(controlItem);
            }
            ControlList = controlList;
        }
        public static void Add(T t)
        {
            dataAccess.Add(t);
            // ControlList.Add(dataAccess.Add(control));
        }
        public static void Delete(DataControl<T> control)
        {
            dataAccess.Delete(control);
            ControlList.Remove(control);
        }
        public static void Update(DataControl<T> control)
        {
            dataAccess.Update(control);
        }
        public static DataControl<T> GetControlByID(string id)
        {
            foreach (DataControl<T> control in ControlList)
            {
                if (control.ID.Equals(id))
                {
                    return control;
                }
            }
            return null;
        }
        private string convertContrlToHtml(IControlToHtml<T> controlToHtml)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataControl<T> control in controlList)
            {
                sb.Append(controlToHtml.ConvertToHtml(control));
            }
            return sb.ToString();
        }

        public string ConvertContrlToHtml(string[] properties, IControlToHtml<T> controlToHtml)
        {
            Properties = properties;
            convertDataToControl(properties);
            return convertContrlToHtml(controlToHtml);
        }
    }
}
