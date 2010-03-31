using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Reflection;
using System.Collections;

namespace UniqueStudio.ComCalendar.BLL
{
    public class DataControl<T>
    {
        PropertyInfo[] properties;
        public T Instance
        {
            get
            {
                return t;
            }
        }
        T t;
        Hashtable propertiesTable = new Hashtable();
        public string ID
        {
            get;
            set;
        }
        public string[] PropertyName
        {
            get
            {
                return propertyName;
            }
        }
        private string[] propertyName;
        public DataControl(T t, string[] properties)
        {
            this.t = t;
            Bind(properties);
            propertyName = properties;
            ID = GetValue("ID");
        }
        public PropertyInfo this[string property]
        {
            get
            {
                return propertiesTable[property] as PropertyInfo;
            }
            set
            {
                propertiesTable[property] = value;
            }
        }
        public int Count
        {
            get
            {
                return propertiesTable.Count;
            }
        }
        private void Bind(string[] propertyStrings)
        {
            PropertyInfo[] properties = ToPropertyInfoList(propertyStrings).ToArray();
            for (int i = 0; i < properties.Length; i++)
            {
                propertiesTable.Add(properties[i].Name, properties[i]);
            }
        }
        private void Bind()
        {
            PropertyInfo[] properties = t.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                propertiesTable.Add(properties[i].Name, properties[i]);
            }
        }
        private List<PropertyInfo> ToPropertyInfoList(string[] properties)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();
            for (int i = 0; i < properties.Length; i++)
            {
                list.Add(GetProperty(properties[i]));
            }
            return list;
        }
        public void SetValue(string property, string value)
        {
            GetProperty(property).SetValue(t, value, null);
        }
        public string GetValue(string property)
        {
            return Convert.ToString(GetProperty(property).GetValue(t, null));
        }
        public PropertyInfo GetProperty(string property)
        {
            return t.GetType().GetProperty(property);
        }
    }
}
