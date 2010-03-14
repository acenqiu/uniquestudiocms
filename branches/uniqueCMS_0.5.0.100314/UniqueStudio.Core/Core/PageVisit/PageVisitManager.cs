using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Core.PageVisit
{
    /// <summary>
    /// 提供页面访问统计的方法
    /// </summary>
    public class PageVisitManager
    {
        private static IPageVisit provider = DALFactory.CreatePageVisit();

        /// <summary>
        /// 初始化<see cref="PageVisitManager"/>类的实例
        /// </summary>
        public PageVisitManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 增加页面访问信息
        /// </summary>
        /// <param name="pv">页面访问信息</param>
        /// <returns>是否增加成功</returns>
        public bool AddPageVisit(PageVisitInfo pv)
        {
            return provider.AddPageVisit(pv);
        }

        /// <summary>
        /// 返回页面访问总量
        /// </summary>
        /// <returns>页面访问总量</returns>
        public static int GetPageVisitCount()
        {
            try
            {
                return provider.GetPageVisitCount();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 返回页面访问列表
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页条目数</param>
        /// <returns>页面访问列表</returns>
        public PageVisitCollection GetPageVisitList(UserInfo currentUser, int pageIndex,int pageSize)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("页面及每个页面的条数不能小于1。");
            }

            if (!PermissionManager.HasPermission(currentUser, "ViewPageVisitInfo"))
            {
                throw new InvalidPermissionException("当前用户没有查看访问信息的权限，请与管理员联系！");
            }

            try
            {
                return provider.GetPageVisitList(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }
    }
}
