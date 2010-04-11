//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：用户没有登录异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 用户没有登录异常。
    /// </summary>
    public class UserDoesNotOnlineException : Exception
    {
        /// <summary>
        /// 初始化<see cref="UserDoesNotOnlineException"/>类的实例。
        /// </summary>
        public UserDoesNotOnlineException()
            : base("该用户没有登录。")
        {

        }

        /// <summary>
        /// 以用户名初始化<see cref="UserDoesNotOnlineException"/>类的实例。
        /// </summary>
        /// <param name="userName">用户名。</param>
        public UserDoesNotOnlineException(string userName)
            : base(string.Format("以下用户没有登录：{0}。", userName))
        {

        }
    }
}
