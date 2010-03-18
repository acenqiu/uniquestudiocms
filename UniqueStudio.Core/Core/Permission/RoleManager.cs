//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供角色管理的方法。
// 完成日期：2010年03月17日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.DAL.Permission;

namespace UniqueStudio.Core.Permission
{
    /// <summary>
    /// 提供角色管理的方法。
    /// </summary>
    public class RoleManager
    {
        private static readonly IRole provider = DALFactory.CreateRole();

        private UserInfo currentUser;

        /// <summary>
        /// 初始化<see cref="RoleManager"/>类的实例。
        /// </summary>
        public RoleManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前用户初始化<see cref="RoleManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前用户。</param>
        public RoleManager(UserInfo currentUser)
        {
            Validator.CheckNull(currentUser, "currentUser");
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 判断特定的用户是否是特定角色中的成员。
        /// </summary>
        /// <remarks>该方法应当被废弃！</remarks>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleName">角色名称。</param>
        /// <returns>是否是特定角色的成员。</returns>
        public static bool IsUserInRole(Guid userId, string roleName)
        {
            //TODO: Still have some problems.
            Validator.CheckGuid(userId, "userId");
            Validator.CheckStringNull(roleName, "roleName");

            try
            {
                return provider.IsUserInRole(userId, roleName);
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
        ///  将某一用户添加到某一角色中。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddUserToRole(Guid userId, int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return AddUserToRole(currentUser, userId, roleId);
        }

        /// <summary>
        ///  将某一用户添加到某一角色中。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddUserToRole(UserInfo currentUser, Guid userId, int roleId)
        {
            Validator.CheckGuid(userId, "userId");
            Validator.CheckNotPositive(roleId, "roleId");

            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.AddUserToRole(userId, roleId);
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
        ///  将某一用户添加到一组角色中。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddUserToRoles(Guid userId, int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return AddUserToRoles(currentUser, userId, roleIds);
        }

        /// <summary>
        ///  将某一用户添加到一组角色中。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddUserToRoles(UserInfo currentUser, Guid userId, int[] roleIds)
        {
            Validator.CheckGuid(userId, "userId");
            Validator.CheckNull(roleIds, "roleIds");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleIds");
            }

            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.AddUserToRoles(userId, roleIds);
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
        /// 将多个用户添加到某一角色中。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddUsersToRole(Guid[] userIds, int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return AddUsersToRole(currentUser, userIds, roleId);
        }

        /// <summary>
        /// 将多个用户添加到某一角色中。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddUsersToRole(UserInfo currentUser, Guid[] userIds, int roleId)
        {
            Validator.CheckNull(userIds, "userIds");
            foreach (Guid userId in userIds)
            {
                Validator.CheckGuid(userId, "userIds");
            }
            Validator.CheckNotPositive(roleId, "roleId");

            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.AddUsersToRole(userIds, roleId);
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
        /// 将一组用户添加到一组角色中。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddUsersToRoles(Guid[] userIds, int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return AddUsersToRoles(currentUser, userIds, roleIds);
        }

        /// <summary>
        /// 将一组用户添加到一组角色中。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool AddUsersToRoles(UserInfo currentUser, Guid[] userIds, int[] roleIds)
        {
            Validator.CheckNull(userIds, "userIds");
            foreach (Guid userId in userIds)
            {
                Validator.CheckGuid(userId, "userIds");
            }
            Validator.CheckNull(roleIds, "roleIds");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleIds");
            }

            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.AddUsersToRoles(userIds, roleIds);
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
        /// 创建新的角色 。
        /// </summary>
        /// <param name="role">角色信息。</param>
        /// <returns>如果添加成功，返回角色信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建角色的权限时抛出该异常。</exception>
        public RoleInfo CreateRole(RoleInfo role)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return CreateRole(currentUser, role);
        }

        /// <summary>
        /// 创建新的角色 。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="role">角色信息。</param>
        /// <returns>如果添加成功，返回角色信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建角色的权限时抛出该异常。</exception>
        public RoleInfo CreateRole(UserInfo currentUser, RoleInfo role)
        {
            Validator.CheckNull(role, "role");
            Validator.CheckStringNull(role.RoleName, "role");
            Validator.CheckNegative(role.SiteId, "role");
            if (role.Permissions != null)
            {
                foreach (PermissionInfo permission in role.Permissions)
                {
                    Validator.CheckNotPositive(permission.PermissionId,"role.Permission");
                }
            }

            PermissionManager.CheckPermission(currentUser, "CreateRole", "创建角色");

            if (IsRoleExists(role.SiteId, role.RoleName))
            {
                throw new Exception("该角色已经存在，请重新设置！");
            }

            try
            {
                return provider.CreateRole(role);
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
        /// 创建多个角色。
        /// </summary>
        /// <remarks>该方法慎重执行，可能导致连接池溢出。</remarks>
        /// <param name="roles">待创建角色列表。</param>
        /// <returns>是否创建成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建角色的权限时抛出该异常。</exception>
        public bool CreateRoles(RoleCollection roles)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return CreateRoles(currentUser, roles);
        }

        /// <summary>
        /// 创建多个角色。
        /// </summary>
        /// <remarks>该方法慎重执行，可能导致连接池溢出。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="roles">待创建角色列表。</param>
        /// <returns>是否创建成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建角色的权限时抛出该异常。</exception>
        public bool CreateRoles(UserInfo currentUser, RoleCollection roles)
        {
            Validator.CheckNull(roles, "roles");
            foreach (RoleInfo role in roles)
            {
                Validator.CheckStringNull(role.RoleName, "roles");
                Validator.CheckNegative(role.SiteId, "roles");
                if (role.Permissions != null)
                {
                    foreach (PermissionInfo permission in role.Permissions)
                    {
                        Validator.CheckNotPositive(permission.PermissionId, "role.Permission");
                    }
                }
                if (IsRoleExists(role.SiteId, role.RoleName))
                {
                    throw new Exception("该角色已经存在，请重新设置！");
                }
            }

            PermissionManager.CheckPermission(currentUser, "CreateRole", "创建角色");

            try
            {
                return provider.CreateRoles(roles);
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
        /// 删除特定的角色。
        /// </summary>
        /// <param name="roleId">待删除权限ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool DeleteRole(int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return DeleteRole(currentUser, roleId);
        }

        /// <summary>
        /// 删除特定的角色。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="roleId">待删除权限ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool DeleteRole(UserInfo currentUser, int roleId)
        {
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "DeleteRole", "删除角色");

            try
            {
                return provider.DeleteRole(roleId);
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
        /// 删除多个角色。
        /// </summary>
        /// <param name="roleIds">待删除角色的ID列表。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool DeleteRoles(int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return DeleteRoles(currentUser, roleIds);
        }

        /// <summary>
        /// 删除多个角色。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="roleIds">待删除角色的ID列表。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool DeleteRoles(UserInfo currentUser, int[] roleIds)
        {
            Validator.CheckNull(roleIds, "roleIds");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleIds");
            }
            PermissionManager.CheckPermission(currentUser, "DeleteRole", "删除角色");

            try
            {
                return provider.DeleteRoles(roleIds);
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
        /// 返回指定角色下的用户列表。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>用户列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public UserCollection GetUsersInRole(int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return GetUsersInRole(currentUser, roleId);
        }

        /// <summary>
        /// 返回指定角色下的用户列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>用户列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public UserCollection GetUsersInRole(UserInfo currentUser, int roleId)
        {
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "ViewRoleInfo", "查看角色信息");

            try
            {
                return provider.GetUsersInRole(roleId);
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
        /// 返回所有角色的列表。
        /// </summary>
        /// <returns>角色列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public RoleCollection GetAllRoles()
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return GetAllRoles(currentUser);
        }

        /// <summary>
        /// 返回所有角色的列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <returns>角色列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public RoleCollection GetAllRoles(UserInfo currentUser)
        {
            PermissionManager.CheckPermission(currentUser, "ViewRoleInfo", "查看角色信息");

            try
            {
                return provider.GetAllRoles();
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
        /// 获得指定角色的信息。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>角色信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色的权限时抛出该异常。</exception>
        public RoleInfo GetRoleInfo(int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return GetRoleInfo(currentUser, roleId);
        }

        /// <summary>
        /// 获得指定角色的信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>角色信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色的权限时抛出该异常。</exception>
        public RoleInfo GetRoleInfo(UserInfo currentUser, int roleId)
        {
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "ViewRoleInfo", "查看角色信息");

            try
            {
                return provider.GetRoleInfo(roleId);
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
        /// 获得指定角色的信息。
        /// </summary>
        /// <param name="roleName">角色名称。</param>
        /// <returns>角色信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public RoleInfo GetRoleInfo(string roleName)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return GetRoleInfo(currentUser, roleName);
        }

        /// <summary>
        /// 获得指定角色的信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="roleName">角色名称。</param>
        /// <returns>角色信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public RoleInfo GetRoleInfo(UserInfo currentUser, string roleName)
        {
            Validator.CheckStringNull(roleName, "roleName");
            PermissionManager.CheckPermission(currentUser, "ViewRoleInfo", "查看角色信息");

            try
            {
                return provider.GetRoleInfo(roleName);
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
        /// 返回某一用户所属的所有角色的列表。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <returns>角色的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public RoleCollection GetRolesForUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return GetRolesForUser(currentUser, userId);
        }

        /// <summary>
        /// 返回某一用户所属的所有角色的列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">用户ID。</param>
        /// <returns>角色的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常。</exception>
        public RoleCollection GetRolesForUser(UserInfo currentUser, Guid userId)
        {
            Validator.CheckGuid(userId, "userId");
            PermissionManager.CheckPermission(currentUser, "ViewRoleInfo", "查看角色信息");

            try
            {
                return provider.GetRolesForUser(userId);
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
        /// 从某一特定角色中移除某一特定的用户。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemoveUserFromRole(Guid userId, int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return RemoveUserFromRole(currentUser, userId, roleId);
        }

        /// <summary>
        /// 从某一特定角色中移除某一特定的用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemoveUserFromRole(UserInfo currentUser, Guid userId, int roleId)
        {
            Validator.CheckGuid(userId, "userId");
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.RemoveUserFromRole(userId, roleId);
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
        /// 从一组角色中移除某一特定的用户。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemoveUserFromRoles(Guid userId, int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return RemoveUserFromRoles(currentUser, userId, roleIds);
        }

        /// <summary>
        /// 从一组角色中移除某一特定的用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemoveUserFromRoles(UserInfo currentUser, Guid userId, int[] roleIds)
        {
            Validator.CheckGuid(userId, "userId");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleIds");
            }
            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.RemoveUserFromRoles(userId, roleIds);
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
        /// 从某一特定角色中移除多个用户。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemoveUsersFromRole(Guid[] userIds, int roleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return RemoveUsersFromRole(currentUser, userIds, roleId);
        }

        /// <summary>
        /// 从某一特定角色中移除多个用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemoveUsersFromRole(UserInfo currentUser, Guid[] userIds, int roleId)
        {
            Validator.CheckNull(userIds, "userIds");
            foreach (Guid userId in userIds)
            {
                Validator.CheckGuid(userId, "userId");
            }
            Validator.CheckNotPositive(roleId, "roleId");
            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.RemoveUsersFromRole(userIds, roleId);
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
        /// 从多个角色中移除多个用户。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemoveUsersFromRoles(Guid[] userIds, int[] roleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return RemoveUsersFromRoles(currentUser, userIds, roleIds);
        }

        /// <summary>
        /// 从多个角色中移除多个用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool RemoveUsersFromRoles(UserInfo currentUser, Guid[] userIds, int[] roleIds)
        {
            Validator.CheckNull(userIds, "userIds");
            foreach (Guid userId in userIds)
            {
                Validator.CheckGuid(userId, "userIds");
            }
            Validator.CheckNull(roleIds, "roleIds");
            foreach (int roleId in roleIds)
            {
                Validator.CheckNotPositive(roleId, "roleIds");
            }
            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.RemoveUsersFromRoles(userIds, roleIds);
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
        /// 更新角色信息。
        /// </summary>
        /// <remarks>不更新角色下的用户列表。</remarks>
        /// <param name="role">待更新角色信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool UpdateRole(RoleInfo role)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return UpdateRole(currentUser, role);
        }

        /// <summary>
        /// 更新角色信息。
        /// </summary>
        /// <remarks>不更新角色下的用户列表。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="role">待更新角色信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool UpdateRole(UserInfo currentUser, RoleInfo role)
        {
            Validator.CheckNull(role, "role");
            Validator.CheckNotPositive(role.RoleId, "role.RoleId");
            Validator.CheckStringNull(role.RoleName, "role.RoleName");

            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑角色");

            try
            {
                return provider.UpdateRole(role);
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
        /// 更新指定用户的角色信息
        /// </summary>
        /// <param name="user">用户信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool UpdateRolesForUser(UserInfo user)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return UpdateRolesForUser(currentUser, user);
        }

        /// <summary>
        /// 更新指定用户的角色信息
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="user">用户信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常。</exception>
        public bool UpdateRolesForUser(UserInfo currentUser, UserInfo user)
        {
            Validator.CheckNull(user, "user");
            Validator.CheckGuid(user.UserId, "user.UserId");
            Validator.CheckNull(user.Roles, "user.Roles");
            foreach (RoleInfo role in user.Roles)
            {
                Validator.CheckNotPositive(role.RoleId, "user.Roles[i].RoleId");
            }

            PermissionManager.CheckPermission(currentUser, "EditRole", "编辑权限");

            try
            {
                return provider.UpdateRolesForUser(user);
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
        /// 判断某一特定的角色是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="roleName">角色名称。</param>
        /// <returns>是否存在。</returns>
        public bool IsRoleExists(int siteId, string roleName)
        {
            Validator.CheckNegative(siteId, "siteId");
            Validator.CheckStringNull(roleName, "roleName");

            try
            {
                return provider.IsRoleExists(siteId, roleName);
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
