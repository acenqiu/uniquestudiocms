//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供角色管理在Sql Server上的实现方法
// 完成日期：2010年03月16日
// 版本：v1.0 alpha
// 作者：邱江毅
//
// 修改记录1：
// 修改日期：2010年03月18日
// 版本号：v1.0 alpha
// 修改人：邱江毅
// 修改内容：+)void AddUsersToRoles(SqlConnection conn, SqlCommand cmd, Guid[] userIds, int[] roleIds);
//=================================================================
using System;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.Permission
{
    /// <summary>
    /// 提供角色管理在Sql Server上的实现方法。
    /// </summary>
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
        private const string IS_ROLE_EXISTS = "IsRoleExists";
        private const string IS_USER_IN_ROLE = "IsUserInRole";
        private const string REMOVE_USER_FROM_ROLE = "RemoveUserFromRole";
        private const string UPDATE_ROLE = "UpdateRole";

        /// <summary>
        /// 初始化<see cref="SqlRoleProvider"/>类的实例。
        /// </summary>
        public SqlRoleProvider()
        {
            //默认构造函数
        }

        /// <summary>
        ///  将某一用户添加到某一角色中。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddUserToRole(Guid userId, int roleId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@UserID",userId),
                                                    new SqlParameter("@RoleID",roleId)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, ADD_USER_TO_ROLE, parms) > 0;
        }

        /// <summary>
        ///  将某一用户添加到某一角色中。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddUserToRole(SqlConnection conn, Guid userId, int roleId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@UserID",userId),
                                                    new SqlParameter("@RoleID",roleId)};
            return SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, ADD_USER_TO_ROLE, parms) > 0;
        }

        /// <summary>
        /// 将多个用户添加到某一角色中。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddUsersToRole(Guid[] userIds, int roleId)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return AddUsersToRole(conn, userIds, roleId);
            }
        }

        /// <summary>
        /// 将多个用户添加到某一角色中。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddUsersToRole(SqlConnection conn, Guid[] userIds, int roleId)
        {
            return AddUsersToRoles(conn, userIds, new int[] { roleId });
        }

        /// <summary>
        ///  将某一用户添加到一组角色中。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddUserToRoles(Guid userId, int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                return AddUserToRoles(conn, userId, roleIds);
            }
        }

        /// <summary>
        ///  将某一用户添加到一组角色中。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddUserToRoles(SqlConnection conn, Guid userId, int[] roleIds)
        {
            return AddUsersToRoles(conn, new Guid[] { userId }, roleIds);
        }

        /// <summary>
        /// 将一组用户添加到一组角色中。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddUsersToRoles(Guid[] userIds, int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return AddUsersToRoles(conn, userIds, roleIds);
            }
        }

        /// <summary>
        /// 将一组用户添加到一组角色中。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddUsersToRoles(SqlConnection conn, Guid[] userIds, int[] roleIds)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    cmd.Transaction = trans;
                    try
                    {
                        AddUsersToRoles(conn, cmd, userIds, roleIds);
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

        /// <summary>
        /// 将一组用户添加到一组角色中。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="cmd">数据库命令。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        public void AddUsersToRoles(SqlConnection conn, SqlCommand cmd, Guid[] userIds, int[] roleIds)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.CommandText = ADD_USER_TO_ROLE;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier);
            cmd.Parameters.Add("@RoleID", SqlDbType.Int);

            foreach (Guid userId in userIds)
            {
                foreach (int roleId in roleIds)
                {
                    cmd.Parameters[0].Value = userId;
                    cmd.Parameters[1].Value = roleId;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 创建新的角色。
        /// </summary>
        /// <param name="role">角色信息。</param>
        /// <returns>如果添加成功，返回角色信息，否则返回空。</returns>
        public RoleInfo CreateRole(RoleInfo role)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(CREATE_ROLE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SiteID", role.SiteId);
                    cmd.Parameters.AddWithValue("@RoleName", role.RoleName);
                    cmd.Parameters.AddWithValue("@Description", role.Description);
                    cmd.Parameters.AddWithValue("@Grade", role.Grade);

                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    //不支持并行事务
                    try
                    {
                        //插入角色
                        object o = cmd.ExecuteScalar();
                        if (o != null && o != DBNull.Value)
                        {
                            role.RoleId = Convert.ToInt32(o);

                            //关联权限
                            if (role.Permissions != null && role.Permissions.Count != 0)
                            {
                                SqlPermissionProvider permissionProvider = new SqlPermissionProvider();
                                int[] permissionIds = new int[role.Permissions.Count];
                                for (int i = 0; i < role.Permissions.Count; i++)
                                {
                                    permissionIds[i] = role.Permissions[i].PermissionId;
                                }
                                if (!permissionProvider.AddPermissionsToRole(conn, permissionIds, role.RoleId))
                                {
                                    DeleteRole(conn, role.RoleId);
                                    return null;
                                }
                            }
                            return role;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch
                    {
                        DeleteRole(conn, role.RoleId);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 创建多个角色。
        /// </summary>
        /// <param name="roles">待创建角色列表。</param>
        /// <returns>是否创建成功。</returns>
        public bool CreateRoles(RoleCollection roles)
        {
            SqlPermissionProvider permissionProvider = new SqlPermissionProvider();
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(CREATE_ROLE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255);
                    cmd.Parameters.Add("@Grade", SqlDbType.Int);

                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    try
                    {
                        foreach (RoleInfo role in roles)
                        {
                            cmd.Parameters[0].Value = role.RoleName;
                            cmd.Parameters[1].Value = role.Description;
                            cmd.Parameters[2].Value = role.Grade;

                            object o = cmd.ExecuteScalar();
                            if (o == null || DBNull.Value == o)
                            {
                                return false;
                            }
                            else
                            {
                                role.RoleId = Convert.ToInt32(o);
                                if (role.Permissions != null && role.Permissions.Count != 0)
                                {
                                    int[] permissionIds = new int[role.Permissions.Count];
                                    for (int i = 0; i < role.Permissions.Count; i++)
                                    {
                                        permissionIds[i] = role.Permissions[i].PermissionId;
                                    }
                                    if (!permissionProvider.AddPermissionsToRole(conn, permissionIds, role.RoleId))
                                    {
                                        DeleteRole(conn, role.RoleId);
                                        return false;
                                    }
                                }
                            }
                        } //end of foreach
                        return true;
                    }
                    catch
                    {
                        //数据库关闭后再执行删除操作
                    }
                }
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

            int[] roleIds = new int[roles.Count];
            for (int i = 0; i < roles.Count; i++)
            {
                roleIds[i] = roles[i].RoleId;
            }
            DeleteRoles(roleIds);
            return false;
        }

        /// <summary>
        /// 删除特定的角色。
        /// </summary>
        /// <param name="roleId">待删除权限ID。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteRole(int roleId)
        {
            SqlParameter parm = new SqlParameter("@RoleID", roleId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_ROLE, parm) > 0;
        }

        /// <summary>
        /// 删除特定的角色。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="roleId">待删除权限ID。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteRole(SqlConnection conn, int roleId)
        {
            SqlParameter parm = new SqlParameter("@RoleID", roleId);
            return SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, DELETE_ROLE, parm) > 0;
        }

        /// <summary>
        /// 删除多个角色。
        /// </summary>
        /// <param name="roleIds">待删除角色的ID列表。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteRoles(int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_ROLE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RoleID", SqlDbType.Int);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (int roleId in roleIds)
                            {
                                cmd.Parameters[0].Value = roleId;
                                cmd.ExecuteNonQuery();
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

        /// <summary>
        /// 返回指定角色下的用户列表。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>用户列表。</returns>
        public UserCollection GetUsersInRole(int roleId)
        {
            SqlParameter parm = new SqlParameter("@RoleID", roleId);
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
        /// 返回指定角色下的用户列表。
        /// </summary>
        /// <remarks>该方法执行完后没有关闭数据库。</remarks>
        /// <param name="conn">数据库连接。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>用户列表。</returns>
        public UserCollection GetUsersInRole(SqlConnection conn, int roleId)
        {
            UserCollection collection = new UserCollection();

            SqlCommand cmd = new SqlCommand(GET_USERS_IN_ROLE, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RoleID", roleId);

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            using (SqlDataReader reader = cmd.ExecuteReader())
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
        /// 返回所有角色的列表。
        /// </summary>
        /// <returns>角色列表。</returns>
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

        /// <summary>
        /// 获得指定角色的信息。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>角色信息。</returns>
        public RoleInfo GetRoleInfo(int roleId)
        {
            SqlParameter parm = new SqlParameter("@RoleID", roleId);
            return GetRoleInfo(parm, GET_ROLE_BY_ID);
        }

        /// <summary>
        /// 获得指定角色的信息。
        /// </summary>
        /// <param name="roleName">角色名称。</param>
        /// <returns>角色信息。</returns>
        public RoleInfo GetRoleInfo(string roleName)
        {
            SqlParameter parm = new SqlParameter("@RoleName", roleName);
            return GetRoleInfo(parm, GET_ROLE_BY_NAME);
        }

        private RoleInfo GetRoleInfo(SqlParameter parm, string cmdText)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(parm);
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    RoleInfo role = null;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            role = FillRoleInfo(reader);
                        }
                        reader.Close();
                    }
                    if (role != null)
                    {
                        role.Permissions = (new SqlPermissionProvider()).GetPermissionsForRole(conn, role.RoleId);
                        role.Users = GetUsersInRole(conn, role.RoleId);
                    }
                    return role;
                }
            }
        }

        /// <summary>
        /// 返回某一用户所属的所有角色的列表。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <returns>角色的集合。</returns>
        public RoleCollection GetRolesForUser(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
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

        /// <summary>
        /// 返回某一用户所属的所有角色的列表。
        /// </summary>
        /// <remarks>该方法执行完后没有关闭数据库。</remarks>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userId">用户ID。</param>
        /// <returns>角色的集合。</returns>
        public RoleCollection GetRolesForUser(SqlConnection conn, Guid userId)
        {
            RoleCollection collection = new RoleCollection();

            SqlCommand cmd = new SqlCommand(GET_ROLES_FOR_USER, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userId);

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    collection.Add(FillRoleInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 判断特定的用户是否是特定角色中的成员。
        /// </summary>
        /// <remarks>该方法应该被废弃！</remarks>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleName">角色名称。</param>
        /// <returns>是否是特定角色的成员。</returns>
        public bool IsUserInRole(Guid userId, string roleName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@UserID",userId),
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

        /// <summary>
        /// 从某一特定角色中移除某一特定的用户。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveUserFromRole(Guid userId, int roleId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@UserID",userId),
                                                            new SqlParameter("@RoleId",roleId)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, REMOVE_USER_FROM_ROLE, parms) > 0;
        }

        /// <summary>
        /// 从某一特定角色中移除某一特定的用户。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveUserFromRole(SqlConnection conn, Guid userId, int roleId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@UserID",userId),
                                                            new SqlParameter("@RoleId",roleId)};
            return SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, REMOVE_USER_FROM_ROLE, parms) > 0;
        }

        /// <summary>
        /// 从一组角色中移除某一特定的用户。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveUserFromRoles(Guid userId, int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return RemoveUserFromRoles(conn, userId, roleIds);
            }
        }

        /// <summary>
        /// 从一组角色中移除某一特定的用户。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userId">用户ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveUserFromRoles(SqlConnection conn, Guid userId, int[] roleIds)
        {
            return RemoveUsersFromRoles(conn, new Guid[] { userId }, roleIds);
        }

        /// <summary>
        /// 从某一特定角色中移除多个用户。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveUsersFromRole(Guid[] userIds, int roleId)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return RemoveUsersFromRole(conn, userIds, roleId);
            }
        }

        /// <summary>
        /// 从某一特定角色中移除多个用户。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveUsersFromRole(SqlConnection conn, Guid[] userIds, int roleId)
        {
            return RemoveUsersFromRoles(conn, userIds, new int[] { roleId });
        }

        /// <summary>
        /// 从多个角色中移除多个用户。
        /// </summary>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveUsersFromRoles(Guid[] userIds, int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return RemoveUsersFromRoles(conn, userIds, roleIds);
            }
        }

        /// <summary>
        /// 从多个角色中移除多个用户。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userIds">用户ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveUsersFromRoles(SqlConnection conn, Guid[] userIds, int[] roleIds)
        {
            using (SqlCommand cmd = new SqlCommand(REMOVE_USER_FROM_ROLE, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier);
                cmd.Parameters.Add("@RoleID", SqlDbType.Int);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    cmd.Transaction = trans;
                    try
                    {
                        foreach (Guid userId in userIds)
                        {
                            foreach (int roleId in roleIds)
                            {
                                cmd.Parameters[0].Value = userId;
                                cmd.Parameters[1].Value = roleId;
                                cmd.ExecuteNonQuery();
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

        /// <summary>
        /// 更新角色信息。
        /// </summary>
        /// <remarks>不更新角色下的用户列表。</remarks>
        /// <param name="role">待更新角色信息。</param>
        /// <returns>是否更新成功。</returns>
        public bool UpdateRole(RoleInfo role)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(UPDATE_ROLE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleID", role.RoleId);
                    cmd.Parameters.AddWithValue("@SiteID", role.SiteId);
                    cmd.Parameters.AddWithValue("@RoleName", role.RoleName);
                    cmd.Parameters.AddWithValue("@Description", role.Description);
                    cmd.Parameters.AddWithValue("@Grade", role.Grade);

                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                //更新权限信息
                                (new SqlPermissionProvider()).UpdatePermissionsForRole(conn, cmd, role);
                                trans.Commit();
                                return true;
                            }
                            else
                            {
                                trans.Rollback();
                                return false;
                            }
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

        /// <summary>
        /// 更新指定用户的角色信息。
        /// </summary>
        /// <param name="user">用户信息。</param>
        /// <returns>是否更新成功。</returns>
        public bool UpdateRolesForUser(UserInfo user)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", user.UserId);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            cmd.CommandText = DELETE_ALL_ROLES_FOR_USER;
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = ADD_USER_TO_ROLE;
                            cmd.Parameters.Add("@RoleID", SqlDbType.Int);

                            foreach (RoleInfo role in user.Roles)
                            {
                                cmd.Parameters[1].Value = role.RoleId;
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

        /// <summary>
        /// 判断某一特定的角色是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="roleName">角色名称。</param>
        /// <returns>是否存在。</returns>
        public bool IsRoleExists(int siteId, string roleName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@SiteID",siteId),
                                                            new SqlParameter("@RoleName", roleName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_ROLE_EXISTS, parms);
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
            role.RoleId = (int)reader["ID"];
            role.SiteId = (int)reader["SiteID"];
            role.RoleName = reader["RoleName"].ToString();
            if (DBNull.Value != reader["Description"])
            {
                role.Description = reader["Description"].ToString();
            }
            role.Grade = (int)reader["Grade"];
            if (DBNull.Value != reader["SiteName"])
            {
                role.SiteName = reader["SiteName"].ToString();
            }
            else
            {
                role.SiteName = string.Empty;
            }
            return role;
        }
    }
}
