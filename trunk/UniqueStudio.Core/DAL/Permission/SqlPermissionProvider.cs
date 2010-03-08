using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.Permission
{
    internal class SqlPermissionProvider : IPermission
    {
        private const string ADD_PERMISSION_TO_ROLE = "AddPermissionToRole";
        private const string CREATE_PERMISSION = "CreatePermission";
        private const string DELETE_PERMISSION = "DeletePermission";
        private const string GET_ALL_PERMISSIONS = "GetAllPermissions";
        private const string GET_PERMISSIONS_FOR_ROLE = "GetPermissionsForRole";
        private const string GET_PERMISSIONS_FOR_USER = "GetPermissionsForUser";
        private const string GET_PERMISSION_BY_ID = "GetPermissionById";
        private const string GET_PERMISSION_BY_NAME = "GetPermissionByName";
        private const string HAS_PERMISSION = "HasPermission";
        private const string IS_PERMISSION_IN_ROLE = "IsPermissionInRole";
        private const string REMOVE_PERMISSION_FROM_ROLE = "RemovePermissionFromRole";
        private const string IS_PERMISSION_EXISTS = "IsPermissionExists";

        public SqlPermissionProvider()
        {
            //默认构造函数
        }

        public bool HasPermission(UserInfo user, string permissionName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@UserID",user.UserId),
                                                    new SqlParameter("@PermissionName",permissionName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, HAS_PERMISSION, parms);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToInt32(o) >= 1;
            }
            else
            {
                throw new Exception();
            }
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
            bool succeed = true;
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PermissionName",DbType.String),
                                                    new SqlParameter("@RoleName",DbType.String)};
                foreach (PermissionInfo permission in permissions)
                {
                    parms[0].Value = permission.PermissionName;
                    parms[1].Value = role.RoleName;

                    if (SqlHelper.ExecuteNonQuery(conn,CommandType.StoredProcedure,ADD_PERMISSION_TO_ROLE,parms)<=0)
                    {
                        succeed = false;
                        //回滚
                        break;
                    }
                }
            }
            return succeed;
        }

        public bool AddPermissionToRole(PermissionInfo permission, RoleInfo role)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PermissionName",permission.PermissionName),
                                                    new SqlParameter("@RoleName",role.RoleName)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, ADD_PERMISSION_TO_ROLE, parms) > 0;
        }

        public PermissionInfo CreatePermission(PermissionInfo permission)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@PermissionName",permission.PermissionName),
                                                        new SqlParameter("@Description",permission.Description),
                                                        new SqlParameter("@Provider",permission.Provider)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_PERMISSION, parms);
            if (o != null && o != DBNull.Value)
            {
                permission.Id = Convert.ToInt32(o);
                return permission;
            }
            else
            {
                return null;
            }
        }

        public bool CreatePermissions(PermissionCollection permissions)
        {
            bool succeed = true;
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@PermissionName",DbType.String),
                                                        new SqlParameter("@Description",DbType.String),
                                                        new SqlParameter("@Provider",DbType.String)};
                foreach (PermissionInfo permission in permissions)
                {
                    parms[0].Value = permission.PermissionName;
                    parms[1].Value = permission.Description;
                    parms[2].Value = permission.Provider;

                    object o = SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, CREATE_PERMISSION, parms);
                    if (o == null || DBNull.Value == o)
                    {
                        succeed = false;
                        break;
                    }
                }
            }
            return succeed;
        }

        public bool DeletePermission(int permissionId)
        {
            SqlParameter parm = new SqlParameter("@ID", permissionId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_PERMISSION, parm) > 0;
        }

        public PermissionCollection GetAllPermissions()
        {
            PermissionCollection collection = new PermissionCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_PERMISSIONS, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillPermissionInfo(reader));
                }
            }
            return collection;
        }

        public PermissionCollection GetPermissionsForRole(RoleInfo role)
        {
            SqlParameter parm = new SqlParameter("@RoleName", role.RoleName);
            PermissionCollection collection = new PermissionCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_PERMISSIONS_FOR_ROLE, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillPermissionInfo(reader));
                }
            }
            return collection;
        }

        public PermissionCollection GetPermissionsForUser(UserInfo user)
        {
            SqlParameter parm = new SqlParameter("@UserID", user.UserId);
            PermissionCollection collection = new PermissionCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_PERMISSIONS_FOR_USER, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillPermissionInfo(reader));
                }
            }
            return collection;
        }

        public PermissionInfo GetPermissionInfo(int permissionId)
        {
            SqlParameter parm = new SqlParameter("@PermissionID", permissionId);
            return GetPermissionInfo(parm, GET_PERMISSION_BY_ID);
        }

        public PermissionInfo GetPermissionInfo(string permissionName)
        {
            SqlParameter parm = new SqlParameter("@PermissionName", permissionName);
            return GetPermissionInfo(parm, GET_PERMISSION_BY_NAME);
        }
            
        private PermissionInfo GetPermissionInfo(SqlParameter parm, string cmdText)
        {
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, cmdText, parm))
            {
                if (reader.Read())
                {
                    return FillPermissionInfo(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public bool RemovePermissionFromRole(PermissionInfo permission, RoleInfo role)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@PermissionName",permission.PermissionName),
                                                            new SqlParameter("@RoleName",role.RoleName)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, REMOVE_PERMISSION_FROM_ROLE, parms) > 0;
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
            SqlParameter parm = new SqlParameter("@PermissionName", permissionName);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_PERMISSION_EXISTS, parm);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean((int)o);
            }
            else
            {
                throw new Exception();
            }
        }

        private PermissionInfo FillPermissionInfo(SqlDataReader reader)
        {
            PermissionInfo permission = new PermissionInfo();
            permission.Id = (int)reader["ID"];
            permission.PermissionName = reader["PermissionName"].ToString();
            if (DBNull.Value != reader["Description"])
            {
                permission.Description = reader["Description"].ToString();
            }
            if (DBNull.Value != reader["Provider"])
            {
                permission.Provider = reader["Provider"].ToString();
            }
            return permission;
        }
    }
}
