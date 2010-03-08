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
    public class PageVisitManager
    {
        private static IPageVisit provider = DALFactory.CreatePageVisit();

        public PageVisitManager()
        {
            //默认构造函数
        }

        public bool AddPageVisit(PageVisitInfo pv)
        {
            return provider.AddPageVisit(pv);
        }

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
