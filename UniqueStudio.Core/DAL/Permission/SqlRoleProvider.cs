using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.DAL.IDAL;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.DatabaseHelper;

namespace UniqueStudio.DAL.Permission
{
    internal class SqlRoleProvider : IRole
    {
        private const string ADD_USER_TO_ROLE = "AddUserToRole";
        private const string CREATE_ROLE = "CreateRole";
        private const string DELETE_ROLE = "DeleteRole";
        private const string DELETE_ALL_ROLES_FOR_USER = "DeleteAllRolesForUser";
        private const string GET_ALL_ROLES = "GetAllRoles";
        private const string GET_ROLE_BY_ID = "GetRoleByID";
        private const string GET_ROLE_BY_NAME = "GetRoleByName";
        private const string GET_ROLES_FOR_USER = "GetRolesForUser";
        private const string GET_USERS_IN_ROLE = "GetUsersInRole";
        private const string IS_USER_IN_ROLE = "IsUserInRole";
        private const string REMOVE_USER_FROM_ROLE = "RemoveUserFromRole";
        private const string IS_ROLE_EXISTS = "IsRoleExists";

        public SqlRoleProvider()
        {
        }

        public bool AddUsersToRole(UserCollection users, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        public bool AddUsersToRoles(UserCollection users, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        public bool AddUserToRole(UserInfo user, RoleInfo role)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@UserID",user.UserId),
                                                    new SqlParameter("@RoleName",role.RoleName)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, ADD_USER_TO_ROLE, parms) > 0;
        }

        public bool AddUserToRoles(UserInfo user, RoleCollection roles)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(ADD_USER_TO_ROLE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", user.UserId);
                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (RoleInfo role in roles)
                            {
                                cmd.Parameters[1].Value = role.RoleName;
                                if (cmd.ExecuteNonQuery() == 0)
                                {
                                    trans.Rollback();
                                    return false;
                                }
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
        }

        public RoleInfo CreateRole(RoleInfo role)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@RoleName",role.RoleName),
                                                        new SqlParameter("@Description",role.Description),
                                                        new SqlParameter("@Grade",role.Grade)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_ROLE, parms);
            if (o != null && o != DBNull.Value)
            {
                role.Id = Convert.ToInt32(o);

                if (role.Permissions != null)
                {
                    SqlPermissionProvider permissionProvider = new SqlPermissionProvider();
                    if (!permissionProvider.AddPermissionsToRole(role.Permissions, role))
                    {
                        //回滚
                        throw new NotImplementedException();
                    }
                }

                return role;
            }
            else
            {
                return null;
            }
        }

        public bool CreateRoles(RoleCollection roles)
        {
            bool succeed = true;
            SqlPermissionProvider permissionProvider = new SqlPermissionProvider();
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@RoleName",DbType.String),
                                                        new SqlParameter("@Description",DbType.String),
                                                        new SqlParameter("@Grade",DbType.Int32)};
                foreach (RoleInfo role in roles)
                {
                    parms[0].Value = role.RoleName;
                    parms[1].Value = role.Description;
                    parms[2].Value = role.Grade;

                    object o = SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, CREATE_ROLE, parms);
                    if (o == null || DBNull.Value == o)
                    {
                        succeed = false;
                        //回滚
                        break;
                    }
                    else
                    {
                        if (role.Permissions != null)
                        {
                            if (!permissionProvider.AddPermissionsToRole(role.Permissions, role))
                            {
                                succeed = false;
                                //回滚
                                break;
                            }
                        }
                    }
                }
            }
            return succeed;
        }

        public bool DeleteRole(int roleId)
        {
            SqlParameter parm = new SqlParameter("@ID", roleId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_ROLE, parm) > 0;
        }

        public UserCollection GetUsersInRole(RoleInfo role)
        {
            SqlParameter parm = new SqlParameter("@RoleName", role.RoleName);
            UserCollection collection = new UserCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_USERS_IN_ROLE, parm))
            {
                while (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.UserId = new Guid(reader["UserID"].ToString());
                    user.Email = reader["Email"].ToString();
                    user.UserName = reader["UserName"].ToString();

                    collection.Add(user);
                }
            }
            return collection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RoleCollection GetAllRoles()
        {
            RoleCollection collection = new RoleCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_ROLES, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillRoleInfo(reader));
                }
            }
            return collection;
        }

        public RoleInfo GetRoleInfo(int roleId)
        {
            SqlParameter parm = new SqlParameter("@RoleID", roleId);
            return GetRoleInfo(parm, GET_ROLE_BY_ID);
        }

        public RoleInfo GetRoleInfo(string roleName)
        {
            SqlParameter parm = new SqlParameter("@RoleName", roleName);
            return GetRoleInfo(parm, GET_ROLE_BY_NAME);
        }

        private RoleInfo GetRoleInfo(SqlParameter parm, string cmdText)
        {
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, cmdText, parm))
            {
                if (reader.Read())
                {
                    RoleInfo role = FillRoleInfo(reader);

                    IPermission permissionProvider = DALFactory.CreatePermission();
                    role.Permissions = permissionProvider.GetPermissionsForRole(role);

                    role.Users = GetUsersInRole(role);
                    return role;
                }
                else
                {
                    return null;
                }
            }
        }

        public RoleCollection GetRolesForUser(UserInfo user)
        {
            SqlParameter parm = new SqlParameter("@UserID", user.UserId);
            RoleCollection collection = new RoleCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ROLES_FOR_USER, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillRoleInfo(reader));
                }
            }
            return collection;
        }

        public bool IsUserInRole(UserInfo user, string roleName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@UserID",user.UserId),
                                                            new SqlParameter("@RoleName",roleName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_USER_IN_ROLE, parms);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean((int)o);
            }
            else
            {
                throw new Exception();
            }
        }

        public bool RemoveUserFromRole(UserInfo user, RoleInfo role)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@UserID",user.UserId),
                                                            new SqlParameter("@RoleName",role.RoleName)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, REMOVE_USER_FROM_ROLE, parms) > 0;
        }

        public bool RemoveUserFromRoles(UserInfo user, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUsersFromRole(UserCollection users, RoleInfo role)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUsersFromRoles(UserCollection users, RoleCollection roles)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRolesForUser(UserInfo user)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_ALL_ROLES_FOR_USER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", user.UserId);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = ADD_USER_TO_ROLE;
                            cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50);

                            foreach (RoleInfo role in user.Roles)
                            {
                                cmd.Parameters[1].Value = role.RoleName;
                                if (cmd.ExecuteNonQuery() == 0)
                                {
                                    trans.Rollback();
                                    return false;
                                }
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
        }

        public bool IsRoleExists(string roleName)
        {
            SqlParameter parm = new SqlParameter("@RoleName", roleName);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_ROLE_EXISTS, parm);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean((int)o);
            }
            else
            {
                throw new Exception();
            }
        }

        private RoleInfo FillRoleInfo(SqlDataReader reader)
        {
            RoleInfo role = new RoleInfo();
            role.Id = (int)reader["ID"];
            role.RoleName = reader["RoleName"].ToString();
            if (DBNull.Value != reader["Description"])
            {
                role.Description = reader["Description"].ToString();
            }
            role.Grade = (int)reader["Grade"];
            return role;
        }
    }
}
