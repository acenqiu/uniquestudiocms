//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：用户密码加密方式。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 用户密码加密方式。
    /// </summary>
    public enum PasswordEncryptionType
    {
        /// <summary>
        /// 使用明文。
        /// </summary>
        Clear,       

        /// <summary>
        /// 存储前加密密码。
        /// </summary>
        Encrypted,  

        /// <summary>
        /// 存储密码Hash值。
        /// </summary>
        Hashed  
    }
}
