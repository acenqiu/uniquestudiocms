using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 角色管理提供类需实现的方法
    /// </summary>
    internal interface IRole
    {
        /// <summary>
        /// 将多个用户添加到某一角色中
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="role">角色列表</param>
        /// <returns>是否添加成功</returns>
        bool AddUsersToRole(UserCollection users, RoleInfo role);

        /// <summary>
        /// 将一组用户添加到一组角色中
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否添加成功</returns>
        bool AddUsersToRoles(UserCollection users, RoleCollection roles);

        /// <summary>
        ///  将某一用户添加到某一角色中
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否添加成功</returns>
        bool AddUserToRole(UserInfo user, RoleInfo role);

        /// <summary>
        ///  将某一用户添加到一组角色中
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否添加成功</returns>
        bool AddUserToRoles(UserInfo user, RoleCollection roles);

        /// <summary>
        /// 创建新的角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>如果添加成功，返回角色信息，否则返回空</returns>
        RoleInfo CreateRole(RoleInfo role);

        /// <summary>
        /// 创建多个角色
        /// </summary>
        /// <remarks>返回类型可能在后续版本中修改</remarks>
        /// <param name="roles">待创建角色列表</param>
        /// <returns>是否创建成功</returns>
        bool CreateRoles(RoleCollection roles);

        /// <summary>
        /// 删除特定的角色。
        /// </summary>
        /// <param name="roleId">待删除权限ID</param>
        /// <returns>是否删除成功</returns>
        bool DeleteRole(int roleId);

        /// <summary>
        /// 返回指定角色下的用户列表
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>用户列表</returns>
        UserCollection GetUsersInRole(RoleInfo role);

        /// <summary>
        /// 返回所有角色的列表。
        /// </summary>
        /// <returns>角色列表</returns>
        RoleCollection GetAllRoles();

        /// <summary>
        /// 获得指定角色的信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色信息</returns>
        RoleInfo GetRoleInfo(int roleId);

        /// <summary>
        /// 获得指定角色的信息
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>角色信息</returns>
        RoleInfo GetRoleInfo(string roleName);

        /// <summary>
        /// 返回某一用户所属的所有角色的列表
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>角色的集合</returns>
        RoleCollection GetRolesForUser(UserInfo user);

        /// <summary>
        /// 判断特定的用户是否是特定角色中的成员
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="roleName">角色名称</param>
        /// <returns>是否是特定角色的成员</returns>
        bool IsUserInRole(UserInfo user, string roleName);

        /// <summary>
        /// 从某一特定角色中移除某一特定的用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否移除成功</returns>
        bool RemoveUserFromRole(UserInfo user, RoleInfo role);

        /// <summary>
        /// 从一组角色中移除某一特定的用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否移除成功</returns>
        bool RemoveUserFromRoles(UserInfo user, RoleCollection roles);

        /// <summary>
        /// 从某一特定角色中移除多个用户
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否移除成功</returns>
        bool RemoveUsersFromRole(UserCollection users, RoleInfo role);

        /// <summary>
        /// 从多个角色中移除多个用户
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否移除成功</returns>
        bool RemoveUsersFromRoles(UserCollection users, RoleCollection roles);

        /// <summary>
        /// 更新指定用户的角色信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否更新成功</returns>
        bool UpdateRolesForUser(UserInfo user);

        /// <summary>
        /// 判断某一特定的角色是否存在
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>是否存在</returns>
        bool IsRoleExists(string roleName);
    }
}
