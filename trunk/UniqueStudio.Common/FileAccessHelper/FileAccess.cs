using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UniqueStudio.Common.FileAccessHelper
{
    public class FileAccess
    {
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.Default);
        }

        public static string ReadFile(string filePath, Encoding encoding)
        {
            if (filePath.Trim().Length == 0)
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

        public static void WriteFile(string fileFullPath, string fileContent)
        {
            //文件路径分解
            throw new NotImplementedException();
        }

        public static void WriteFile(string fileDirectory, string fileName, string fileContent)
        {
            WriteFile(fileDirectory, fileName, fileContent, Encoding.UTF8);
        }

        public static void WriteFile(string fileDirectory, string fileName, string fileContent, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}
