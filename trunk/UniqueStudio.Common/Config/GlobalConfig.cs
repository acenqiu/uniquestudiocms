using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.Common.Config
{
    public class GlobalConfig
    {
        public static string SqlConnectionString = ConfigurationSettings.AppSettings["ConnectionString"];
        
        public static PasswordEncryptionType DefaultPasswordEncryption = PasswordEncryptionType.Hashed;
        public static bool DefaultIsApproved = true;
        public static bool EnableUrlRewrite = false;
        public static bool EnablePermissionCheck = true;
        public static string BasePhysicalPath = string.Empty;
        public static int CacheCapacity = 100;

        public static string CACHE_CURRENT_USER = "CurrentUser";

        public const string COOKIE = "US";
        public const string COOKIE_EMAIL = "UEmail";
        public const string COOKIE_PASSWORD = "UPassword";
        public const string COOKIE_AUTOLOGIN = "AutoLogin";
        public const string SESSION_NAME = "UName";
        public const string SESSION_ID = "UID";
        public const string SESSION_USER = "User";
    }
}
