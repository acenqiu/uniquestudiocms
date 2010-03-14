using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 页面访问的集合
    /// </summary>
    public class PageVisitCollection : List<PageVisitInfo>
    {
        private int amount;
        private int pageIndex;
        private int pageSize;

        /// <summary>
        /// 初始化<see cref="PageVisitCollection"/>类的实例
        /// </summary>
        public PageVisitCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="PageVisitCollection"/>类的实例
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public PageVisitCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// 页面访问总量
        /// </summary>
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        /// <summary>
        /// 页索引
        /// </summary>
        /// <remarks>仅表示在查询时的页索引</remarks>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        /// <summary>
        /// 单页条目数
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        /// <summary>
        /// 页总数
        /// </summary>
        /// <remarks>仅表示在查询时的页总数</remarks>
        public int PageCount
        {
            get { return (amount - 1) / pageSize + 1; }
        }
    }
}
