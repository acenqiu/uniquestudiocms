//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供页面访问统计的方法。
// 完成日期：2010年03月20日
// 版本：v0.4
// 作者：邱江毅
//=================================================================
using System;
using System.Data.Common;

using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Core.PageVisit
{
    /// <summary>
    /// 提供页面访问统计的方法。
    /// </summary>
    public class PageVisitManager
    {
        private static IPageVisit provider = DALFactory.CreatePageVisit();

        /// <summary>
        /// 初始化<see cref="PageVisitManager"/>类的实例。
        /// </summary>
        public PageVisitManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 增加页面访问信息。
        /// </summary>
        /// <param name="pv">页面访问信息。</param>
        public static void AddPageVisit(PageVisitInfo pv)
        {
            if (pv == null)
            {
                return;
            }
            //暂不进行参数验证
            try
            {
                provider.AddPageVisit(pv);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                //为了不影响用户访问，不在抛出异常
            }
        }

        /// <summary>
        /// 返回页面访问总量。
        /// </summary>
        /// <returns>页面访问总量。</returns>
        public static int GetPageVisitCount()
        {
            try
            {
                return provider.GetPageVisitCount();
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回页面访问列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="pageIndex">页索引。</param>
        /// <param name="pageSize">每页条目数。</param>
        /// <returns>页面访问列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看访问信息的权限时抛出该异常。</exception>
        public PageVisitCollection GetPageVisitList(UserInfo currentUser, int pageIndex, int pageSize)
        {
            Validator.CheckNotPositive(pageSize, "pageSize");
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            PermissionManager.CheckPermission(currentUser, "ViewPageVisitInfo", "查看访问信息");

            try
            {
                return provider.GetPageVisitList(pageIndex, pageSize);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定月份每日的访问量信息。
        /// </summary>
        /// <param name="month">月份。</param>
        /// <returns>二维点的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看访问信息的权限时抛出该异常。</exception>
        public PointCollection<int, int> GetPvStatByMonth(UserInfo currentUser, DateTime month)
        {
            Validator.CheckNull(month, "month");
            if (month.Year < 2000)
            {
                throw new ArgumentException();
            }
            PermissionManager.CheckPermission(currentUser, "ViewPageVisitInfo", "查看访问信息");

            try
            {
                return provider.GetPvStatByMonth(month);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }
    }
}
