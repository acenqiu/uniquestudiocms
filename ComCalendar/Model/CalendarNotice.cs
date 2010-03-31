using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.ComCalendar.Model
{
    public class CalendarNotice
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CalendarNotice()
        {
        }
        private Guid id;
        private DateTime date;
        private string time;
        private string content;
        private string remarks;
        /// <summary>
        /// 通知ID
        /// </summary>
        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }
        
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        /// <summary>
        /// 通知内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}
