using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.DAL.IDAL;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;

namespace UniqueStudio.DAL.User
{
    /// <summary>
    /// 提供用户管理在Sql Server上的实现方法
    /// </summary>
    internal class SqlUserProvider : IUser
    {
        private const string APPROVE_USER = "ApproveUser";
        private const string CREATE_USER = "CreateUser";
        private const string CHANGE_USER_PASSWORD = "ChangeUserPassword";
        private const string CHANGE_USER_PASSWORD_QA = "ChangeUserPasswordQA";
        private const string DELETE_USER = "DeleteUser";
        private const string GET_USERINFO = "GetUserInfo";
        private const string GET_USER_LIST = "GetUserList";
        private const string IS_USER_ONLINE = "IsUserOnline";
        private const string LOCK_USER = "LockUser";
        private const string UNLOCK_USER = "UnLockUser";
        private const string UPDATE_USER_EXINFO = "UpdateUserExInfo";
        private const string RESET_USER_PASSWORD = "ResetUserPassword";
        private const string VALID_USER_BY_EMAIL = "ValidUserByEmail";
        private const string VALID_USER_BY_USERNAME = "ValidUserByUserName";

        /// <summary>
        /// 初始化<see cref="SqlUserProvider"/>类的实例
        /// </summary>
        public SqlUserProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 激活指定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否激活成功</returns>
        public bool ApproveUser(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, APPROVE_USER, parm) > 0;
        }

        /// <summary>
        /// 激活多个用户
        /// </summary>
        /// <param name="userIds">用户ID的集合</param>
        /// <returns>是否激活成功</returns>
        public bool ApproveUsers(Guid[] userIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(APPROVE_USER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (Guid userId in userIds)
                            {
                                cmd.Parameters[0].Value = userId;
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
        /// 创建用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>如果创建成功，返回用户信息，否则返回空</returns>
        public UserInfo CreateUser(UserInfo user)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(CREATE_USER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@PasswordEncryption", Enum.GetName(typeof(PasswordEncryptionType), user.PasswordEncryption));
                    cmd.Parameters.AddWithValue("@PasswordQuestion", user.PasswordQuestion);
                    cmd.Parameters.AddWithValue("@PasswordAnswer", user.PasswordAnswer);
                    cmd.Parameters.AddWithValue("@IsApproved", user.IsApproved);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            object o = cmd.ExecuteScalar();
                            if (o != null && o != DBNull.Value)
                            {
                                user.UserId = new Guid(o.ToString());
                                user.Password = string.Empty;

                                if (user.Roles != null)
                                {
                                    Permission.SqlRoleProvider roleProvider = new Permission.SqlRoleProvider();
                                    if (!roleProvider.AddUserToRoles(user, user.Roles))
                                    {
                                        //添加到角色失败，回滚
                                        trans.Rollback();
                                        return null;
                                    }
                                }
                                trans.Commit();
                                return user;
                            }
                            else
                            {
                                //用户添加失败，回滚
                                trans.Rollback();
                                return null;
                            }
                        }
                        catch
                        {
                            //出现异常，回滚
                            trans.Rollback();
                            return null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <remarks>只能修改自己的密码。用户信息需提供用户ID，邮箱及密码。</remarks>
        /// <param name="user">用户信息</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="newPasswordEncryption">密码加密方式</param>
        /// <returns>是否成功</returns>
        public bool ChangeUserPassword(UserInfo user, string newPassword, PasswordEncryptionType newPasswordEncryption)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@UserID",user.UserId),
                                                        new SqlParameter("@Password",newPassword),
                                                        new SqlParameter("@PasswordEncryption",Enum.GetName(typeof(PasswordEncryptionType), newPasswordEncryption))};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, CHANGE_USER_PASSWORD, parms) > 0;
        }

        /// <summary>
        /// 修改用户密码提示问题及答案
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否修改成功</returns>
        public bool ChangeUserPasswordQuestionAndAnswer(UserInfo user)
        {
            SqlParameter[] parms = new SqlParameter[] {
                                                        new SqlParameter("@UserID",user.UserId),
                                                        new SqlParameter("@PasswordQuestion",user.PasswordQuestion),
                                                        new SqlParameter("@PasswordAnswer",user.PasswordAnswer)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, CHANGE_USER_PASSWORD_QA, parms) > 0;
        }

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteUser(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_USER, parm) > 0;
        }

        /// <summary>
        /// 删除多个用户
        /// </summary>
        /// <param name="userIds">用户ID的集合</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteUsers(Guid[] userIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_USER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (Guid userId in userIds)
                            {
                                cmd.Parameters[0].Value = userId;
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
        /// 返回用户列表
        /// </summary>
        /// <param name="pageIndex">页索引（从1开始）</param>
        /// <param name="pageSize">每页的条目数</param>
        /// <returns>用户列表</returns>
        public UserCollection GetUserList(int pageIndex, int pageSize)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PageIndex",pageIndex),
                                                    new SqlParameter("@PageSize",pageSize),
                                                    new SqlParameter("@Amount",SqlDbType.Int)};
                    parms[0].Direction = ParameterDirection.InputOutput;
                    parms[2].Direction = ParameterDirection.Output;
                    SqlHelper.PrepareCommand(cmd, conn, CommandType.StoredProcedure, GET_USER_LIST, parms);

                    UserCollection collection = new UserCollection(pageSize);
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            collection.Add(FillUserInfo(reader));
                        }
                        reader.Close();
                    }

                    collection.PageIndex = (int)parms[0].Value;
                    collection.Amount = (int)parms[2].Value;
                    collection.PageSize = pageSize;
                    cmd.Parameters.Clear();

                    return collection;
                }
            }
        }

        /// <summary>
        /// 返回指定用户的信息
        /// </summary>
        /// <remarks>
        /// 仅包含基本信息，不含角色、权限信息。
        /// 建议在用户修改自身信息时调用。
        /// </remarks>
        /// <param name="userId">用户ID</param>
        /// <param name="includeExInfo">是否包含用户扩展信息</param>
        /// <returns>用户信息</returns>
        public UserInfo GetUserInfo(Guid userId, bool includeExInfo)
        {
            UserInfo user = null;
            SqlParameter[] parms = new SqlParameter[]{    
                                                    new SqlParameter("@UserID", userId),
                                                    new SqlParameter("@IncludeExInfo",includeExInfo)};
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_USERINFO, parms))
            {
                if (reader.Read())
                {
                    user = FillUserInfo(reader);
                    if (includeExInfo)
                    {
                        if (reader["ExInfo"] != DBNull.Value)
                        {
                            user.ExInfoXml = reader["ExInfo"].ToString();
                        }
                        else
                        {
                            user.ExInfoXml = null;
                        }
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// 返回用户的完整信息
        /// </summary>
        /// <remarks>
        /// 该方法在GetUserInfo（不含附加信息）的基础上增加角色、权限信息。
        /// 建议在管理员管理用户时使用。
        /// </remarks>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        public UserInfo GetEntireUserInfo(Guid userId)
        {
            UserInfo user = null;
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@UserID", userId),
                                                        new SqlParameter("@IncludeExInfo",false)};
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_USERINFO, parms))
            {
                if (reader.Read())
                {
                    user = FillUserInfo(reader);
                    user.Roles = (new Permission.SqlRoleProvider()).GetRolesForUser(user);
                    user.Permissions = (new Permission.SqlPermissionProvider()).GetPermissionsForUser(user);
                }
            }
            return user;
        }

        /// <summary>
        /// 判断用户是否处于在线状态
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否在线</returns>
        public bool IsUserOnline(UserInfo user)
        {
            SqlParameter parm = new SqlParameter("@UserID", user.UserId);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_USER_ONLINE, parm);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToInt32(o) == 1;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 锁定指定用户
        /// </summary>
        /// <param name="userId">待锁定用户ID</param>
        /// <returns>是否锁定成功</returns>
        public bool LockUser(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, LOCK_USER, parm) > 0;
        }

        /// <summary>
        /// 锁定多个用户
        /// </summary>
        /// <param name="userIds">待锁定用户ID的集合</param>
        /// <returns>是否锁定成功</returns>
        public bool LockUsers(Guid[] userIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(LOCK_USER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (Guid userId in userIds)
                            {
                                cmd.Parameters[0].Value = userId;
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
        /// 解锁指定用户
        /// </summary>
        /// <param name="userId">待解锁用户ID</param>
        /// <returns>是否解锁成功</returns>
        public bool UnLockUser(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UNLOCK_USER, parm) > 0;
        }

        /// <summary>
        /// 解锁多个用户
        /// </summary>
        /// <param name="userIds">待解锁用户ID的集合</param>
        /// <returns>是否解锁成功</returns>
        public bool UnLockUsers(Guid[] userIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(UNLOCK_USER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (Guid userId in userIds)
                            {
                                cmd.Parameters[0].Value = userId;
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
        /// 更新用户附加信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateUserExInfo(UserInfo user)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@UserID",user.UserId),
                                                        new SqlParameter("@ExInfo",user.ExInfoXml)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_USER_EXINFO, parms) > 0;
        }

        /// <summary>
        /// 用户验证
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="type">验证类型</param>
        /// <returns>用户信息</returns>
        public UserInfo ValidUser(string account,ValidationType type)
        {
            SqlParameter parm=null;
            string cmdText = null;
            if (type == ValidationType.Email)
            {
                parm = new SqlParameter("@Email", account);
                cmdText = VALID_USER_BY_EMAIL;
            }
            else
            {
                parm = new SqlParameter("@UserName", account);
                cmdText = VALID_USER_BY_USERNAME;
            }
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, cmdText, parm))
            {
                if (reader.Read())
                {
                    UserInfo user = new UserInfo();
                    user.UserId = new Guid(reader["UserID"].ToString());
                    user.Password = reader["Password"].ToString();
                    user.PasswordEncryption = (PasswordEncryptionType)Enum.Parse(typeof(PasswordEncryptionType),
                                                                                                        reader["PasswordEncryption"].ToString());
                    user.IsApproved = Convert.ToBoolean(reader["IsApproved"].ToString());
                    user.IsLockedOut = Convert.ToBoolean(reader["IsLockedOut"].ToString());
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        private UserInfo FillUserInfo(SqlDataReader reader)
        {
            UserInfo user = new UserInfo();
            user.UserId = new Guid(reader["UserID"].ToString());
            user.Email = reader["Email"].ToString();
            user.UserName = reader["UserName"].ToString();
            user.PasswordEncryption = (PasswordEncryptionType)Enum.Parse(typeof(PasswordEncryptionType),
                                                                            reader["PasswordEncryption"].ToString());
            user.PasswordQuestion = reader["PasswordQuestion"].ToString();
            user.PasswordAnswer = reader["PasswordAnswer"].ToString();
            user.CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
            user.LastActivityDate = Convert.ToDateTime(reader["LastActivityDate"].ToString());
            user.IsApproved = Convert.ToBoolean(reader["IsApproved"].ToString());
            user.IsLockedOut = Convert.ToBoolean(reader["IsLockedOut"].ToString());
            user.IsOnline = Convert.ToBoolean(reader["IsOnline"].ToString());
            if (DBNull.Value != reader["SessionID"])
            {
                user.SessionId = reader["SessionID"].ToString();
                user.ExpireTime = Convert.ToDateTime(reader["ExpireTime"].ToString());
            }
            else
            {
                user.SessionId = null;
            }
            return user;
        }
    }
}
