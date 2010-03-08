using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.ComContent.Model
{
    public class PostStatInfo
    {
        private int year;
        private int month;
        private int count;

        public PostStatInfo()
        {
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        public int Month
        {
            get { return month; }
            set { month = value; }
        }
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }
}
