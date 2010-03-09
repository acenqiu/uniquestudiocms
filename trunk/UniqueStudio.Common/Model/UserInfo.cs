using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示用户的实体类。
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        private Guid userId;
        private string email;
        private string userName;
        private string password;
        private PasswordEncryptionType passwordEncryption;
        private string passwordQuestion = string.Empty;
        private string passwordAnswer = string.Empty;
        private DateTime createDate;
        private DateTime lastActivityDate;
        private bool isApproved;
        private bool isLockedOut;
        private bool isOnline;
        private string sessionId = string.Empty;
        private DateTime expireTime;
        private UserExInfo exInfo = null;
        private string exInfoXml = null;
        private RoleCollection roles;
        private PermissionCollection permissions;

        /// <summary>
        /// 初始化<see cref="UserInfo"/>类的实例。
        /// </summary>
        public UserInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以用户ID初始化<see cref="UserInfo"/>类的实例。
        /// </summary>
        /// <param name="userId">用户ID</param>
        public UserInfo(Guid userId)
        {
            this.userId = userId;
        }

        /// <summary>
        /// 以邮箱、密码初始化UserInfo类的实例。
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        public UserInfo(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        /// <summary>
        /// 以用户ID、用户名、SessionID初始化UserInfo类的实例。
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="sessionId">SessionID</param>
        public UserInfo(Guid userId, string userName, string sessionId)
        {
            this.userId = userId;
            this.userName = userName;
            this.sessionId = sessionId;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// 密码加密方式
        /// </summary>
        public PasswordEncryptionType PasswordEncryption
        {
            get { return passwordEncryption; }
            set { passwordEncryption = value; }
        }

        /// <summary>
        /// 密码提示问题
        /// </summary>
        public string PasswordQuestion
        {
            get { return passwordQuestion; }
            set { passwordQuestion = value; }
        }

        /// <summary>
        /// 密码提示问题答案
        /// </summary>
        public string PasswordAnswer
        {
            get { return passwordAnswer; }
            set { passwordAnswer = value; }
        }

        /// <summary>
        /// 用户创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 用户最后一次登录时间
        /// </summary>
        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        /// <summary>
        /// 用户是否激活
        /// </summary>
        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        /// <summary>
        /// 用户是否被锁定
        /// </summary>
        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }

        /// <summary>
        /// 用户是否在线
        /// </summary>
        public bool IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; }
        }

        /// <summary>
        /// 用户SessionID
        /// </summary>
        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        /// <summary>
        /// 用户会话失效时间（绝对时间）
        /// </summary>
        public DateTime ExpireTime
        {
            get { return expireTime; }
            set { expireTime = value; }
        }

        /// <summary>
        /// 用户附加信息
        /// </summary>
        /// <remarks>调用前请查空，并且确保该属性和其对应的XML值至少有一个不为空。</remarks>
        public UserExInfo ExInfo
        {
            get { return exInfo; }
            set { exInfo = value; }
        }

        /// <summary>
        /// 用户附加信息对应的XML存储形式
        /// </summary>
        public string ExInfoXml
        {
            get { return exInfoXml; }
            set { exInfoXml = value; }
        }

        /// <summary>
        /// 用户所属组
        /// </summary>
        /// <remarks>调用前检测是否为空，若为空表示没设置</remarks>
        public RoleCollection Roles
        {
            get { return roles; }
            set { roles = value; }
        }

        /// <summary>
        /// 用户所具有权限
        /// </summary>
        /// <remarks>调用前检测是否为空，若为空表示没设置</remarks>
        public PermissionCollection Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }
    }
}
