using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.Core.User
{
    /// <summary>
    /// 为用户事件提供参数
    /// </summary>
    public class UserEventArgs : EventArgs
    {
        private UserInfo user;

        /// <summary>
        /// 初始化<see cref="UserEventArgs"/>的实例
        /// </summary>
        public UserEventArgs()
        {
        }

        /// <summary>
        /// 以用户信息初始化<see cref="UserEventArgs"/>的实例
        /// </summary>
        /// <param name="user">用户信息</param>
        public UserEventArgs(UserInfo user)
        {
            this.user = user;
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo User
        {
            get { return user; }
            set { user = value; }
        }
    }
}
