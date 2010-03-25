//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：角色管理提供类需实现的方法
// 完成日期：2010年03月16日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 角色管理提供类需实现的方法。
    /// </summary>
    internal interface IRole
    {
        /// <summary>
        ///  将某一用户添加到某一角色中。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        bool AddUserToRole(Guid userId, int roleId);

        /// <summary>
        /// 将多个用户添加到某一角色中。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        bool AddUsersToRole(Guid[] userIds, int roleId);

        /// <summary>
        ///  将某一用户添加到一组角色中。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        bool AddUserToRoles(Guid userId, int[] roleIds);

        /// <summary>
        /// 将一组用户添加到一组角色中。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        bool AddUsersToRoles(Guid[] userIds, int[] roleIds);

        /// <summary>
        /// 创建新的角色。
        /// </summary>
        /// <param name="role">角色信息。</param>
        /// <returns>如果添加成功，返回角色信息，否则返回空。</returns>
        RoleInfo CreateRole(RoleInfo role);

        /// <summary>
        /// 创建多个角色。
        /// </summary>
        /// <param name="roles">待创建角色列表。</param>
        /// <returns>是否创建成功。</returns>
        bool CreateRoles(RoleCollection roles);

        /// <summary>
        /// 删除特定的角色。
        /// </summary>
        /// <param name="roleId">待删除角色ID。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteRole(int roleId);

        /// <summary>
        /// 删除多个角色。
        /// </summary>
        /// <param name="roleIds">待删除角色的ID列表。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteRoles(int[] roleIds);

        /// <summary>
        /// 返回指定角色下的用户列表。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>用户列表。</returns>
        UserCollection GetUsersInRole(int roleId);

        /// <summary>
        /// 返回所有角色的列表。
        /// </summary>
        /// <returns>角色列表。</returns>
        RoleCollection GetAllRoles();

        /// <summary>
        /// 获得指定角色的信息。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>角色信息。</returns>
        RoleInfo GetRoleInfo(int roleId);

        /// <summary>
        /// 获得指定角色的信息。
        /// </summary>
        /// <param name="roleName">角色名称。</param>
        /// <returns>角色信息。</returns>
        RoleInfo GetRoleInfo(string roleName);

        /// <summary>
        /// 返回某一用户所属的所有角色的列表。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <returns>角色的集合。</returns>
        RoleCollection GetRolesForUser(Guid userId);

        /// <summary>
        /// 判断特定的用户是否是特定角色中的成员。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleName">角色名称。</param>
        /// <returns>是否是特定角色的成员。</returns>
        bool IsUserInRole(Guid userId, string roleName);

        /// <summary>
        /// 从某一特定角色中移除某一特定的用户。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        bool RemoveUserFromRole(Guid userId, int roleId);

        /// <summary>
        /// 从一组角色中移除某一特定的用户。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        bool RemoveUserFromRoles(Guid userId, int[] roleIds);

        /// <summary>
        /// 从某一特定角色中移除多个用户。
        /// </summary>
        /// <param name="userId">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        bool RemoveUsersFromRole(Guid[] userIds, int roleId);

        /// <summary>
        /// 从多个角色中移除多个用户。
        /// </summary>
        /// <param name="userId">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        bool RemoveUsersFromRoles(Guid[] userIds, int[] roleIds);

        /// <summary>
        /// 更新角色信息。
        /// </summary>
        /// <remarks>不更新角色下的用户列表。</remarks>
        /// <param name="role">待更新角色信息。</param>
        /// <returns>是否更新成功。</returns>
        bool UpdateRole(RoleInfo role);

        /// <summary>
        /// 更新指定用户的角色信息。
        /// </summary>
        /// <param name="user">用户信息。</param>
        /// <returns>是否更新成功。</returns>
        bool UpdateRolesForUser(UserInfo user);

        /// <summary>
        /// 判断某一特定的角色是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="roleName">角色名称。</param>
        /// <returns>是否存在</returns>
        bool IsRoleExists(int siteId, string roleName);
    }
}
