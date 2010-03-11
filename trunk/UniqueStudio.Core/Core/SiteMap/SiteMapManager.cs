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
    /// <summary>
    /// 提供网站地图管理的方法
    /// </summary>
    public class SiteMapManager
    {
        private static readonly ISiteMap provider = DALFactory.CreateSiteMap();

        /// <summary>
        /// 初始化<see cref="SiteMapManager"/>类的实例
        /// </summary>
        public SiteMapManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="page">页面信息</param>
        /// <returns>如果创建成功，返回页面信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建页面的权限时抛出该异常</exception>
        public PageInfo CreatePage(UserInfo currentUser, PageInfo page)
        {
            if (!PermissionManager.HasPermission(currentUser, "CreatePage"))
            {
                throw new InvalidPermissionException("当前用户没有创建页面的权限，请与管理员联系。");
            }

            return provider.CreatePage(page);
        }

        /// <summary>
        /// 删除指定页面
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="pageId">待删除页面ID</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除页面的权限时抛出该异常</exception>
        public bool DeletePage(UserInfo currentUser, int pageId)
        {
            if (!PermissionManager.HasPermission(currentUser, "DeletePage"))
            {
                throw new InvalidPermissionException("当前用户没有删除页面的权限，请与管理员联系。");
            }
            return provider.DeletePage(pageId);
        }

        /// <summary>
        /// 返回所有页面
        /// </summary>
        /// <returns>页面的集合</returns>
        public PageCollection GetAllPages()
        {
            return provider.GetAllPages();
        }

        /// <summary>
        /// 更新页面信息
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="page">页面信息</param>
        /// <returns>是否更新成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑页面的权限时抛出该异常</exception>
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