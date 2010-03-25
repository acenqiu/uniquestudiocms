//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：页面访问管理提供类需实现的方法。
// 完成日期：2010年03月20日
// 版本：v0.4
// 作者：邱江毅
//=================================================================
using System;
using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 页面访问管理提供类需实现的方法。
    /// </summary>
    internal interface IPageVisit
    {
        /// <summary>
        /// 增加页面访问信息。
        /// </summary>
        /// <param name="pv">页面访问信息。</param>
        void AddPageVisit(PageVisitInfo pv);

        /// <summary>
        /// 返回页面访问总量。
        /// </summary>
        /// <returns>页面访问总量。</returns>
        int GetPageVisitCount();

        /// <summary>
        /// 返回页面访问列表。
        /// </summary>
        /// <param name="pageIndex">页索引。</param>
        /// <param name="pageSize">每页条目数。</param>
        /// <returns>页面访问列表。</returns>
        PageVisitCollection GetPageVisitList(int pageIndex,int pageSize);

        /// <summary>
        /// 返回指定月份每日的访问量信息。
        /// </summary>
        /// <param name="month">月份。</param>
        /// <returns>二维点的集合。</returns>
        PointCollection<int, int> GetPvStatByMonth(DateTime month);
    }
}
