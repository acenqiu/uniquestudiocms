using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UniqueStudio.Common.Security
{
    /// <summary>
    /// 提供MD5加密的方法
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="md5">待加密字符串</param>
        /// <returns>MD5散列值</returns>
        public static string MD5Encrypt(string md5)
        {
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
