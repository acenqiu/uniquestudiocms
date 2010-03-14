using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 全局配置类
    /// </summary>
    /// <remarks>修改该类的值需重新编译</remarks>
    public class GlobalConfig
    {
        /// <summary>
        /// Sql数据库连接字符串
        /// </summary>
        public static string SqlConnectionString = ConfigurationSettings.AppSettings["ConnectionString"];
        /// <summary>
        /// 默认密码加密方式
        /// </summary>
        public static PasswordEncryptionType DefaultPasswordEncryption = PasswordEncryptionType.Hashed;
        /// <summary>
        /// 是否启用url重写功能
        /// </summary>
        public static bool EnableUrlRewrite = false;
        /// <summary>
        /// 是否启用权限检查
        /// </summary>
        public static bool EnablePermissionCheck = true;
        /// <summary>
        /// 网站根目录物理路径
        /// </summary>
        public static string BasePhysicalPath = string.Empty;
        /// <summary>
        /// 缓存容量
        /// </summary>
        public static int CacheCapacity = 100;

        //Cookie键
        public const string COOKIE = "US";
        public const string COOKIE_EMAIL = "UAccount";
        public const string COOKIE_PASSWORD = "UPassword";
        public const string COOKIE_AUTOLOGIN = "AutoLogin";

        //Session键
        public const string SESSION_USER = "User";
    }
}
