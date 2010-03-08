using System;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface IUser
    {
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
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <param name="newPasswordEncryption"></param>
        /// <returns></returns>
        bool ChangeUserPassword(UserInfo user, string newPassword, 
                                                PasswordEncryptionType newPasswordEncryption);

        /// <summary>
        /// 修改用户密码提示问题及答案
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool ChangeUserPasswordQuestionAndAnswer(UserInfo user);

        /// <summary>
        /// 删除某一用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool DeleteUser(Guid userId);

        /// <summary>
        /// 删除多个用户
        /// </summary>
        /// <param name="userIds">用户ID的集合</param>
        /// <returns>是否删除成功</returns>
        bool DeleteUsers(Guid[] userIds);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        UserCollection GetUserList(int pageIndex, int pageSize);

        /// <summary>
        /// 获取某一用户信息
        /// </summary>
        /// <remarks>
        /// 仅包含基本信息，不含角色、权限信息。
        /// 建议在用户修改自身信息时调用。
        /// </remarks>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        UserInfo GetUserInfo(Guid userId, bool includeExInfo);

        /// <summary>
        /// 获取用户的完整信息
        /// </summary>
        /// <remarks>
        /// 该方法在GetUserInfo（不含附加信息）的基础上增加角色、权限信息。
        /// 建议在管理员管理用户时使用。
        /// </remarks>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        UserInfo GetEntireUserInfo(Guid userId);

        /// <summary>
        /// 判断用户是否处于在线状态
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否在线</returns>
        bool IsUserOnline(UserInfo user);

        bool LockUser(Guid userId);

        bool LockUsers(Guid[] userIds);

        bool UnLockUser(Guid userId);

        bool UnLockUsers(Guid[] userIds);

        bool UpdateUserExInfo(UserInfo user);

        /// <summary>
        /// 验证用户
        /// </summary>
        UserInfo ValidUser(string account,ValidationType type);
    }
}
