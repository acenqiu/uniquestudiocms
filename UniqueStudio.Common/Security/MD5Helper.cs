using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UniqueStudio.Common.Security
{
    public class MD5Helper
    {
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
