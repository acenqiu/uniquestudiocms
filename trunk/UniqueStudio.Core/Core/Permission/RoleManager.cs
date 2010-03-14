using System;
using System.Collections.Generic;
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
    /// 提供角色管理的方法
    /// </summary>
    public class RoleManager
    {
        private static readonly IRole provider = DALFactory.CreateRole();

        private UserInfo currentUser;

        /// <summary>
        /// 判断特定的用户是否是特定角色中的成员
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="roleName">角色名称</param>
        /// <returns>是否是特定角色的成员</returns>
        public static bool IsUserInRole(UserInfo user, string roleName)
        {
            Validator.CheckNull(user, "user");
            Validator.CheckStringNull(roleName, "roleName");

            try
            {
                return provider.IsUserInRole(user, roleName);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 初始化<see cref="RoleManager"/>类的实例
        /// </summary>
        public RoleManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前用户初始化<see cref="RoleManager"/>类的实例
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        public RoleManager(UserInfo currentUser)
        {
            Validator.CheckNull(currentUser, "currentUser");

            this.currentUser = currentUser;
        }

        /// <summary>
        /// 将多个用户添加到某一角色中
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="role">角色列表</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddUsersToRole(UserCollection users, RoleInfo role)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用RoleManager(UserInfo)实例化该类。");
            }
            return AddUsersToRole(currentUser, users, role);
        }

        /// <summary>
        /// 将多个用户添加到某一角色中
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="users">用户列表</param>
        /// <param name="role">角色列表</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddUsersToRole(UserInfo currentUser, UserCollection users, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将一组用户添加到一组角色中
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="users">用户列表</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddUsersToRoles(UserInfo currentUser, UserCollection users, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  将某一用户添加到某一角色中
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddUserToRole(UserInfo currentUser, UserInfo user, RoleInfo role)
        {
            return provider.AddUserToRole(user, role);
        }

        /// <summary>
        ///  将某一用户添加到一组角色中
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否添加成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool AddUserToRoles(UserInfo currentUser, UserInfo user, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建新的角色
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>如果添加成功，返回角色信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建角色的权限时抛出该异常</exception>
        public RoleInfo CreateRole(UserInfo currentUser, RoleInfo role)
        {
            return provider.CreateRole(role);
        }

        /// <summary>
        /// 创建多个角色
        /// </summary>
        /// <remarks>返回类型可能在后续版本中修改</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="roles">待创建角色列表</param>
        /// <returns>是否创建成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool CreateRoles(UserInfo currentUser, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除特定的角色。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="roleId">待删除权限ID</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool DeleteRole(UserInfo currentUser, int roleId)
        {
            return provider.DeleteRole(roleId);
        }

        /// <summary>
        /// 删除多个角色
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="roleIds">待删除角色的ID列表</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool DeleteRoles(UserInfo currentUser, int[] roleIds)
        {
            //TODO:重写
            List<int> errorList = new List<int>();
            for (int i=0;i<roleIds.Length;i++)
            {
                if (!DeleteRole(currentUser, roleIds[i]))
                {
                    errorList.Add(roleIds[i]);
                }
            }
            if (errorList.Count == 0)
            {
                return true;
            }
            else
            {
                StringBuilder sb = new StringBuilder("以下角色删除失败：");
                for (int i = 0; i < errorList.Count - 1; i++)
                {
                    sb.Append(errorList[i].ToString()).Append("，");
                }
                sb.Append(errorList[errorList.Count - 1].ToString()).Append("。");
                throw new Exception(sb.ToString());
            }
        }

        /// <summary>
        /// 返回指定角色下的用户列表
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>用户列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常</exception>
        public UserCollection GetUsersInRole(UserInfo currentUser, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回所有角色的列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <returns>角色列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常</exception>
        public RoleCollection GetAllRoles(UserInfo currentUser)
        {
            return provider.GetAllRoles();
        }

        /// <summary>
        /// 获得指定角色的信息
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public RoleInfo GetRoleInfo(UserInfo currentUser, int roleId)
        {
            return provider.GetRoleInfo(roleId);
        }

        /// <summary>
        /// 获得指定角色的信息
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="roleName">角色名称</param>
        /// <returns>角色信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看角色信息的权限时抛出该异常</exception>
        public RoleInfo GetRoleInfo(UserInfo currentUser, string roleName)
        {
            return provider.GetRoleInfo(roleName);
        }

        /// <summary>
        /// 返回某一用户所属的所有角色的列表
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="user">用户信息</param>
        /// <returns>角色的集合</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户信息的权限时抛出该异常</exception>
        public RoleCollection GetRolesForUser(UserInfo currentUser, UserInfo user)
        {
            return provider.GetRolesForUser(user);
        }

        /// <summary>
        /// 从某一特定角色中移除某一特定的用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool RemoveUserFromRole(UserInfo currentUser, UserInfo user, RoleInfo role)
        {
            return provider.RemoveUserFromRole(user, role);
        }

        /// <summary>
        /// 从一组角色中移除某一特定的用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool RemoveUserFromRoles(UserInfo currentUser, UserInfo user, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从某一特定角色中移除多个用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="users">用户列表</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool RemoveUsersFromRole(UserInfo currentUser, UserCollection users, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从多个角色中移除多个用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="users">用户列表</param>
        /// <param name="roles">角色列表</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool RemoveUsersFromRoles(UserInfo currentUser, UserCollection users, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新指定用户的角色信息
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="user">用户信息</param>
        /// <returns>是否更新成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑角色的权限时抛出该异常</exception>
        public bool UpdateRolesForUser(UserInfo currentUser, UserInfo user)
        {
            if (user == null || user.Roles==null)
            {
                throw new ArgumentNullException("user");
            }

            if (!PermissionManager.HasPermission(currentUser, "EditRole"))
            {
                throw new InvalidPermissionException("当前用户不具有编辑角色的权限，请与管理员联系！");
            }

            try
            {
                return provider.UpdateRolesForUser(user);
            }
            catch (Exception ex)
            {
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 判断某一特定的角色是否存在
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>是否存在</returns>
        public bool IsRoleExists(string roleName)
        {
            return provider.IsRoleExists(roleName);
        }
    }
}
