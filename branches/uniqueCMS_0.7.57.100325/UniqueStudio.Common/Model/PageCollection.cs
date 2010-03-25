using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 页面的集合
    /// </summary>
    public class PageCollection:List<PageInfo>
    {
        /// <summary>
        /// 初始化<see cref="PageCollection"/>类实例
        /// </summary>
        public PageCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="PageCollection"/>类实例
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public PageCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
