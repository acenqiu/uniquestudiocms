using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface IPermission
    {
        bool HasPermission(UserInfo user, string permissionName);

        bool AddPermissionsToRoles(PermissionCollection permissions, RoleCollection roles);

        bool AddPermissionToRoles(PermissionInfo permission, RoleCollection roles);

        bool AddPermissionsToRole(PermissionCollection permissions, RoleInfo role);

        bool AddPermissionToRole(PermissionInfo permission, RoleInfo role);

        PermissionInfo CreatePermission(PermissionInfo permission);

        bool CreatePermissions(PermissionCollection permissions);

        bool DeletePermission(int permissionId);

        PermissionCollection GetAllPermissions();

        PermissionCollection GetPermissionsForRole(RoleInfo role);

        PermissionCollection GetPermissionsForUser(UserInfo user);

        PermissionInfo GetPermissionInfo(int permissionId);

        PermissionInfo GetPermissionInfo(string permissionName);

        bool RemovePermissionFromRole(PermissionInfo permission, RoleInfo role);

        bool RemovePermissionsFromRole(PermissionCollection permissions, RoleInfo role);

        bool RemovePermissionFromRoles(PermissionInfo permission, RoleCollection roles);

        bool RemovePermissionsFromRoles(PermissionCollection permissions, RoleCollection roles);

        bool IsPermissionExists(string permissionName);
    }
}
