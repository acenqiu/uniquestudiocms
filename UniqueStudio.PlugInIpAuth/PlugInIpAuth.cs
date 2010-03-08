using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Core.PlugIn;
using UniqueStudio.Core.User;

namespace UniqueStudio.PlugInIpAuth
{
    internal class PlugInIpAuth : IPlugIn
    {
        public void Init()
        {
            //throw new Exception();
            //UserManager.OnUserCreated += new UserManager.UserCreatedHandler(UserManager_OnUserCreated);
        }

        void UserManager_OnUserCreated(object sende, UserArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
