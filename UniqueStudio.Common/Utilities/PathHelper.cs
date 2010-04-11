//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供对Url进行处理的一些辅助方法。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
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
        /// <summary>
        /// 连接两个路径，获得一个完整的路径。
        /// </summary>
        /// <param name="path1">路径1。</param>
        /// <param name="path2">路径2。</param>
        /// <returns>完整路径。</returns>
        public static string PathCombine(string path1, string path2)
        {
            Validator.CheckStringNull(path1, "path1");
            Validator.CheckStringNull(path2, "path2");

            path1 = path1.TrimEnd(new char[] { '/', '\\' });
            path2 = path2.TrimStart(new char[] { '/', '\\' });
            return path1 + "/" + path2;
        }

        /// <summary>
        /// 移除QueryString的前导问号及指定键。
        /// </summary>
        /// <param name="query">QueryString。</param>
        /// <param name="removeKeys">需移除的键。</param>
        /// <returns>经处理的QueryString。</returns>
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

        /// <summary>
        /// 将指定的QueryString分离成键值对的集合。
        /// </summary>
        /// <param name="queryString">QueryString。</param>
        /// <returns>键值对的集合。</returns>
        public static Dictionary<string, string> SplitToKeyValuePairs(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                return null;
            }

            queryString = queryString.TrimStart(new char[] { '?' });
            string[] keyValues = queryString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>(keyValues.Length);
            for (int i = 0; i < keyValues.Length; i++)
            {
                string[] elements = keyValues[i].Split(new char[] { '=' });
                if (elements.Length == 2)
                {
                    keyValuePairs.Add(elements[0], elements[1]);
                }
            }
            return keyValuePairs;
        }
    }
}