using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 用户密码加密方式
    /// </summary>
    public enum PasswordEncryptionType
    {
        /// <summary>
        /// 使用明文
        /// </summary>
        Clear,       

        /// <summary>
        /// 存储前加密密码
        /// </summary>
        Encrypted,  

        /// <summary>
        /// 存储密码Hash值
        /// </summary>
        Hashed  
    }
}
