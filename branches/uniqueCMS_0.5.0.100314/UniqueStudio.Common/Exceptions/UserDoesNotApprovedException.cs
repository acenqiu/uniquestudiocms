using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 用户未激活异常
    /// </summary>
    public class UserDoesNotApprovedException : Exception
    {
        /// <summary>
        /// 初始化<see cref="UserDoesNotApprovedException"/>类的实例
        /// </summary>
        public UserDoesNotApprovedException()
            : base("该用户没有激活！")
        {

        }

        /// <summary>
        /// 以用户名初始化<see cref="UserDoesNotApprovedException"/>类的实例
        /// </summary>
        /// <param name="userName">用户名</param>
        public UserDoesNotApprovedException(string userName)
            : base(string.Format("用户 {0} 没有激活！", userName))
        {

        }
    }
}
