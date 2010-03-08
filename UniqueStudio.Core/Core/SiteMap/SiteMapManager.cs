using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.DAL.SiteMap;

namespace UniqueStudio.Core.SiteMap
{
    public class SiteMapManager
    {
        private static readonly ISiteMap provider = DALFactory.CreateSiteMap();

        public SiteMapManager()
        {
        }

        public PageInfo CreatePage(UserInfo currentUser, PageInfo page)
        {
            if (!PermissionManager.HasPermission(currentUser, "CreatePage"))
            {
                throw new InvalidPermissionException("当前用户没有创建页面的权限，请与管理员联系。");
            }

            return provider.CreatePage(page);
        }

        public bool DeletePage(UserInfo currentUser, int pageId)
        {
            if (!PermissionManager.HasPermission(currentUser, "DeletePage"))
            {
                throw new InvalidPermissionException("当前用户没有删除页面的权限，请与管理员联系。");
            }
            return provider.DeletePage(pageId);
        }

        public PageCollection GetAllPages()
        {
            return provider.GetAllPages();
        }

        public bool UpdatePage(UserInfo currentUser, PageInfo page)
        {
            if (!PermissionManager.HasPermission(currentUser, "EditPage"))
            {
                throw new InvalidPermissionException("当前用户没有编辑页面的权限，请与管理员联系。");
            }
            return provider.UpdatePage(page);
        }

    }
}