using System;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示缓存项的实体类
    /// </summary>
    public class CacheItemInfo
    {
        private object value;
        private DateTime createTime;
        private int hits = 0;
        
        /// <summary>
        /// 初始化<see cref="CacheItemInfo"/>类的实例
        /// </summary>
        public CacheItemInfo()
        {
        }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }
        /// <summary>
        /// 缓存项创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return createTime; } 
            set { createTime = value; }
        }
        /// <summary>
        /// 缓存项读取次数
        /// </summary>
        public int Hits
        {
            get { return hits; }
            set { hits = value; }
        }
    }
}
