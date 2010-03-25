using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 用户已被锁定异常
    /// </summary>
    public class UserLockedOutException : Exception
    {
        /// <summary>
        /// 初始化<see cref="UserLockedOutException"/>类的实例
        /// </summary>
        public UserLockedOutException()
            : base("该用户处于锁定状态！")
        {
        }

        /// <summary>
        /// 以用户名初始化<see cref="UserLockedOutException"/>类的实例
        /// </summary>
        /// <param name="userName">用户名</param>
        public UserLockedOutException(string userName)
            :base(string.Format("用户 {0} 处于锁定状态！",userName))
        {
        }
    }
}
