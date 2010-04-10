//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：全局配置类。
// 完成日期：2010年04月10日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System.Configuration;

using UniqueStudio.Common.Model;

namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 全局配置类。
    /// </summary>
    public class GlobalConfig
    {
        /// <summary>
        /// Sql数据库连接字符串
        /// </summary>
        public static string SqlConnectionString = ConfigurationSettings.AppSettings["ConnectionString"];
        /// <summary>
        /// 默认密码加密方式。
        /// </summary>
        public static PasswordEncryptionType DefaultPasswordEncryption = PasswordEncryptionType.Hashed;
        /// <summary>
        /// 是否启用url重写功能。
        /// </summary>
        public static bool EnableUrlRewrite = false;
        /// <summary>
        /// 网站根目录物理路径。
        /// </summary>
        public static string BasePhysicalPath = string.Empty;
        /// <summary>
        /// 缓存容量。
        /// </summary>
        public static int CacheCapacity = 100;

        //Cache键
        public const string CACHE_SITES = "Sites";

        //Cookie键
        public const string COOKIE = "US";
        public const string COOKIE_EMAIL = "UAccount";
        public const string COOKIE_PASSWORD = "UPassword";
        public const string COOKIE_AUTOLOGIN = "AutoLogin";
        public const string COOKIE_SITEID = "SiteId";

        //Session键
        public const string SESSION_USER = "User";
        public const string SESSION_SITEID = "SiteId";
    }
}
