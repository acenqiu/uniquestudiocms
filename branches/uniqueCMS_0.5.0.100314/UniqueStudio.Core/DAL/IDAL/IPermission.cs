using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 权限管理提供类需实现的方法
    /// </summary>
    internal interface IPermission
    {
        /// <summary>
        /// 返回指定用户是否拥有特定权限
        /// </summary>
        /// <remarks>当前版本需提供用户ID，在后续版本中还要求提供用户SessionID</remarks>
        /// <param name="user">用户信息</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns>是否拥有指定权限</returns>
        bool HasPermission(UserInfo user, string permissionName);

        /// <summary>
        /// 添加多个权限到多个角色
        /// </summary>
        /// <param name="permissions">权限列表</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否添加成功</returns>
        bool AddPermissionsToRoles(PermissionCollection permissions, RoleCollection roles);

        /// <summary>
        /// 添加一个权限到多个角色
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否添加成功</returns>
        bool AddPermissionToRoles(PermissionInfo permission, RoleCollection roles);

        /// <summary>
        /// 添加多个权限到一个角色
        /// </summary>
        /// <param name="permissions">权限列表</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否添加成功</returns>
        bool AddPermissionsToRole(PermissionCollection permissions, RoleInfo role);

        /// <summary>
        /// 添加一个权限到一个角色
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否添加成功</returns>
        bool AddPermissionToRole(PermissionInfo permission, RoleInfo role);

        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <returns>如果创建成功，返回权限信息，否则返回空</returns>
        PermissionInfo CreatePermission(PermissionInfo permission);

        /// <summary>
        /// 创建多个权限
        /// </summary>
        /// <param name="permissions">待创建权限列表</param>
        /// <returns>是否创建成功</returns>
        bool CreatePermissions(PermissionCollection permissions);

        /// <summary>
        /// 删除一个权限
        /// </summary>
        /// <param name="permissionId">待删除权限ID</param>
        /// <returns>是否删除成功</returns>
        bool DeletePermission(int permissionId);

        /// <summary>
        /// 返回权限列表
        /// </summary>
        /// <returns>权限列表</returns>
        PermissionCollection GetAllPermissions();

        /// <summary>
        /// 返回指定角色所具有的权限列表
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>权限列表</returns>
        PermissionCollection GetPermissionsForRole(RoleInfo role);

        /// <summary>
        /// 返回指定用户所具有的权限列表
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>权限列表</returns>
        PermissionCollection GetPermissionsForUser(UserInfo user);

        /// <summary>
        /// 返回指定权限信息
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns>权限信息</returns>
        PermissionInfo GetPermissionInfo(int permissionId);

        /// <summary>
        /// 返回指定权限信息
        /// </summary>
        /// <param name="permissionName">权限名称</param>
        /// <returns>权限信息</returns>
        PermissionInfo GetPermissionInfo(string permissionName);

        /// <summary>
        /// 从指定角色中移除特定权限
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否移除成功</returns>
        bool RemovePermissionFromRole(PermissionInfo permission, RoleInfo role);

        /// <summary>
        /// 从指定角色中移除多个权限
        /// </summary>
        /// <param name="permissions">权限列表</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否移除成功</returns>
        bool RemovePermissionsFromRole(PermissionCollection permissions, RoleInfo role);

        /// <summary>
        /// 从多个角色中移除特定权限
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否移除成功</returns>
        bool RemovePermissionFromRoles(PermissionInfo permission, RoleCollection roles);

        /// <summary>
        /// 从多个角色中移除多个权限
        /// </summary>
        /// <param name="permissions">权限列表</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否移除成功</returns>
        bool RemovePermissionsFromRoles(PermissionCollection permissions, RoleCollection roles);

        /// <summary>
        /// 返回指定权限是否存在
        /// </summary>
        /// <param name="permissionName">权限名</param>
        /// <returns>是否存在</returns>
        bool IsPermissionExists(string permissionName);
    }
}
