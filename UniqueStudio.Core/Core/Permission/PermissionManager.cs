using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.Core.User;

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
        /// 检测指定用户是否拥有特定权限
        /// </summary>
        /// <remarks>该方法在没有权限时直接抛出异常</remarks>
        /// <param name="user">用户信息</param>
        /// <param name="permissionName">权限名称</param>
        /// <param name="permissionDescription">权限说明（建议使用中文，可空）</param>
        /// <exception cref="UniqueStudio.Common.Exceptions.DatabaseException">
        /// 当数据库出现错误时抛出该异常</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.UserDoesNotOnlineException">
        /// 当用户不处于在线状态时抛出该异常</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户不具有指定权限时抛出该异常</exception>
        public static void CheckPermission(UserInfo user, string permissionName, string permissionDescription)
        {
            if (!GlobalConfig.EnablePermissionCheck)
            {
                return;
            }

            Validator.CheckNull(user, "user");
            Validator.CheckStringNull(permissionName, "permissionName");
            Validator.CheckGuid(user.UserId, "user");

            //用户登录状态确认
            if (!UserManager.IsUserOnline(user))
            {
                throw new UserDoesNotOnlineException();
            }

            //权限检验
            bool hasPermission;
            try
            {
                hasPermission = provider.HasPermission(user, permissionName);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }

            if (!hasPermission)
            {
                if (string.IsNullOrEmpty(permissionDescription))
                {
                    throw new InvalidPermissionException(permissionName);
                }
                else
                {
                    throw new InvalidPermissionException(permissionName, permissionDescription);
                }
            }
        }

        /// <summary>
        /// 返回指定用户是否拥有特定权限
        /// </summary>
        /// <remarks>当前版本需提供用户ID，在后续版本中还要求提供用户SessionID</remarks>
        /// <param name="user">用户信息</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns>是否拥有指定权限</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.DatabaseException">
        /// 当数据库出现错误时抛出该异常</exception>
        public static bool HasPermission(UserInfo user, string permissionName)
        {
            try
            {
                CheckPermission(user, permissionName, null);
                return true;
            }
            catch (DatabaseException ex)
            {
                //如果是数据库异常，则抛出
                throw;
            }
            catch
            {
                return false;
            }
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
            CheckPermission(currentUser, "ViewPermissionInfo", "查看权限信息");

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
