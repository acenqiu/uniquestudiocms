using System;
using System.Security.Cryptography;
using System.Text;

namespace UniqueStudio.Common.RESTful
{
    /// <summary>
    /// 哈希辅助类
    /// </summary>
    /// <remarks>该类可能与<see cref="MD5Helper"/>类整合</remarks>
    public static class Hashing
    {
        /// <summary>
        /// 指定字节流编码计算MD5哈希值，可解决不同系统中文编码差异的问题。
        /// </summary>
        /// <param name="source">要进行哈希的字符串</param>
        /// <param name="bytesEncoding">获取字符串字节流的编码，如果是中文，不同系统之间务必请使用相同编码</param>
        /// <returns>32位大写MD5哈希值</returns>
        public static string ComputeMD5(string source, Encoding bytesEncoding)
        {
            byte[] sourceBytes = bytesEncoding.GetBytes(source);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashedBytes = md5.ComputeHash(sourceBytes);
            StringBuilder buffer = new StringBuilder(hashedBytes.Length);
            foreach (byte item in hashedBytes)
            {
                buffer.AppendFormat("{0:x2}", item);
            }

            return buffer.ToString();
        }

        /// <summary>
        /// 计算UTF-8编码的编码字符串的MD5哈希结果
        /// </summary>
        /// <param name="source">输入字符串</param>
        /// <returns>32位大写MD5哈希结果</returns>
        public static string ComputeMD5(string source)
        {
            return ComputeMD5(source, Encoding.UTF8);
        }
    }
}