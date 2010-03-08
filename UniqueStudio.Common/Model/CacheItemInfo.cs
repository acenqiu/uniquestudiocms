using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    public class CacheItemInfo
    {
        private object value;
        private DateTime createTime;
        private int hits = 0;

        public CacheItemInfo()
        {
        }

        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        public int Hits
        {
            get { return hits; }
            set { hits = value; }
        }
    }
}
