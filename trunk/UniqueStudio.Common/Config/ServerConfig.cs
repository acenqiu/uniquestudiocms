using System;
using System.Collections.Generic;
using System.Text;

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


        /// <summary>
        /// 网站首页地址。
        /// </summary>
        public static string BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }
    }
}
