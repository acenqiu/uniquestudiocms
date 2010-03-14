using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 页面访问管理提供类需实现的方法
    /// </summary>
    internal interface IPageVisit
    {
        /// <summary>
        /// 增加页面访问信息
        /// </summary>
        /// <param name="pv">页面访问信息</param>
        /// <returns>是否增加成功</returns>
        bool AddPageVisit(PageVisitInfo pv);

        /// <summary>
        /// 返回页面访问总量
        /// </summary>
        /// <returns>页面访问总量</returns>
        int GetPageVisitCount();

        /// <summary>
        /// 返回页面访问列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页条目数</param>
        /// <returns>页面访问列表</returns>
        PageVisitCollection GetPageVisitList(int pageIndex,int pageSize);
    }
}
