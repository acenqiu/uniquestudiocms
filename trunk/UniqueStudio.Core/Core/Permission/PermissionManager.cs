using System;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.Permission
{
    /// <summary>
    /// 提供权限管理的方法
    /// </summary>
    public class PermissionManager
    {
        private static readonly IPermission provider = DALFactory.CreatePermission();

        public PermissionManager()
        {
            //默认构造函数
        }

        public static bool HasPermission(UserInfo user, string permissionName)
        {
            if (!GlobalConfig.EnablePermissionCheck)
            {
                return true;
            }

            if (user == null || string.IsNullOrEmpty(permissionName))
            {
                return false;
            }
            if (user.UserId == new Guid())
            {
                return false;
            }


            //用户登录状态确认
            //if (!UserManager.IsUserOnline(user))
            //{
            //    //最好能区分是没有登录还是没有权限。。
            //    return false;
            //}

            //权限检验
            return provider.HasPermission(user, permissionName);
        }

        public bool AddPermissionsToRoles(PermissionCollection permissions, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        public bool AddPermissionToRoles(PermissionInfo permission, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        public bool AddPermissionsToRole(PermissionCollection permissions, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        public bool AddPermissionToRole(PermissionInfo permission, RoleInfo role)
        {
            return provider.AddPermissionToRole(permission, role);
        }

        public PermissionInfo CreatePermission(PermissionInfo permission)
        {
            return provider.CreatePermission(permission);
        }

        public bool CreatePermissions(PermissionCollection permissions)
        {
            throw new NotImplementedException();
        }

        public bool DeletePermission(int permissionId)
        {
            return provider.DeletePermission(permissionId);
        }

        //暂时不确定是否要控制查看权限的权限
        public PermissionCollection GetAllPermissions()
        {
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

        public PermissionCollection GetAllPermissions(UserInfo currentUser)
        {
            if (!HasPermission(currentUser, "ViewPermissionInfo"))
            {
                throw new InvalidPermissionException("当前用户没有浏览权限列表的权限，请与管理员联系！");
            }

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

        public PermissionCollection GetPermissionsForRole(RoleInfo role)
        {
            return provider.GetPermissionsForRole(role);
        }

        public PermissionInfo GetPermissionInfo(int permissionId)
        {
            return provider.GetPermissionInfo(permissionId);
        }

        public PermissionInfo GetPermissionInfo(string permissionName)
        {
            return provider.GetPermissionInfo(permissionName);
        }
        
        public bool RemovePermissionFromRole(PermissionInfo permission, RoleInfo role)
        {
            return provider.RemovePermissionFromRole(permission, role);
        }

        public bool RemovePermissionsFromRole(PermissionCollection permissions, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        public bool RemovePermissionFromRoles(PermissionInfo permission, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        public bool RemovePermissionsFromRoles(PermissionCollection permissions, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        public bool IsPermissionExists(string permissionName)
        {
            return provider.IsPermissionExists(permissionName);
        }
    }
}
