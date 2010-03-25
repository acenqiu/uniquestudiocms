using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UniqueStudio.Common.FileAccessHelper
{
    /// <summary>
    /// 提供文件访问的方法
    /// </summary>
    public class FileAccess
    {
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件内容</returns>
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.Default);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="encoding">文件编码</param>
        /// <returns>文件内容</returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("请指定文件完整路径。");
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的文件不存在。");
            }
            StreamReader reader = new StreamReader(filePath, encoding, true);
            string content = string.Empty;
            try
            {
                content = reader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            return content;
        }
    }
}
