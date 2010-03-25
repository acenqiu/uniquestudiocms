//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示缓存项的实体类。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示缓存项的实体类。
    /// </summary>
    public class CacheItemInfo
    {
        private object value;
        private DateTime createTime;
        private int hits = 0;
        
        /// <summary>
        /// 初始化<see cref="CacheItemInfo"/>类的实例。
        /// </summary>
        public CacheItemInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 值。
        /// </summary>
        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }
        /// <summary>
        /// 缓存项创建时间。
        /// </summary>
        public DateTime CreateTime
        {
            get { return createTime; } 
            set { createTime = value; }
        }
        /// <summary>
        /// 缓存项读取次数。
        /// </summary>
        public int Hits
        {
            get { return hits; }
            set { hits = value; }
        }
    }
}
