//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：用户已被锁定异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 用户已被锁定异常。
    /// </summary>
    public class UserLockedOutException : Exception
    {
        /// <summary>
        /// 初始化<see cref="UserLockedOutException"/>类的实例。
        /// </summary>
        public UserLockedOutException()
            : base("该用户处于锁定状态。")
        {

        }

        /// <summary>
        /// 以用户名初始化<see cref="UserLockedOutException"/>类的实例。
        /// </summary>
        /// <param name="userName">用户名。</param>
        public UserLockedOutException(string userName)
            : base(string.Format("以下用户处于锁定状态：{0} 。", userName))
        {

        }
    }
}
