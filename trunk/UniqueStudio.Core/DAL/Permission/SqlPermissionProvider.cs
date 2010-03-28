//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供权限管理在Sql Server上的实现方法。
// 完成日期：2010年03月16日
// 版本：v1.0 alpha
// 作者：邱江毅
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
    /// 提供权限管理在Sql Server上的实现方法。
    /// </summary>
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
        private const string IS_PERMISSION_EXISTS = "IsPermissionExists";
        private const string REMOVE_ALL_PERMISSIONS_FOR_ROLE = "RemoveAllPermissionsForRole";
        private const string REMOVE_PERMISSION_FROM_ROLE = "RemovePermissionFromRole";


        /// <summary>
        /// 初始化<see cref="SqlPermissionProvider"/>类的实例。
        /// </summary>
        public SqlPermissionProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 返回指定用户是否拥有特定权限。
        /// </summary>
        /// <remarks>当前版本需提供用户ID，在后续版本中还要求提供用户SessionID。</remarks>
        /// <param name="user">用户信息。</param>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>是否拥有指定权限。</returns>
        public bool HasPermission(UserInfo user, int siteId, string permissionName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@UserID",user.UserId),
                                                    new SqlParameter("@SiteID",siteId),
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

        /// <summary>
        /// 添加一个权限到一个角色。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddPermissionToRole(int permissionId, int roleId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PermissionID",permissionId),
                                                    new SqlParameter("@RoleID",roleId)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, ADD_PERMISSION_TO_ROLE, parms) > 0;
        }

        /// <summary>
        /// 添加一个权限到一个角色。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddPermissionToRole(SqlConnection conn, int permissionId, int roleId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PermissionID",permissionId),
                                                    new SqlParameter("@RoleID",roleId)};
            return SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, ADD_PERMISSION_TO_ROLE, parms) > 0;
        }

        /// <summary>
        /// 添加一个权限到多个角色。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddPermissionToRoles(int permissionId, int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return AddPermissionToRoles(conn, permissionId, roleIds);
            }
        }

        /// <summary>
        /// 添加一个权限到多个角色。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddPermissionToRoles(SqlConnection conn, int permissionId, int[] roleIds)
        {
            return AddPermissionsToRoles(conn, new int[] { permissionId }, roleIds);
        }

        /// <summary>
        /// 添加多个权限到一个角色。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddPermissionsToRole(int[] permissionIds, int roleId)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return AddPermissionsToRole(conn, permissionIds, roleId);
            }
        }

        /// <summary>
        /// 添加多个权限到一个角色。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddPermissionsToRole(SqlConnection conn, int[] permissionIds, int roleId)
        {
            return AddPermissionsToRoles(conn, permissionIds, new int[] { roleId });
        }

        /// <summary>
        /// 添加多个权限到多个角色。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddPermissionsToRoles(int[] permissionIds, int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return AddPermissionsToRoles(conn, permissionIds, roleIds);
            }
        }

        /// <summary>
        /// 添加多个权限到多个角色。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否添加成功。</returns>
        public bool AddPermissionsToRoles(SqlConnection conn, int[] permissionIds, int[] roleIds)
        {
            using (SqlCommand cmd = new SqlCommand(ADD_PERMISSION_TO_ROLE, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PermissionID", SqlDbType.Int);
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
                        foreach (int permissionId in permissionIds)
                        {
                            foreach (int roleId in roleIds)
                            {
                                cmd.Parameters[0].Value = permissionId;
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
        /// 创建权限。
        /// </summary>
        /// <param name="permission">权限信息。</param>
        /// <returns>如果创建成功，返回权限信息，否则返回空。</returns>
        public PermissionInfo CreatePermission(PermissionInfo permission)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@PermissionName",permission.PermissionName),
                                                        new SqlParameter("@Description",permission.Description),
                                                        new SqlParameter("@Provider",permission.Provider)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_PERMISSION, parms);
            if (o != null && o != DBNull.Value)
            {
                permission.PermissionId = Convert.ToInt32(o);
                return permission;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 创建多个权限。
        /// </summary>
        /// <param name="permissions">待创建权限列表。</param>
        /// <returns>是否创建成功。</returns>
        public bool CreatePermissions(PermissionCollection permissions)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(CREATE_PERMISSION, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PermissionName", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255);
                    cmd.Parameters.Add("@Provider", SqlDbType.NVarChar, 50);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (PermissionInfo permission in permissions)
                            {
                                cmd.Parameters[0].Value = permission.PermissionName;
                                cmd.Parameters[1].Value = permission.Description;
                                cmd.Parameters[2].Value = permission.Provider;

                                object o = cmd.ExecuteScalar();
                                if (o == null || DBNull.Value == o)
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
        /// 删除一个权限。
        /// </summary>
        /// <param name="permissionId">待删除权限ID。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeletePermission(int permissionId)
        {
            SqlParameter parm = new SqlParameter("@ID", permissionId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_PERMISSION, parm) > 0;
        }

        /// <summary>
        /// 删除多个权限。
        /// </summary>
        /// <param name="permissionIds">待删除权限的ID列表</param>
        /// <returns>是否删除成功。</returns>
        public bool DeletePermissions(int[] permissionIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_PERMISSION, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (int permissionId in permissionIds)
                            {
                                cmd.Parameters[0].Value = permissionId;
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
        /// 返回权限列表。
        /// </summary>
        /// <returns>权限列表。</returns>
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

        /// <summary>
        /// 返回指定角色所具有的权限列表。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>权限列表。</returns>
        public PermissionCollection GetPermissionsForRole(int roleId)
        {
            PermissionCollection collection = new PermissionCollection();
            SqlParameter parm = new SqlParameter("@RoleID", roleId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_PERMISSIONS_FOR_ROLE, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillPermissionInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回指定角色所具有的权限列表。
        /// </summary>
        /// <remarks>该方法执行完后没有关闭数据库。</remarks>
        /// <param name="conn">数据库连接。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>权限列表。</returns>
        public PermissionCollection GetPermissionsForRole(SqlConnection conn, int roleId)
        {
            PermissionCollection collection = new PermissionCollection();

            SqlCommand cmd = new SqlCommand(GET_PERMISSIONS_FOR_ROLE, conn);
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
                    collection.Add(FillPermissionInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回指定用户所具有的权限列表。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <returns>权限列表。</returns>
        public PermissionCollection GetPermissionsForUser(Guid userId)
        {
            PermissionCollection collection = new PermissionCollection();
            SqlParameter parm = new SqlParameter("@UserID", userId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_PERMISSIONS_FOR_USER, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillPermissionInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回指定用户所具有的权限列表。
        /// </summary>
        /// <remarks>该方法执行完后没有关闭数据库。</remarks>
        /// <param name="conn">数据库连接。</param>
        /// <param name="userId">用户ID。</param>
        /// <returns>权限列表。</returns>
        public PermissionCollection GetPermissionsForUser(SqlConnection conn, Guid userId)
        {
            PermissionCollection collection = new PermissionCollection();

            SqlCommand cmd = new SqlCommand(GET_PERMISSIONS_FOR_USER, conn);
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
                    collection.Add(FillPermissionInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回指定权限信息。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <returns>权限信息。</returns>
        public PermissionInfo GetPermissionInfo(int permissionId)
        {
            SqlParameter parm = new SqlParameter("@PermissionID", permissionId);
            return GetPermissionInfo(parm, GET_PERMISSION_BY_ID);
        }

        /// <summary>
        /// 返回指定权限信息。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>权限信息。</returns>
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

        /// <summary>
        /// 移除指定角色的所有权限。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveAllPermissionsForRole(int roleId)
        {
            SqlParameter parm = new SqlParameter("@RoleID", roleId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, REMOVE_ALL_PERMISSIONS_FOR_ROLE, parm) > 0;
        }

        /// <summary>
        /// 移除指定角色的所有权限。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemoveAllPermissionsForRole(SqlConnection conn, int roleId)
        {
            SqlParameter parm = new SqlParameter("@RoleID", roleId);
            return SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, REMOVE_ALL_PERMISSIONS_FOR_ROLE, parm) > 0;
        }

        /// <summary>
        /// 从指定角色中移除特定权限。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemovePermissionFromRole(int permissionId, int roleId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@PermissionID",permissionId),
                                                            new SqlParameter("@RoleID",roleId)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, REMOVE_PERMISSION_FROM_ROLE, parms) > 0;
        }

        /// <summary>
        /// 从指定角色中移除特定权限。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemovePermissionFromRole(SqlConnection conn, int permissionId, int roleId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@PermissionID",permissionId),
                                                            new SqlParameter("@RoleID",roleId)};
            return SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, REMOVE_PERMISSION_FROM_ROLE, parms) > 0;
        }

        /// <summary>
        /// 从指定角色中移除多个权限。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemovePermissionsFromRole(int[] permissionIds, int roleId)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return RemovePermissionsFromRole(conn, permissionIds, roleId);
            }
        }

        /// <summary>
        /// 从指定角色中移除多个权限。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleId">角色ID。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemovePermissionsFromRole(SqlConnection conn, int[] permissionIds, int roleId)
        {
            return RemovePermissionsFromRoles(conn, permissionIds, new int[] { roleId });
        }

        /// <summary>
        /// 从多个角色中移除特定权限。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemovePermissionFromRoles(int permissionId, int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return RemovePermissionFromRoles(conn, permissionId, roleIds);
            }
        }

        /// <summary>
        /// 从多个角色中移除特定权限。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="permissionId">权限ID。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemovePermissionFromRoles(SqlConnection conn, int permissionId, int[] roleIds)
        {
            return RemovePermissionsFromRoles(conn, new int[] { permissionId }, roleIds);
        }

        /// <summary>
        /// 从多个角色中移除多个权限。
        /// </summary>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemovePermissionsFromRoles(int[] permissionIds, int[] roleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                return RemovePermissionsFromRoles(conn, permissionIds, roleIds);
            }
        }

        /// <summary>
        /// 从多个角色中移除多个权限。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="permissionIds">权限ID的集合。</param>
        /// <param name="roleIds">角色ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        public bool RemovePermissionsFromRoles(SqlConnection conn, int[] permissionIds, int[] roleIds)
        {
            using (SqlCommand cmd = new SqlCommand(REMOVE_PERMISSION_FROM_ROLE, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PermissionID", SqlDbType.Int);
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
                        foreach (int permissionId in permissionIds)
                        {
                            foreach (int roleId in roleIds)
                            {
                                cmd.Parameters[0].Value = permissionId;
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
        /// 更新指定角色的权限信息。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="cmd">数据库命令。</param>
        /// <param name="role">待更新角色。</param>
        public void UpdatePermissionsForRole(SqlConnection conn, SqlCommand cmd, RoleInfo role)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            //删除已有权限
            cmd.CommandText = REMOVE_ALL_PERMISSIONS_FOR_ROLE;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@RoleID", role.RoleId);
            cmd.ExecuteNonQuery();

            //添加新权限
            if (role.Permissions != null)
            {
                cmd.CommandText = ADD_PERMISSION_TO_ROLE;
                cmd.Parameters.Add("@PermissionID", SqlDbType.Int);
                foreach (PermissionInfo permission in role.Permissions)
                {
                    cmd.Parameters[1].Value = permission.PermissionId;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 返回指定权限是否存在
        /// </summary>
        /// <param name="permissionName">权限名</param>
        /// <returns>是否存在</returns>
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
            permission.PermissionId = (int)reader["ID"];
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
