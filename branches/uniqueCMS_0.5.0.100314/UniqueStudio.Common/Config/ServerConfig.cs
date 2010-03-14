using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 系统服务器配置
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
    }
}
