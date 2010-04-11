//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供文件操作的方法。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System.IO;
using System.Text;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Common.FileAccessHelper
{
    /// <summary>
    /// 提供文件操作的方法。
    /// </summary>
    public class FileAccess
    {
        /// <summary>
        /// 读取文件。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <returns>文件内容。</returns>
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.Default);
        }

        /// <summary>
        /// 读取文件。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <param name="encoding">文件编码。</param>
        /// <returns>文件内容。</returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            Validator.CheckStringNull(filePath, "filePath");

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
