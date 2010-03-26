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

        public static string CleanUrlQueryString(string query, params string[] removeKeys)
        {
            if (string.IsNullOrEmpty(query))
            {
                return query;
            }

            query = query.Trim(new char[] { '?' });
            if (string.IsNullOrEmpty(query) || removeKeys == null || removeKeys.Length == 0)
            {
                return query;
            }

            //移除多余键
            string[] keyValues = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            int i;
            foreach (string keyValue in keyValues)
            {
                for (i = 0; i < removeKeys.Length; i++)
                {
                    if (keyValue.StartsWith(removeKeys[i] + "="))
                    {
                        break;
                    }
                }
                if (i == removeKeys.Length)
                {
                    sb.Append(keyValue).Append("&");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}