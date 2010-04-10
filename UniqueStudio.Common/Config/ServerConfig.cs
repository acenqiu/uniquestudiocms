//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：系统服务器配置。
// 完成日期：2010年04月10日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 系统服务器配置。
    /// </summary>
    public class ServerConfig : SystemConfig
    {
        /// <summary>
        /// 初始化<see cref="ServerConfig"/>类的实例。
        /// </summary>
        public ServerConfig()
        {
            path = @"admin\xml\ServerConfig.xml";
        }

        private static string baseAddress = string.Empty;
        private static bool isDisplayTime = true;

        /// <summary>
        /// 网站首页地址。
        /// </summary>
        public static string BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }

        /// <summary>
        /// 是否在后台显示时间。
        /// </summary>
        public static bool IsDisplayTime
        {
            get { return isDisplayTime; }
            set { isDisplayTime = value; }
        }
    }
}
