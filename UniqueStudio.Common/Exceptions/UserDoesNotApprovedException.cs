//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：用户未激活异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 用户未激活异常。
    /// </summary>
    public class UserDoesNotApprovedException : Exception
    {
        /// <summary>
        /// 初始化<see cref="UserDoesNotApprovedException"/>类的实例。
        /// </summary>
        public UserDoesNotApprovedException()
            : base("该用户没有激活！")
        {

        }

        /// <summary>
        /// 以用户名初始化<see cref="UserDoesNotApprovedException"/>类的实例。
        /// </summary>
        /// <param name="userName">用户名</param>
        public UserDoesNotApprovedException(string userName)
            : base(string.Format("以下用户没有激活： {0} 。", userName))
        {

        }
    }
}
