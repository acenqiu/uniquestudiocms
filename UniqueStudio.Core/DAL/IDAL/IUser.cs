using System;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 用户管理提供类需实现的方法
    /// </summary>
    internal interface IUser
    {
        /// <summary>
        /// 判断用户是否处于在线状态
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否在线</returns>
        bool IsUserOnline(UserInfo user);

        /// <summary>
        /// 激活指定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否激活成功</returns>
        bool ApproveUser(Guid userId);

        /// <summary>
        /// 激活多个用户
        /// </summary>
        /// <param name="userIds">用户ID的集合</param>
        /// <returns>是否激活成功</returns>
        bool ApproveUsers(Guid[] userIds);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>如果创建成功，返回用户信息，否则返回空</returns>
        UserInfo CreateUser(UserInfo user);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <remarks>只能修改自己的密码。用户信息需提供用户ID，邮箱及密码。</remarks>
        /// <param name="user">用户信息</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="newPasswordEncryption">密码加密方式</param>
        /// <returns>是否成功</returns>
        bool ChangeUserPassword(UserInfo user, string newPassword, 
                                                PasswordEncryptionType newPasswordEncryption);

        /// <summary>
        /// 修改用户密码提示问题及答案
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否修改成功</returns>
        bool ChangeUserPasswordQuestionAndAnswer(UserInfo user);

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否删除成功</returns>
        bool DeleteUser(Guid userId);

        /// <summary>
        /// 删除多个用户
        /// </summary>
        /// <param name="userIds">用户ID的集合</param>
        /// <returns>是否删除成功</returns>
        bool DeleteUsers(Guid[] userIds);

        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <param name="pageIndex">页索引（从1开始）</param>
        /// <param name="pageSize">每页的条目数</param>
        /// <returns>用户列表</returns>
        UserCollection GetUserList(int pageIndex, int pageSize);

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
        UserInfo GetUserInfo(Guid userId, bool includeExInfo);

        /// <summary>
        /// 返回用户的完整信息
        /// </summary>
        /// <remarks>
        /// 该方法在GetUserInfo（不含附加信息）的基础上增加角色、权限信息。
        /// 建议在管理员管理用户时使用。
        /// </remarks>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        UserInfo GetEntireUserInfo(Guid userId);

        /// <summary>
        /// 判断某一特定的邮箱是否存在。
        /// </summary>
        /// <param name="email">邮箱。</param>
        /// <returns>是否存在。</returns>
        bool IsEmailExists(string email);

        /// <summary>
        /// 判断某一特定的用户名是否存在。
        /// </summary>
        /// <param name="userName">用户名。</param>
        /// <returns>是否存在。</returns>
        bool IsUserNameExists(string userName);

        /// <summary>
        /// 锁定指定用户
        /// </summary>
        /// <param name="userId">待锁定用户ID</param>
        /// <returns>是否锁定成功</returns>
        bool LockUser(Guid userId);

        /// <summary>
        /// 锁定多个用户
        /// </summary>
        /// <param name="userIds">待锁定用户ID的集合</param>
        /// <returns>是否锁定成功</returns>
        bool LockUsers(Guid[] userIds);

        /// <summary>
        /// 解锁指定用户
        /// </summary>
        /// <param name="userId">待解锁用户ID</param>
        /// <returns>是否解锁成功</returns>
        bool UnLockUser(Guid userId);

        /// <summary>
        /// 解锁多个用户
        /// </summary>
        /// <param name="userIds">待解锁用户ID的集合</param>
        /// <returns>是否解锁成功</returns>
        bool UnLockUsers(Guid[] userIds);

        /// <summary>
        /// 更新用户附加信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否更新成功</returns>
        bool UpdateUserExInfo(UserInfo user);

        /// <summary>
        /// 用户验证
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="type">验证类型</param>
        /// <returns>用户信息</returns>
        UserInfo ValidUser(string account,ValidationType type);
    }
}
