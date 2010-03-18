//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：为用户事件提供参数。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.Core.User
{
    /// <summary>
    /// 为用户事件提供参数。
    /// </summary>
    public class UserEventArgs : EventArgs
    {
        private UserInfo user;

        /// <summary>
        /// 初始化<see cref="UserEventArgs"/>的实例。
        /// </summary>
        public UserEventArgs()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以用户信息初始化<see cref="UserEventArgs"/>的实例。
        /// </summary>
        /// <param name="user">用户信息。</param>
        public UserEventArgs(UserInfo user)
        {
            this.user = user;
        }

        /// <summary>
        /// 用户信息。
        /// </summary>
        public UserInfo User
        {
            get { return user; }
            set { user = value; }
        }
    }
}
