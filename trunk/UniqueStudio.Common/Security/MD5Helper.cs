//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供MD5加密的方法。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Security.Cryptography;
using System.Text;

using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Common.Security
{
    /// <summary>
    /// 提供MD5加密的方法。
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// MD5加密。
        /// </summary>
        /// <param name="md5">待加密字符串。</param>
        /// <returns>MD5散列值。</returns>
        public static string MD5Encrypt(string md5)
        {
            Validator.CheckStringNull(md5, "md5");

            MD5CryptoServiceProvider MyMD5 = new MD5CryptoServiceProvider();
            try
            {
                Byte[] MyMD5_Str = MyMD5.ComputeHash(Encoding.Unicode.GetBytes(md5));
                string byte2String = null;
                for (int i = 0; i < MyMD5_Str.Length; i++)
                {
                    byte2String += MyMD5_Str[i].ToString("x");
                }
                return byte2String;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
