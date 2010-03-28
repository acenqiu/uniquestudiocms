//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供权限管理的方法
// 完成日期：2010年03月17日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Data.Common;

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
    /// 提供权限管理的方法。
    /// </summary>
    public class PermissionManager
    {
        private static readonly IPermission provider = DALFactory.CreatePermission();

        private UserInfo currentUser;

        /// <summary>
        /// 初始化<see cref="PermissionManager"/>类的实例。
        /// </summary>
        public PermissionManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前登录用户初始化<see cref="PermissionManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前用户。</param>
        public PermissionManager(UserInfo currentUser)
        {
            Validator.CheckNull(currentUser, "currentUser");
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 检测指定用户是否拥有特定权限。
        /// </summary>
        /// <remarks>
        /// <para>该方法在没有权限时直接抛出异常。</para>
        /// <para>仅当该方法的执行与具体处于哪个网站无关时使用该重载。</para>
        /// </remarks>
        /// <param name="user">用户信息。</param>
        /// <param name="permissionName">权限名称。</param>
        /// <param name="permissionDescription">权限说明（建议使用中文，可空）。</param>
        /// <exception cref="UniqueStudio.Common.Exceptions.DatabaseException">
        /// 当数据库出现错误时抛出该异常。</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.UserDoesNotOnlineException">
        /// 当用户不处于在线状态时抛出该异常。</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户不具有指定权限时抛出该异常。</exception>
        public static void CheckPermission(UserInfo user,string permissionName, string permissionDescription)
        {
             CheckPermission(user, 0, permissionName, permissionDescription);
        }

        /// <summary>
        /// 检测指定用户是否拥有特定权限。
        /// </summary>
        /// <remarks>
        /// <para>该方法在没有权限时直接抛出异常。</para>
        /// <para>仅当该方法的执行与具体处于哪个网站无关时使用该重载。</para>
        /// </remarks>
        /// <param name="user">用户信息。</param>
        /// <param name="siteId">当前用户所在的网站。</param>
        /// <param name="permissionName">权限名称。</param>
        /// <param name="permissionDescription">权限说明（建议使用中文，可空）。</param>
        /// <exception cref="UniqueStudio.Common.Exceptions.DatabaseException">
        /// 当数据库出现错误时抛出该异常。</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.UserDoesNotOnlineException">
        /// 当用户不处于在线状态时抛出该异常。</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户不具有指定权限时抛出该异常。</exception>
        public static void CheckPermission(UserInfo user, int siteId, string permissionName, string permissionDescription)
        {
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
                hasPermission = provider.HasPermission(user,siteId, permissionName);
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
        /// 返回指定用户是否拥有特定权限。
        /// </summary>
        /// <remarks>
        /// <para>当前版本需提供用户ID，在后续版本中还要求提供用户SessionID。</para>
        /// <para>仅当该方法的执行与具体处于哪个网站无关时使用该重载。</para>
        /// </remarks>
        /// <param name="user">用户信息。</param>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>是否拥有指定权限。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.DatabaseException">
        /// 当数据库出现错误时抛出该异常。</exception>
        public static bool HasPermission(UserInfo user, string permissionName)
        {
            return HasPermission(user, 0, permissionName);
        }

        /// <summary>
        /// 返回指定用户是否拥有特定权限。
        /// </summary>
        /// <remarks>当前版本需提供用户ID，在后续版本中还要求提供用户SessionID。</remarks>
        /// <param name="user">用户信息。</param>
        /// <param name="siteId">当前用户所在的网站。</param>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>是否拥有指定权限。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.DatabaseException">
        /// 当数据库出现错误时抛出该异常。</exception>
        public static bool HasPermission(UserInfo user, int siteId, string permissionName)
        {
            try
            {
                CheckPermission(user,siteId, permissionName, null);
                return true;
            }
            catch (DatabaseException)
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
        /// 添加一个权限到一个角色。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddPermissionToRole(int permissionId, int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return AddPermissionToRole(currentUser, permissionId, roleId);
        }

        /// <summary>
        /// 添加一个权限到一个角色。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddPermissionToRole(UserInfo currentUser, int permissionId, int roleId)
        {
            Validator.CheckNotPositive(permissionId, "permissionId");
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "EditPermission", "编辑权限");

            try
            {
                return provider.AddPermissionToRole(permissionId, roleId);
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
        /// 添加一个权限到多个角色。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddPermissionToRoles(int permissionId, int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return AddPermissionToRoles(currentUser, permissionId, roleIds);
        }

        /// <summary>
        /// 添加一个权限到多个角色。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddPermissionToRoles(UserInfo currentUser, int permissionId, int[] roleIds)
        {
            Validator.CheckNotPositive(permissionId, "permissionId");
            Validator.CheckNull(roleIds, "roleIds");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleIds");
            }
            PermissionManager.CheckPermission(currentUser, "EditPermission", "编辑权限");

            try
            {
                return provider.AddPermissionToRoles(permissionId, roleIds);
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
        /// 添加多个权限到一个角色。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddPermissionsToRole(int[] permissionIds, int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return AddPermissionsToRole(currentUser, permissionIds, roleId);
        }

        /// <summary>
        /// 添加多个权限到一个角色。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddPermissionsToRole(UserInfo currentUser, int[] permissionIds, int roleId)
        {
            Validator.CheckNull(permissionIds, "permissionIds");
            foreach (int permissionId in permissionIds)
            {
                Validator.CheckNotPositive(permissionId, "permissionIds");
            }
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "EditPermission", "编辑权限");

            try
            {
                return provider.AddPermissionsToRole(permissionIds, roleId);
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
        /// 添加多个权限到多个角色。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddPermissionsToRoles(int[] permissionIds, int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return AddPermissionsToRoles(currentUser, permissionIds, roleIds);
        }

        /// <summary>
        /// 添加多个权限到多个角色。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddPermissionsToRoles(UserInfo currentUser, int[] permissionIds, int[] roleIds)
        {
            Validator.CheckNull(permissionIds, "permissionIds");
            foreach (int permissionId in permissionIds)
            {
                Validator.CheckNotPositive(permissionId, "permissionIds");
            }
            Validator.CheckNull(roleIds, "roleIds");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleIds");
            }
            PermissionManager.CheckPermission(currentUser, "EditPermission", "编辑权限");

            try
            {
                return provider.AddPermissionsToRoles(permissionIds, roleIds);
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
        /// 返回权限列表。
        /// </summary>
        /// <returns>权限列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看权限信息的权限时抛出该异常。</exception>
        public PermissionCollection GetAllPermissions()
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return GetAllPermissions(currentUser);
        }

        /// <summary>
        /// 返回权限列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <returns>权限列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看权限信息的权限时抛出该异常。</exception>
        public PermissionCollection GetAllPermissions(UserInfo currentUser)
        {
            CheckPermission(currentUser, "ViewPermissionInfo", "查看权限信息");

            try
            {
                return provider.GetAllPermissions();
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
        /// 返回指定角色所具有的权限列表。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>权限列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public PermissionCollection GetPermissionsForRole(int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return GetPermissionsForRole(currentUser, roleId);
        }

        /// <summary>
        /// 返回指定角色所具有的权限列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>权限列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public PermissionCollection GetPermissionsForRole(UserInfo currentUser, int roleId)
        {
            Validator.CheckNotPositive(roleId, "roleId");
            CheckPermission(currentUser, "ViewPermissionInfo", "查看权限信息");

            try
            {
                return provider.GetPermissionsForRole(roleId);
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
        /// 从指定角色中移除特定权限。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemovePermissionFromRole(int permissionId, int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return RemovePermissionFromRole(currentUser, permissionId, roleId);
        }

        /// <summary>
        /// 从指定角色中移除特定权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemovePermissionFromRole(UserInfo currentUser, int permissionId, int roleId)
        {
            Validator.CheckNotPositive(permissionId, "permissionId");
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "EditPermission", "编辑权限");

            try
            {
                return provider.RemovePermissionFromRole(permissionId, roleId);
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
        /// 从指定角色中移除多个权限。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemovePermissionsFromRole(int[] permissionIds, int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return RemovePermissionsFromRole(currentUser, permissionIds, roleId);
        }

        /// <summary>
        /// 从指定角色中移除多个权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemovePermissionsFromRole(UserInfo currentUser, int[] permissionIds, int roleId)
        {
            Validator.CheckNull(permissionIds, "permissionIds");
            foreach (int permissionId in permissionIds)
            {
                Validator.CheckNotPositive(permissionId, "permissionIds");
            }
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "EditPermission", "编辑权限");

            try
            {
                return provider.RemovePermissionsFromRole(permissionIds, roleId);
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
        /// 从多个角色中移除特定权限。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemovePermissionFromRoles(int permissionId, int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return RemovePermissionFromRoles(currentUser, permissionId, roleIds);
        }

        /// <summary>
        /// 从多个角色中移除特定权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemovePermissionFromRoles(UserInfo currentUser, int permissionId, int[] roleIds)
        {
            Validator.CheckNotPositive(permissionId, "permissionId");
            Validator.CheckNull(roleIds, "roleIds");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleId");
            }
            PermissionManager.CheckPermission(currentUser, "EditPermission", "编辑权限");

            try
            {
                return provider.RemovePermissionFromRoles(permissionId, roleIds);
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
        /// 从多个角色中移除多个权限。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemovePermissionsFromRoles(int[] permissionIds, int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PermissionManager(UserInfo)实例化该类。");
            }
            return RemovePermissionsFromRoles(currentUser, permissionIds, roleIds);
        }

        /// <summary>
        /// 从多个角色中移除多个权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemovePermissionsFromRoles(UserInfo currentUser, int[] permissionIds, int[] roleIds)
        {
            Validator.CheckNull(permissionIds, "permissionIds");
            foreach (int permissionId in permissionIds)
            {
                Validator.CheckNotPositive(permissionId, "permissionId");
            }
            Validator.CheckNull(roleIds, "roleIds");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleIds");
            }
            PermissionManager.CheckPermission(currentUser, "EditPermission", "编辑权限");

            try
            {
                return provider.RemovePermissionsFromRoles(permissionIds, roleIds);
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

        private bool IsPermissionExists(string permissionName)
        {
            Validator.CheckStringNull(permissionName, "permissionName");

            try
            {
                return provider.IsPermissionExists(permissionName);
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