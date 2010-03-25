using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 用户没有登录异常
    /// </summary>
    public class UserDoesNotOnlineException:Exception
    {
        /// <summary>
        /// 初始化<see cref="UserDoesNotOnlineException"/>类的实例
        /// </summary>
        public UserDoesNotOnlineException()
        {
        }

        /// <summary>
        /// 以错误信息初始化<see cref="UserDoesNotOnlineException"/>类的实例
        /// </summary>
        /// <param name="message">错误信息</param>
        public UserDoesNotOnlineException(string message)
            :base(message)
        {
        }
    }
}
