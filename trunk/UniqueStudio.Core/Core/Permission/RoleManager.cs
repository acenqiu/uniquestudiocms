using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.DAL.Permission;

namespace UniqueStudio.Core.Permission
{
    /// <summary>
    /// 提供对角色进行管理的方法。
    /// </summary>
    public class RoleManager
    {
        private static readonly IRole provider = DALFactory.CreateRole();

        /// <summary>
        /// 判断某一特定的用户是否是某一特定角色中的成员。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool IsUserInRole(UserInfo user, string roleName)
        {
            return provider.IsUserInRole(user, roleName);
        }

        /// <summary>
        /// 初始化RoleManager类的实例。
        /// </summary>
        public RoleManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 将一组用户添加到某一角色中。
        /// </summary>
        /// <param name="users"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool AddUsersToRole(UserCollection users, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将一组用户添加到一组角色中。 
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool AddUsersToRoles(UserCollection users, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  将某一用户添加到某一角色中。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool AddUserToRole(UserInfo user, RoleInfo role)
        {
            return provider.AddUserToRole(user, role);
        }

        /// <summary>
        ///  将某一用户添加到一组角色中。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool AddUserToRoles(UserInfo user, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建新的角色。
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public RoleInfo CreateRole(RoleInfo role)
        {
            return provider.CreateRole(role);
        }

        /// <summary>
        /// 添加一组角色
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public RoleCollection CreateRoles(RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除特定的角色。
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool DeleteRole(int roleId)
        {
            return provider.DeleteRole(roleId);
        }

        public bool DeleteRoles(int[] roleIds)
        {
            List<int> errorList = new List<int>();
            for (int i=0;i<roleIds.Length;i++)
            {
                if (!DeleteRole(roleIds[i]))
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
        ///
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public UserCollection FindUsersInRole(RoleInfo role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回所有角色的列表。
        /// </summary>
        /// <returns></returns>
        public RoleCollection GetAllRoles()
        {
            return provider.GetAllRoles();
        }

        public RoleInfo GetRoleInfo(int roleId)
        {
            return provider.GetRoleInfo(roleId);
        }

        public RoleInfo GetRoleInfo(string roleName)
        {
            return provider.GetRoleInfo(roleName);
        }

        /// <summary>
        /// 返回某一用户所属的所有角色的列表。
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public RoleCollection GetRolesForUser(UserInfo user)
        {
            return provider.GetRolesForUser(user);
        }

        /// <summary>
        /// 从某一特定角色中移除某一特定的成员。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool RemoveUserFromRole(UserInfo user, RoleInfo role)
        {
            return provider.RemoveUserFromRole(user, role);
        }

        /// <summary>
        /// 从一组角色中移除某一特定的成员。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool RemoveUserFromRoles(UserInfo user, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从某一特定角色中移除一组成员。
        /// </summary>
        /// <param name="users"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool RemoveUsersFromRole(UserCollection users, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从一组角色中移除一组成员。
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool RemoveUsersFromRoles(UserCollection users, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRolesForUser(UserInfo currentUser, UserInfo user)
        {
            if (user == null || user.Roles==null)
            {
                throw new ArgumentNullException("user");
            }

            if (!PermissionManager.HasPermission(currentUser, "EditUserRole"))
            {
                throw new InvalidPermissionException("当前用户不具有编辑用户权限的权限，请与管理员联系！");
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
        /// 判断某一特定的角色是否存在。
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool IsRoleExists(string roleName)
        {
            return provider.IsRoleExists(roleName);
        }
    }
}
