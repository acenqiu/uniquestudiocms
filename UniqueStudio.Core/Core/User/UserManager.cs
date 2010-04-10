//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供用户管理的方法。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Data.Common;
using System.Text.RegularExpressions;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Security;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.User
{
    /// <summary>
    /// 提供用户管理的方法。
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// 表示将处理用户成功创建事件的方法。
        /// </summary>
        /// <param name="sender">触发该事件的对象，总为空。</param>
        /// <param name="e">事件参数。</param>
        public delegate void UserCreatedEventHandler(object sender, UserEventArgs e);

        /// <summary>
        /// 表示将处理用户成功删除事件的方法。
        /// </summary>
        /// <param name="sender">触发该事件的对象，总为空。</param>
        /// <param name="e">事件参数。</param>
        public delegate void UserDeletedEventHandler(object sender, UserEventArgs e);

        /// <summary>
        /// 表示将处理用户验证事件的方法。
        /// </summary>
        /// <param name="sender">事件参数。</param>
        /// <param name="e">事件参数。</param>
        public delegate void UserAuthorizationEventHandler(object sender, UserEventArgs e);

        /// <summary>
        /// 当用户已成功创建时发生。
        /// </summary>
        public static event UserCreatedEventHandler OnUserCreated;
        /// <summary>
        /// 当用户已成功删除时发生。
        /// </summary>
        public static event UserDeletedEventHandler OnUserDeleted;
        /// <summary>
        /// 当进行用户验证时发生。
        /// </summary>
        public static event UserAuthorizationEventHandler OnUserAuthorization;

        private static readonly IUser provider = DALFactory.CreateUser();

        //具体规则有待商榷
        private static Regex rUserName = new Regex(".{1,20}");
        private static Regex rPassword = new Regex("[^ ]{4,40}");

        private UserInfo currentUser = null;

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
            Validator.CheckNull(currentUser, "currentUser");
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 返回用户在线状态。
        /// </summary>
        /// <remarks>当用户登录信息没有存储在数据库中时始终返回真。</remarks>
        /// <param name="user">用户信息。</param>
        /// <returns>是否在线。</returns>
        public static bool IsUserOnline(UserInfo user)
        {
            //TODO:还须修改(在使用数据库存储用户状态时使用)
            //return provider.IsUserOnline(user);
            return true;
        }

        /// <summary>
        /// 激活指定用户。
        /// </summary>
        /// <remarks>需使用UserManager(UserInfo)实例化该类。</remarks>
        /// <param name="userId">待激活用户ID。</param>
        /// <returns>是否激活成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有激活用户的权限时抛出该异常。</exception>
        public bool ApproveUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return ApproveUser(currentUser, userId);
        }

        /// <summary>
        /// 激活指定用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">待激活用户ID。</param>
        /// <returns>是否激活成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有激活用户的权限时抛出该异常。</exception>
        public bool ApproveUser(UserInfo currentUser, Guid userId)
        {
            Validator.CheckGuid(userId, "userId");
            PermissionManager.CheckPermission(currentUser, "ApproveUser", "激活用户");

            try
            {
                return provider.ApproveUser(userId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 激活多个用户。
        /// </summary>
        /// <remarks>需使用UserManager(UserInfo)实例化该类。</remarks>
        /// <param name="userIds">待激活用户ID的集合。</param>
        /// <returns>是否激活成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有激活用户的权限时抛出该异常。</exception>
        public bool ApproveUsers(Guid[] userIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return ApproveUsers(currentUser, userIds);
        }

        /// <summary>
        /// 激活多个用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userIds">待激活用户ID的集合。</param>
        /// <returns>是否激活成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有激活用户的权限时抛出该异常。</exception>
        public bool ApproveUsers(UserInfo currentUser, Guid[] userIds)
        {
            Validator.CheckNull(userIds, "userIds");
            foreach (Guid userId in userIds)
            {
                Validator.CheckGuid(userId, "userIds");
            }
            PermissionManager.CheckPermission(currentUser, "ApproveUser", "激活用户");

            try
            {
                return provider.ApproveUsers(userIds);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 创建用户。
        /// </summary>
        /// <remarks>需使用UserManager(UserInfo)实例化该类。</remarks>
        /// <param name="user">待创建用户的信息。</param>
        /// <returns>如果创建成功，返回该用户信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建用户的权限时抛出该异常。</exception>
        public UserInfo CreateUser(UserInfo user)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return CreateUser(currentUser, user);
        }

        /// <summary>
        /// 创建用户。
        /// </summary>
        /// <remarks>如果是外部注册，请将<paramref name="currentUser"/>设置为空。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="user">待创建用户的信息。</param>
        /// <returns>如果创建成功，返回该用户信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建用户的权限时抛出该异常。</exception>
        public UserInfo CreateUser(UserInfo currentUser, UserInfo user)
        {
            Validator.CheckStringNull(user.UserName, "user");
            Validator.CheckStringNull(user.Password, "user");
            Validator.CheckEmail(user.Email, "user");

            if (!rUserName.IsMatch(user.UserName))
            {
                throw new ArgumentException("用户名格式不正确！");
            }
            if (Validator.CheckEmail(user.UserName))
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
                if (SecurityConfig.EnableRegister == false)
                {
                    throw new Exception("管理员禁用了外部注册功能！");
                }
                else
                {
                    user.IsApproved = SecurityConfig.IsApprovedAfterRegister;
                }
            }
            else
            {
                PermissionManager.CheckPermission(currentUser, "CreateUser", "创建用户");
                user.IsApproved = true;
            }

            if (IsEmailExists(user.Email))
            {
                throw new Exception("该邮箱已经被注册过，请重新设置！");
            }
            if (IsUserNameExists(user.UserName))
            {
                throw new Exception("该用户名已经被注册过，请重新设置！");
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
                            ErrorLogger.LogError(ex, "OnUserCreated事件触发异常。");
                        }
                    }
                }
                return user;
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 修改用户密码。
        /// </summary>
        /// <remarks>只能修改自己的密码。用户信息需提供用户ID，邮箱及密码。</remarks>
        /// <param name="user">用户信息。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>是否成功。</returns>
        public bool ChangeUserPassword(UserInfo user, string newPassword)
        {
            Validator.CheckNull(user, "user");
            Validator.CheckGuid(user.UserId, "user.UserId");
            Validator.CheckEmail(user.Email, "user.Email");
            Validator.CheckStringNull(user.Password, "user.Password");
            Validator.CheckStringNull(newPassword, "newPassword");
            if (!rPassword.IsMatch(newPassword))
            {
                throw new ArgumentException("密码格式不正确！");
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
                        newPassword = MD5Helper.MD5Encrypt(newPassword);
                    }
                    return provider.ChangeUserPassword(user, newPassword, GlobalConfig.DefaultPasswordEncryption);
                }
                catch (DbException ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new DatabaseException();
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new UnhandledException();
                }
            }
            else
            {
                throw new Exception("账号或密码错误！");
            }
        }

        /// <summary>
        /// 删除指定用户。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除用户的权限时抛出该异常。</exception>
        public bool DeleteUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return DeleteUser(userId);
        }

        /// <summary>
        /// 删除指定用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">用户ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除用户的权限时抛出该异常。</exception>
        public bool DeleteUser(UserInfo currentUser, Guid userId)
        {
            Validator.CheckGuid(userId, "userId");
            PermissionManager.CheckPermission(currentUser, "DeleteUser", "删除用户");

            try
            {
                return provider.DeleteUser(userId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 删除多个用户。
        /// </summary>
        /// <param name="userIds">待删除用户ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public bool DeleteUsers(Guid[] userIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return DeleteUsers(currentUser, userIds);
        }

        /// <summary>
        /// 删除多个用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userIds">待删除用户ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public bool DeleteUsers(UserInfo currentUser, Guid[] userIds)
        {
            Validator.CheckNull(userIds, "userIds");
            foreach (Guid userId in userIds)
            {
                Validator.CheckGuid(userId, "userIds");
            }
            PermissionManager.CheckPermission(currentUser, "DeleteUser", "删除用户");

            try
            {
                return provider.DeleteUsers(userIds);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回用户列表。
        /// </summary>
        /// <remarks>返回第一页，每页条目数为默认值。</remarks>
        /// <returns>用户列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserCollection GetUserList()
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetUserList(currentUser);
        }

        /// <summary>
        /// 返回用户列表。
        /// </summary>
        /// <remarks>每页条目数为默认值。</remarks>
        /// <param name="pageIndex">页索引（从1开始）。</param>
        /// <returns>用户列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserCollection GetUserList(int pageIndex)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetUserList(currentUser, pageIndex);
        }

        /// <summary>
        /// 返回用户列表。
        /// </summary>
        /// <param name="pageIndex">页索引（从1开始）。</param>
        /// <param name="pageSize">每页的条目数。</param>
        /// <returns>用户列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserCollection GetUserList(int pageIndex, int pageSize)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetUserList(currentUser, pageIndex, pageSize);
        }

        /// <summary>
        /// 返回用户列表。
        /// </summary>
        /// <remarks>返回第一页，每页条目数为默认值。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <returns>用户列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserCollection GetUserList(UserInfo currentUser)
        {
            return GetUserList(currentUser, 1);
        }

        /// <summary>
        /// 返回用户列表。
        /// </summary>
        /// <remarks>每页条目数为默认值。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="pageIndex">页索引（从1开始）。</param>
        /// <returns>用户列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserCollection GetUserList(UserInfo currentUser, int pageIndex)
        {
            return GetUserList(currentUser, pageIndex, 10);
        }

        /// <summary>
        /// 返回用户列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="pageIndex">页索引（从1开始）。</param>
        /// <param name="pageSize">每页的条目数。</param>
        /// <returns>用户列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserCollection GetUserList(UserInfo currentUser, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            Validator.CheckNotPositive(pageSize, "pageSize");
            PermissionManager.CheckPermission(currentUser, "ViewUserInfo", "查看用户信息");

            try
            {
                return provider.GetUserList(pageIndex, pageSize);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定用户的信息。
        /// </summary>
        /// <remarks>仅自身或具有查看用户信息的用户可以获取。</remarks>
        /// <param name="userId">用户ID。</param>
        /// <returns>用户信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserInfo GetUserInfo(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetUserInfo(currentUser, userId);
        }

        /// <summary>
        /// 返回指定用户的信息。
        /// </summary>
        /// <remarks>仅自身或具有查看用户信息的用户可以获取。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">用户ID。</param>
        /// <returns>用户信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserInfo GetUserInfo(UserInfo currentUser, Guid userId)
        {
            Validator.CheckGuid(userId, "userId");
            if (currentUser == null || (currentUser.UserId != userId && !PermissionManager.HasPermission(currentUser, "ViewUserInfo")))
            {
                throw new InvalidPermissionException("查看用户信息");
            }

            try
            {
                if (currentUser.UserId == userId)
                {
                    //仅用户自身可以获取附加信息
                    UserInfo user = provider.GetUserInfo(userId, true);
                    if (user != null && !string.IsNullOrEmpty(user.ExInfoXml))
                    {
                        user.ExInfo = (UserExInfo)XmlManager.ConvertToEntity(user.ExInfoXml, typeof(UserExInfo), null);
                        user.ExInfoXml = null;
                    }
                    return user;
                }
                else
                {
                    return provider.GetUserInfo(userId, false);
                }
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回用户的完整信息。
        /// </summary>
        /// <remarks>仅自身或具有查看用户信息的用户可以获取。</remarks>
        /// <param name="userId">用户ID。</param>
        /// <returns>用户信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserInfo GetEntireUserInfo(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return GetEntireUserInfo(currentUser, userId);
        }

        /// <summary>
        /// 返回用户的完整信息。
        /// </summary>
        /// <remarks>仅自身或具有查看用户信息的用户可以获取。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">用户ID。</param>
        /// <returns>用户信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看用户列表的权限时抛出该异常。</exception>
        public UserInfo GetEntireUserInfo(UserInfo currentUser, Guid userId)
        {
            Validator.CheckGuid(userId, "userId");

            if (currentUser == null || (currentUser.UserId != userId && !PermissionManager.HasPermission(currentUser, "ViewUserInfo")))
            {
                throw new InvalidPermissionException("查看用户信息");
            }

            try
            {
                return provider.GetEntireUserInfo(userId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 判断某一特定的邮箱是否存在。
        /// </summary>
        /// <param name="email">邮箱。</param>
        /// <returns>是否存在。</returns>
        public bool IsEmailExists(string email)
        {
            Validator.CheckEmail(email, "email");

            try
            {
                return provider.IsEmailExists(email);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 判断某一特定的用户名是否存在。
        /// </summary>
        /// <param name="userName">用户名。</param>
        /// <returns>是否存在。</returns>
        public bool IsUserNameExists(string userName)
        {
            if (!rUserName.IsMatch(userName))
            {
                throw new ArgumentException("用户名格式不正确！");
            }

            try
            {
                return provider.IsUserNameExists(userName);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 锁定指定用户。
        /// </summary>
        /// <param name="userId">待锁定用户ID。</param>
        /// <returns>是否锁定成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有锁定用户的权限时抛出该异常。</exception>
        public bool LockUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return LockUser(currentUser, userId);
        }

        /// <summary>
        /// 锁定指定用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">待锁定用户ID。</param>
        /// <returns>是否锁定成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有锁定用户的权限时抛出该异常。</exception>
        public bool LockUser(UserInfo currentUser, Guid userId)
        {
            Validator.CheckGuid(userId, "userId");
            PermissionManager.CheckPermission(currentUser, "LockUser", "锁定用户");

            try
            {
                return provider.LockUser(userId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 锁定多个用户。
        /// </summary>
        /// <param name="userIds">待锁定用户ID的集合。</param>
        /// <returns>是否锁定成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有锁定用户的权限时抛出该异常。</exception>
        public bool LockUsers(Guid[] userIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return LockUsers(currentUser, userIds);
        }

        /// <summary>
        /// 锁定多个用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userIds">待锁定用户ID的集合。</param>
        /// <returns>是否锁定成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有锁定用户的权限时抛出该异常。</exception>
        public bool LockUsers(UserInfo currentUser, Guid[] userIds)
        {
            Validator.CheckNull(userIds, "userIds");
            foreach (Guid userId in userIds)
            {
                Validator.CheckGuid(userId, "userIds");
            }
            PermissionManager.CheckPermission(currentUser, "LockUser", "锁定用户");

            try
            {
                return provider.LockUsers(userIds);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 解锁指定用户。
        /// </summary>
        /// <param name="userId">待解锁用户ID。</param>
        /// <returns>是否解锁成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有解锁用户的权限时抛出该异常。</exception>
        public bool UnLockUser(Guid userId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return UnLockUser(currentUser, userId);
        }

        /// <summary>
        /// 解锁指定用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userId">待解锁用户ID。</param>
        /// <returns>是否解锁成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有解锁用户的权限时抛出该异常。</exception>
        public bool UnLockUser(UserInfo currentUser, Guid userId)
        {
            Validator.CheckGuid(userId, "userId");
            PermissionManager.CheckPermission(currentUser, "UnLockUser", "解锁用户");

            try
            {
                return provider.UnLockUser(userId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 解锁多个用户。
        /// </summary>
        /// <param name="userIds">待解锁用户ID的集合。</param>
        /// <returns>是否解锁成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有解锁用户的权限时抛出该异常。</exception>
        public bool UnLockUsers(Guid[] userIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用UserManager(UserInfo)实例化该类。");
            }
            return UnLockUsers(currentUser, userIds);
        }

        /// <summary>
        /// 解锁多个用户。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="userIds">待解锁用户ID的集合。</param>
        /// <returns>是否解锁成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有解锁用户的权限时抛出该异常。</exception>
        public bool UnLockUsers(UserInfo currentUser, Guid[] userIds)
        {
            Validator.CheckNull(userIds, "userIds");
            foreach (Guid userId in userIds)
            {
                Validator.CheckGuid(userId, "userIds");
            }
            PermissionManager.CheckPermission(currentUser, "UnLockUser", "解锁用户");

            try
            {
                return provider.UnLockUsers(userIds);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 更新用户附加信息。
        /// </summary>
        /// <param name="user">用户信息。</param>
        /// <returns>是否更新成功。</returns>
        public bool UpdateUserExInfo(UserInfo user)
        {
            Validator.CheckNull(user, "user");
            Validator.CheckGuid(user.UserId, "user");

            if (!IsUserOnline(user))
            {
                throw new Exception("您修改的用户不处于在线状态！");
            }

            if (user.ExInfo == null)
            {
                user.ExInfoXml = string.Empty;
            }
            else
            {
                try
                {
                    user.ExInfoXml = XmlManager.ConvertToXml(user.ExInfo, typeof(UserExInfo)).OuterXml;
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new UnhandledException();
                }
                user.ExInfo = null;
            }

            try
            {
                return provider.UpdateUserExInfo(user);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 用户验证。
        /// </summary>
        /// <remarks>根据账号的形式决定采用哪种登录方式。</remarks>
        /// <param name="account">账号。</param>
        /// <param name="password">密码。</param>
        /// <returns>如果验证通过，返回用户信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.UserDoesNotApprovedException">
        /// 当用户未激活时抛出该异常。</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.UserLockedOutException">
        /// 当用户被锁定时抛出该异常。</exception>
        public UserInfo UserAuthorization(string account, string password)
        {
            Validator.CheckStringNull(account, "account");
            Validator.CheckStringNull(account, "password");

            if (!rPassword.IsMatch(password))
            {
                throw new ArgumentException("密码格式错误！");
            }

            UserInfo user = null;
            try
            {
                if (Validator.CheckEmail(account))
                {
                    user = provider.ValidUser(account, ValidationType.Email);
                }
                else
                {
                    user = provider.ValidUser(account, ValidationType.UserName);
                }
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
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
                        throw new UserDoesNotApprovedException();
                    }
                    if (user.IsLockedOut)
                    {
                        throw new UserLockedOutException();
                    }
                    return provider.GetUserInfo(user.UserId, false);
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
