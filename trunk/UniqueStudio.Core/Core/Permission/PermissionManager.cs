using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.Permission
{
    /// <summary>
    /// 提供权限管理的方法
    /// </summary>
    public class PermissionManager
    {
        private static readonly IPermission provider = DALFactory.CreatePermission();

        /// <summary>
        /// 初始化<see cref="PermissionManager"/>类的实例
        /// </summary>
        public PermissionManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 返回指定用户是否拥有特定权限
        /// </summary>
        /// <remarks>当前版本需提供用户ID，在后续版本中还要求提供用户SessionID</remarks>
        /// <param name="user">用户信息</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns>是否拥有指定权限</returns>
        public static bool HasPermission(UserInfo user, string permissionName)
        {
            if (!GlobalConfig.EnablePermissionCheck)
            {
                return true;
            }

            if (user == null || string.IsNullOrEmpty(permissionName))
            {
                return false;
            }
            if (user.UserId == new Guid())
            {
                return false;
            }


            //用户登录状态确认
            //if (!UserManager.IsUserOnline(user))
            //{
            //    //最好能区分是没有登录还是没有权限。。
            //    return false;
            //}

            //权限检验
            return provider.HasPermission(user, permissionName);
        }

        /// <summary>
        /// 添加多个权限到多个角色
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="permissions">权限列表</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddPermissionsToRoles(UserInfo currentUser, PermissionCollection permissions, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加一个权限到多个角色
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="permission">权限信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddPermissionToRoles(UserInfo currentUser, PermissionInfo permission, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加多个权限到一个角色
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="permissions">权限列表</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddPermissionsToRole(UserInfo currentUser, PermissionCollection permissions, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加一个权限到一个角色
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="permission">权限信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddPermissionToRole(UserInfo currentUser, PermissionInfo permission, RoleInfo role)
        {
            return provider.AddPermissionToRole(permission, role);
        }

        /// <summary>
        /// 返回权限列表
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <returns>权限列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看权限信息的权限时抛出该异常</exception>
        public PermissionCollection GetAllPermissions(UserInfo currentUser)
        {
            if (!HasPermission(currentUser, "ViewPermissionInfo"))
            {
                throw new InvalidPermissionException("当前用户没有查看权限信息的权限，请与管理员联系！");
            }

            try
            {
                return provider.GetAllPermissions();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 返回指定角色所具有的权限列表
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>权限列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常</exception>
        public PermissionCollection GetPermissionsForRole(UserInfo currentUser, RoleInfo role)
        {
            return provider.GetPermissionsForRole(role);
        }

        /// <summary>
        /// 从指定角色中移除特定权限
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="permission">权限信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool RemovePermissionFromRole(UserInfo currentUser, PermissionInfo permission, RoleInfo role)
        {
            return provider.RemovePermissionFromRole(permission, role);
        }

        /// <summary>
        /// 从指定角色中移除多个权限
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="permissions">权限列表</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool RemovePermissionsFromRole(UserInfo currentUser, PermissionCollection permissions, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从多个角色中移除特定权限
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="permission">权限信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool RemovePermissionFromRoles(UserInfo currentUser, PermissionInfo permission, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从多个角色中移除多个权限
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="permissions">权限列表</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool RemovePermissionsFromRoles(UserInfo currentUser, PermissionCollection permissions, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        private bool IsPermissionExists(string permissionName)
        {
            return provider.IsPermissionExists(permissionName);
        }
    }
}
