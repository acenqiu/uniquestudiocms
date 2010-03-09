using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 用户的集合
    /// </summary>
    public class UserCollection:List<UserInfo>
    {
        private int amount;
        private int pageIndex;
        private int pageSize;

        /// <summary>
        /// 初始化<see cref="UserCollection"/>类的实例
        /// </summary>
        public UserCollection()
            :base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="UserCollection"/>类的实例
        /// </summary>
        /// <param name="capacity"></param>
        public UserCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// 用户总数
        /// </summary>
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        /// <summary>
        /// 页索引
        /// </summary>
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
        public int PageCount
        {
            get { return (amount - 1) / pageSize + 1; }
        }
    }
}
