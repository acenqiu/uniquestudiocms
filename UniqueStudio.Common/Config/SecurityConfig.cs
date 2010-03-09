using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 系统安全配置
    /// </summary>
    public class SecurityConfig:SystemConfig
    {
        /// <summary>
        /// 初始化<see cref="SecurityConfig"/>类的实例。
        /// </summary>
        public SecurityConfig()
        {
            path = @"admin\xml\SecurityConfig.xml";
        }
    }
}
