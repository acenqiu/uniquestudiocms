using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface IRole
    {
        /// <summary>
        /// 将一组用户添加到某一角色中。
        /// </summary>
        /// <param name="users"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool AddUsersToRole(UserCollection users, RoleInfo role);

        /// <summary>
        /// 将一组用户添加到一组角色中。 
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool AddUsersToRoles(UserCollection users, RoleCollection roles);

        /// <summary>
        ///  将某一用户添加到某一角色中。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool AddUserToRole(UserInfo user, RoleInfo role);

        /// <summary>
        ///  将某一用户添加到一组角色中。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool AddUserToRoles(UserInfo user, RoleCollection roles);

        /// <summary>
        /// 创建新的角色。
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        RoleInfo CreateRole(RoleInfo role);

        /// <summary>
        /// 添加一组角色
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool CreateRoles(RoleCollection roles);

        /// <summary>
        /// 删除特定的角色。
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool DeleteRole(int roleId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        UserCollection GetUsersInRole(RoleInfo role);

        /// <summary>
        /// 返回所有角色的列表。
        /// </summary>
        /// <returns></returns>
        RoleCollection GetAllRoles();

        /// <summary>
        /// 返回指定角色信息
        /// </summary>
        /// <remarks>
        /// 包含角色名称、说明，权限信息，用户信息。
        /// </remarks>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        RoleInfo GetRoleInfo(int roleId);

        /// <summary>
        /// 返回指定角色信息
        /// </summary>
        /// <remarks>
        /// 包含角色名称、说明，权限信息，用户信息。
        /// </remarks>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        RoleInfo GetRoleInfo(string roleName);

        /// <summary>
        /// 返回某一用户所属的所有角色的列表。
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        RoleCollection GetRolesForUser(UserInfo user);

        /// <summary>
        /// 判断某一特定的用户是否是某一特定角色中的成员。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        bool IsUserInRole(UserInfo user, string roleName);

        /// <summary>
        /// 从某一特定角色中移除某一特定的成员。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool RemoveUserFromRole(UserInfo user, RoleInfo role);

        /// <summary>
        /// 从一组角色中移除某一特定的成员。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool RemoveUserFromRoles(UserInfo user, RoleCollection roles);

        /// <summary>
        /// 从某一特定角色中移除一组成员。
        /// </summary>
        /// <param name="users"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool RemoveUsersFromRole(UserCollection users, RoleInfo role);

        /// <summary>
        /// 从一组角色中移除一组成员。
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool RemoveUsersFromRoles(UserCollection users, RoleCollection roles);

        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否成功</returns>
        bool UpdateRolesForUser(UserInfo user);

        /// <summary>
        /// 判断某一特定的角色是否存在。
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        bool IsRoleExists(string roleName);
    }
}
