//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供权限管理在Sql Server上的实现方法
// 完成日期：2010年03月16日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 权限管理提供类需实现的方法。
    /// </summary>
    internal interface IPermission
    {
        /// <summary>
        /// 返回指定用户是否拥有特定权限。
        /// </summary>
        /// <param name="user">用户信息。</param>
        /// <param name="siteId">当前用户所在的网站ID。</param>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>是否拥有指定权限。</returns>
        bool HasPermission(UserInfo user, int siteId, string permissionName);

        /// <summary>
        /// 添加一个权限到一个角色。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        bool AddPermissionToRole(int permissionId, int roleId);

        /// <summary>
        /// 添加一个权限到多个角色。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        bool AddPermissionToRoles(int permissionId, int[] roleIds);

        /// <summary>
        /// 添加多个权限到一个角色。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        bool AddPermissionsToRole(int[] permissionIds, int roleId);

        /// <summary>
        /// 添加多个权限到多个角色。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        bool AddPermissionsToRoles(int[] permissionIds, int[] roleIds);

        /// <summary>
        /// 创建权限。
        /// </summary>
        /// <param name="permission">权限信息。</param>
        /// <returns>如果创建成功，返回权限信息，否则返回空。</returns>
        PermissionInfo CreatePermission(PermissionInfo permission);

        /// <summary>
        /// 创建多个权限。
        /// </summary>
        /// <param name="permissions">待创建权限列表。</param>
        /// <returns>是否创建成功。</returns>
        bool CreatePermissions(PermissionCollection permissions);

        /// <summary>
        /// 删除一个权限。
        /// </summary>
        /// <param name="permissionId">待删除权限ID。</param>
        /// <returns>是否删除成功。</returns>
        bool DeletePermission(int permissionId);

        /// <summary>
        /// 删除多个权限。
        /// </summary>
        /// <param name="permissionIds">待删除权限的ID列表。</param>
        /// <returns>是否删除成功。</returns>
        bool DeletePermissions(int[] permissionIds);

        /// <summary>
        /// 返回权限列表。
        /// </summary>
        /// <returns>权限列表。</returns>
        PermissionCollection GetAllPermissions();

        /// <summary>
        /// 返回指定角色所具有的权限列表。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>权限列表。</returns>
        PermissionCollection GetPermissionsForRole(int roleId);

        /// <summary>
        /// 返回指定用户所具有的权限列表。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <returns>权限列表。</returns>
        PermissionCollection GetPermissionsForUser(Guid userId);

        /// <summary>
        /// 返回指定权限信息。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <returns>权限信息。</returns>
        PermissionInfo GetPermissionInfo(int permissionId);

        /// <summary>
        /// 返回指定权限信息。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>权限信息。</returns>
        PermissionInfo GetPermissionInfo(string permissionName);

        /// <summary>
        /// 从指定角色中移除特定权限。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        bool RemovePermissionFromRole(int permissionId, int roleId);

        /// <summary>
        /// 从指定角色中移除多个权限。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        bool RemovePermissionsFromRole(int[] permissionIds, int roleId);

        /// <summary>
        /// 从多个角色中移除特定权限。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        bool RemovePermissionFromRoles(int permissionId, int[] roleIds);

        /// <summary>
        /// 从多个角色中移除多个权限。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        bool RemovePermissionsFromRoles(int[] permissionIds, int[] roleIds);

        /// <summary>
        /// 返回指定权限是否存在。
        /// </summary>
        /// <param name="permissionName">权限名。</param>
        /// <returns>是否存在。</returns>
        bool IsPermissionExists(string permissionName);
    }
}
