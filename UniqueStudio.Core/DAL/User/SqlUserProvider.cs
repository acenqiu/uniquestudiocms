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
    public class SqlUserProvider : IUser
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

        public SqlUserProvider()
        {
            //默认构造函数
        }

        public bool ApproveUser(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, APPROVE_USER, parm) > 0;
        }

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

        public bool ChangeUserPassword(UserInfo user, string newPassword, PasswordEncryptionType newPasswordEncryption)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@UserID",user.UserId),
                                                        new SqlParameter("@Password",newPassword),
                                                        new SqlParameter("@PasswordEncryption",Enum.GetName(typeof(PasswordEncryptionType), newPasswordEncryption))};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, CHANGE_USER_PASSWORD, parms) > 0;
        }

        public bool ChangeUserPasswordQuestionAndAnswer(UserInfo user)
        {
            SqlParameter[] parms = new SqlParameter[] {
                                                        new SqlParameter("@UserID",user.UserId),
                                                        new SqlParameter("@PasswordQuestion",user.PasswordQuestion),
                                                        new SqlParameter("@PasswordAnswer",user.PasswordAnswer)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, CHANGE_USER_PASSWORD_QA, parms) > 0;
        }

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

        public bool LockUser(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, LOCK_USER, parm) > 0;
        }

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

        public bool UnLockUser(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UNLOCK_USER, parm) > 0;
        }

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

        public bool UpdateUserExInfo(UserInfo user)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@UserID",user.UserId),
                                                        new SqlParameter("@ExInfo",user.ExInfoXml)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_USER_EXINFO, parms) > 0;
        }

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
