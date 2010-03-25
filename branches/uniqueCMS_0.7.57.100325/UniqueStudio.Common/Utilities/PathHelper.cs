using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Utilities
{
    /// <summary>
    /// 提供对Url进行处理的一些辅助方法。
    /// </summary>
    public class PathHelper
    {
        public static string PathCombine(string path1, string path2)
        {
            path1 = path1.TrimEnd(new char[] { '/', '\\' });
            path2 = path2.TrimStart(new char[] { '/', '\\' });
            return path1 + "/" + path2;
        }

        public static string CleanUrlQueryString(string url, params string[] removeKeys)
        {
            Uri uri = new Uri(url);
            return url;
        }
    }
}
