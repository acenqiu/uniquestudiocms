﻿using System;
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

        //Boolean
        private static bool enableLoginByUserName = true;
        private static bool enableRegister = false;
        private static bool isApprovedAfterRegister = false;

        //Boolean

        /// <summary>
        /// 是否启用用户名方式登录
        /// </summary>
        public static bool EnableLoginByUserName
        {
            get { return enableLoginByUserName; }
            set { enableLoginByUserName = value; }
        }
        /// <summary>
        /// 是否开放注册
        /// </summary>
        public static bool EnableRegister
        {
            get { return enableRegister; }
            set { enableRegister = value; }
        }
        /// <summary>
        /// 外部注册后是否处于激活状态
        /// </summary>
        public static bool IsApprovedAfterRegister
        {
            get { return isApprovedAfterRegister; }
            set { isApprovedAfterRegister = value; }
        }
    }
}
