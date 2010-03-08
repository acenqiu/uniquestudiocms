using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.Core.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserArgs : EventArgs
    {
        private UserInfo user;

        public UserArgs()
        {
        }

        public UserArgs(UserInfo user)
        {
            this.user = user;
        }

        public UserInfo User
        {
            get { return user; }
            set { user = value; }
        }
    }
}
