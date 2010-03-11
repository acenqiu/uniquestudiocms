using System;
using System.Text.RegularExpressions;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Security;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.User
{
    /// <summary>
    /// 提供用户管理的方法
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// 表示将处理用户成功创建事件的方法
        /// </summary>
        /// <param name="sender">触发该事件的对象，总为空</param>
        /// <param name="e">事件参数</param>
        public delegate void UserCreatedEventHandler(object sender, UserEventArgs e);
        /// <summary>
        /// 表示将处理用户成功删除事件的方法
        /// </summary>
        /// <param name="sender">触发该事件的对象，总为空</param>
        /// <param name="e">事件参数</param>
        public delegate void UserDeletedEventHandler(object sender, UserEventArgs e);
        /// <summary>
        /// 表示将处理用户验证事件的方法
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="e">事件参数</param>
        public delegate void UserAuthorizationEventHandler(object sender, UserEventArgs e);

        /// <summary>
        /// 当用户已成功创建时发生
        /// </summary>
        public static event UserCreatedEventHandler OnUserCreated;
        /// <summary>
        /// 当用户已成功删除时发生
        /// </summary>
        public static event UserDeletedEventHandler OnUserDeleted;
        /// <summary>
        /// 当进行用户验证时发生
        /// </summary>
        public static event UserAuthorizationEventHandler OnUserAuthorization;

        private static readonly IUser provider = DALFactory.CreateUser();

        //具体规则有待商榷
        private static Regex rEmail = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        private static Regex rUserName = new Regex(".{1,20}");
        private static Regex rPassword = new Regex("[^ ]{1,40}");

        private UserInfo currentUser = null;

        /// <summary>
        /// 返回用户在线状态
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否在线</returns>
        public static bool IsUserOnline(UserInfo user)
        {
            //TODO:还须修改(在使用数据库存储用户状态时使用)
            return provider.IsUserOnline(user);
        }

        /// <summary>
        /// 初始化<see cref="UserManager"/>类的实例。
        /// </summary>
        public UserManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前登录用户的信息初始化<see cref="UserManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前登录用户</param>
        public UserManager(UserInfo currentUser)
        {
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 激活指定用户
        /// </summary>
        /// <param name="userId">待激活用户ID</param>
        /// <returns>是否激活成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有激活用户的权限时抛出该异常</exception>
        public bool ApproveUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return ApproveUser(currentUser, userId);
        }

        /// <summary>
        /// 激活指定用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userId">待激活用户ID</param>
        /// <returns>是否激活成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有激活用户的权限时抛出该异常</exception>
        public bool ApproveUser(UserInfo currentUser, Guid userId)
        {
            if (!PermissionManager.HasPermission(currentUser, "ApproveUser"))
            {
                throw new InvalidPermissionException("该用户无激活用户的权限，请与管理员联系！");
            }
            try
            {
                return provider.ApproveUser(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 激活多个用户
        /// </summary>
        /// <param name="userIds">待激活用户ID的集合</param>
        /// <returns>是否激活成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有激活用户的权限时抛出该异常</exception>
        public bool ApproveUsers(Guid[] userIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return ApproveUsers(currentUser, userIds);
        }

        /// <summary>
        /// 激活多个用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userIds">待激活用户ID的集合</param>
        /// <returns>是否激活成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有激活用户的权限时抛出该异常</exception>
        public bool ApproveUsers(UserInfo currentUser, Guid[] userIds)
        {
            if (!PermissionManager.HasPermission(currentUser, "ApproveUser"))
            {
                throw new InvalidPermissionException("该用户无激活用户的权限，请与管理员联系！");
            }
            try
            {
                return provider.ApproveUsers(userIds);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">待创建用户的信息</param>
        /// <returns>如果创建成功，返回该用户信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建用户的权限时抛出该异常</exception>
        public UserInfo CreateUser(UserInfo user)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return CreateUser(currentUser, user);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <remarks>如果是外部注册，请将<paramref name="currentUser"/>设置为空</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="user">待创建用户的信息</param>
        /// <returns>如果创建成功，返回该用户信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建用户的权限时抛出该异常</exception>
        public UserInfo CreateUser(UserInfo currentUser, UserInfo user)
        {
            //参数检测
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("邮箱、用户名、密码必须设置！", "user");
            }
            if (!rEmail.IsMatch(user.Email))
            {
                throw new ArgumentException("邮箱格式不正确！");
            }
            if (!rUserName.IsMatch(user.UserName))
            {
                throw new ArgumentException("用户名格式不正确！");
            }
            if (rEmail.IsMatch(user.UserName))
            {
                throw new ArgumentException("请不要将用户名设置成邮箱格式！");
            }
            if (!rPassword.IsMatch(user.Password))
            {
                throw new ArgumentException("密码格式不正确！");
            }

            //权限检测
            if (currentUser == null)
            {
                if (WebSiteConfig.EnableRegister == false)
                {
                    throw new Exception("管理员禁用了外部注册功能！");
                }
                else
                {
                    user.IsApproved = WebSiteConfig.IsApprovedAfterRegister;
                }
            }
            else
            {
                if (!PermissionManager.HasPermission(currentUser, "CreateUser"))
                {
                    throw new InvalidPermissionException("该用户无创建用户的权限，请与管理员联系！");
                }
                user.IsApproved = true;
            }

            user.PasswordEncryption = GlobalConfig.DefaultPasswordEncryption;
            switch (user.PasswordEncryption)
            {
                case PasswordEncryptionType.Clear:
                    break;
                case PasswordEncryptionType.Encrypted:
                    break;
                case PasswordEncryptionType.Hashed:
                    user.Password = MD5Helper.MD5Encrypt(user.Password);
                    break;
            }

            try
            {
                user = provider.CreateUser(user);
                if (user != null)
                {
                    if (OnUserCreated != null)
                    {
                        try
                        {
                            OnUserCreated(null, new UserEventArgs(user));
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.LogError(ex, "OnUserCreated事件触发异常");
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <remarks>只能修改自己的密码。用户信息需提供用户ID，邮箱及密码。</remarks>
        /// <param name="user">用户信息</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>是否成功</returns>
        public bool ChangeUserPassword(UserInfo user, string newPassword)
        {
            if (user == null || string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException();
            }

            UserInfo temp = UserAuthorization(user.Email, user.Password);
            if (temp != null)
            {
                if (temp.UserId != user.UserId)
                {
                    throw new Exception("您修改的不是自己的密码！");
                }

                //考虑去掉最后一个参数
                try
                {
                    if (GlobalConfig.DefaultPasswordEncryption == PasswordEncryptionType.Hashed)
                    {
                        newPassword = MD5Helper.MD5Encrypt(user.Password); 
                    }
                    return provider.ChangeUserPassword(user, newPassword, GlobalConfig.DefaultPasswordEncryption);
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new DatabaseException();
                }
            }
            else
            {
                throw new Exception("用户名或密码错误！");
            }
        }

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除用户的权限时抛出该异常</exception>
        public bool DeleteUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return DeleteUser(userId);
        }

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除用户的权限时抛出该异常</exception>
        public bool DeleteUser(UserInfo currentUser, Guid userId)
        {
            if (userId == new Guid())
            {
                throw new ArgumentException("用户ID错误");
            }

            if (!PermissionManager.HasPermission(currentUser, "DeleteUser"))
            {
                throw new InvalidPermissionException("当前用户没有删除用户的权限，请与管理员联系！");
            }

            try
            {
                return provider.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 删除多个用户
        /// </summary>
        /// <param name="userIds">待删除用户ID的集合</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public bool DeleteUsers(Guid[] userIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return DeleteUsers(currentUser, userIds);
        }

        /// <summary>
        /// 删除多个用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userIds">待删除用户ID的集合</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public bool DeleteUsers(UserInfo currentUser, Guid[] userIds)
        {
            if (!PermissionManager.HasPermission(currentUser, "DeleteUser"))
            {
                throw new InvalidPermissionException("当前用户没有删除用户的权限，请与管理员联系！");
            }

            try
            {
                return provider.DeleteUsers(userIds);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <remarks>返回第一页，每页条目数为默认值</remarks>
        /// <returns>用户列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserCollection GetUserList()
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetUserList(currentUser);
        }

        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <remarks>每页条目数为默认值</remarks>
        /// <param name="pageIndex">页索引（从1开始）</param>
        /// <returns>用户列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserCollection GetUserList(int pageIndex)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetUserList(currentUser, pageIndex);
        }

        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <param name="pageIndex">页索引（从1开始）</param>
        /// <param name="pageSize">每页的条目数</param>
        /// <returns>用户列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserCollection GetUserList(int pageIndex, int pageSize)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetUserList(currentUser, pageIndex, pageSize);
        }

        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <remarks>返回第一页，每页条目数为默认值</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <returns>用户列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserCollection GetUserList(UserInfo currentUser)
        {
            return GetUserList(currentUser, 1);
        }

        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <remarks>每页条目数为默认值</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="pageIndex">页索引（从1开始）</param>
        /// <returns>用户列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserCollection GetUserList(UserInfo currentUser, int pageIndex)
        {
            return GetUserList(currentUser, pageIndex, 10);
        }

        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="pageIndex">页索引（从1开始）</param>
        /// <param name="pageSize">每页的条目数</param>
        /// <returns>用户列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserCollection GetUserList(UserInfo currentUser, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("页面及每个页面的条数不能小于1。");
            }

            if (!PermissionManager.HasPermission(currentUser, "ViewUserInfo"))
            {
                throw new InvalidPermissionException("当前用户没有查看用户信息的权限，请与管理员联系！");
            }

            try
            {
                return provider.GetUserList(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 返回指定用户的信息
        /// </summary>
        /// <remarks>仅自身或具有查看用户信息的用户可以获取</remarks>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserInfo GetUserInfo(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetUserInfo(currentUser, userId);
        }

        /// <summary>
        /// 返回指定用户的信息
        /// </summary>
        /// <remarks>仅自身或具有查看用户信息的用户可以获取</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserInfo GetUserInfo(UserInfo currentUser, Guid userId)
        {
            if (currentUser == null || (currentUser.UserId != userId && !PermissionManager.HasPermission(currentUser, "ViewUserInfo")))
            {
                throw new InvalidPermissionException("当前用户没有查看用户信息的权限，请与管理员联系！");
            }

            try
            {
                if (currentUser.UserId == userId)
                {
                    //仅用户自身可以获取附加信息
                    UserInfo user = provider.GetUserInfo(userId, true);
                    if (user != null && user.ExInfoXml != null)
                    {
                        XmlManager manager = new XmlManager();
                        user.ExInfo = (UserExInfo)manager.ConvertToEntity(user.ExInfoXml, typeof(UserExInfo), null);
                        user.ExInfoXml = null;
                    }
                    return user;
                }
                else
                {
                    return provider.GetUserInfo(userId, false);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 返回用户的完整信息
        /// </summary>
        /// <remarks>仅自身或具有查看用户信息的用户可以获取</remarks>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserInfo GetEntireUserInfo(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetEntireUserInfo(currentUser, userId);
        }

        /// <summary>
        /// 返回用户的完整信息
        /// </summary>
        /// <remarks>仅自身或具有查看用户信息的用户可以获取</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常</exception>
        public UserInfo GetEntireUserInfo(UserInfo currentUser, Guid userId)
        {
            if (currentUser == null || (currentUser.UserId != userId && !PermissionManager.HasPermission(currentUser, "ViewUserInfo")))
            {
                throw new InvalidPermissionException("当前用户没有查看用户信息的权限，请与管理员联系！");
            }

            try
            {
                return provider.GetEntireUserInfo(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 锁定指定用户
        /// </summary>
        /// <param name="userId">待锁定用户ID</param>
        /// <returns>是否锁定成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有锁定用户的权限时抛出该异常</exception>
        public bool LockUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return LockUser(currentUser, userId);
        }

        /// <summary>
        /// 锁定指定用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userId">待锁定用户ID</param>
        /// <returns>是否锁定成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有锁定用户的权限时抛出该异常</exception>
        public bool LockUser(UserInfo currentUser, Guid userId)
        {
            if (!PermissionManager.HasPermission(currentUser, "LockUser"))
            {
                throw new InvalidPermissionException("该用户无锁定用户的权限，请与管理员联系！");
            }
            try
            {
                return provider.LockUser(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 锁定多个用户
        /// </summary>
        /// <param name="userIds">待锁定用户ID的集合</param>
        /// <returns>是否锁定成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有锁定用户的权限时抛出该异常</exception>
        public bool LockUsers(Guid[] userIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return LockUsers(currentUser, userIds);
        }

        /// <summary>
        /// 锁定多个用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userIds">待锁定用户ID的集合</param>
        /// <returns>是否锁定成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有锁定用户的权限时抛出该异常</exception>
        public bool LockUsers(UserInfo currentUser, Guid[] userIds)
        {
            if (!PermissionManager.HasPermission(currentUser, "LockUser"))
            {
                throw new InvalidPermissionException("该用户无锁定用户的权限，请与管理员联系！");
            }
            try
            {
                return provider.LockUsers(userIds);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 解锁指定用户
        /// </summary>
        /// <param name="userId">待解锁用户ID</param>
        /// <returns>是否解锁成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有解锁用户的权限时抛出该异常</exception>
        public bool UnLockUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return UnLockUser(currentUser, userId);
        }

        /// <summary>
        /// 解锁指定用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userId">待解锁用户ID</param>
        /// <returns>是否解锁成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有解锁用户的权限时抛出该异常</exception>
        public bool UnLockUser(UserInfo currentUser, Guid userId)
        {
            if (!PermissionManager.HasPermission(currentUser, "UnLockUser"))
            {
                throw new InvalidPermissionException("该用户无解锁用户的权限，请与管理员联系！");
            }
            try
            {
                return provider.UnLockUser(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 解锁多个用户
        /// </summary>
        /// <param name="userIds">待解锁用户ID的集合</param>
        /// <returns>是否解锁成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有解锁用户的权限时抛出该异常</exception>
        public bool UnLockUsers(Guid[] userIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return UnLockUsers(currentUser, userIds);
        }

        /// <summary>
        /// 解锁多个用户
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="userIds">待解锁用户ID的集合</param>
        /// <returns>是否解锁成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有解锁用户的权限时抛出该异常</exception>
        public bool UnLockUsers(UserInfo currentUser, Guid[] userIds)
        {
            if (!PermissionManager.HasPermission(currentUser, "UnLockUser"))
            {
                throw new InvalidPermissionException("该用户无解锁用户的权限，请与管理员联系！");
            }
            try
            {
                return provider.UnLockUsers(userIds);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 更新用户附加信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateUserExInfo(UserInfo user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            //TODO:更多验证规则

            if (user.ExInfo == null)
            {
                user.ExInfoXml = string.Empty;
            }
            else
            {
                XmlManager manager = new XmlManager();
                user.ExInfoXml = manager.ConvertToXml(user.ExInfo, typeof(UserExInfo)).OuterXml;
                user.ExInfo = null;
            }

            try
            {
                return provider.UpdateUserExInfo(user);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 用户验证
        /// </summary>
        /// <remarks>根据账号的形式决定采用哪种登录方式</remarks>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>如果验证通过，返回用户信息，否则返回空</returns>
        public UserInfo UserAuthorization(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }
            if (!rPassword.IsMatch(password))
            {
                throw new ArgumentException("密码格式错误！");
            }

            UserInfo user = null;
            try
            {
                if (rEmail.IsMatch(account))
                {
                    user = provider.ValidUser(account, ValidationType.Email);
                }
                else
                {
                    user = provider.ValidUser(account, ValidationType.UserName);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }

            if (user != null)
            {
                if (user.PasswordEncryption == PasswordEncryptionType.Hashed)
                {
                    password = MD5Helper.MD5Encrypt(password.Trim());
                }
                if (password == user.Password)
                {
                    if (!user.IsApproved)
                    {
                        throw new Exception("该用户没有激活");
                    }
                    if (user.IsLockedOut)
                    {
                        throw new Exception("该用户处于锁定状态");
                    }
                    return provider.GetUserInfo(user.UserId,false);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
