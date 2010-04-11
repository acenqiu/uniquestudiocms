using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.RESTful
{
    /// <summary>
    /// 集合辅助类。
    /// </summary>
    public static class CollectionUtil
    {
        /// <summary>
        /// 将字符串字典里的数据转换成URL查询字符串格式。
        /// </summary>
        /// <param name="dict">字符串字典。</param>
        /// <returns>url查询字符串。</returns>
        public static string ToQueryString(IDictionary<string, string> dict)
        {
            if (dict.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder buffer = new StringBuilder();
            int count = 0;

            foreach (string key in dict.Keys)
            {
                if (count == dict.Count - 1)
                {
                    buffer.AppendFormat("{0}={1}", key, dict[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}&", key, dict[key]);
                }
                count++;
            }

            return buffer.ToString();
        }

        /// <summary>
        /// 按字符串字典里的数据转换为以字母升序且以键值对形式存储的字符串。
        /// </summary>
        /// <remarks>格式为：key1=value1key2=value2。</remarks>
        /// <param name="dict">字符串字典。</param>
        /// <returns>对应字符串。</returns>
        public static string ToSortedString(IDictionary<string, string> dict)
        {
            SortedDictionary<string, string> sortedDict = new SortedDictionary<string, string>(dict);

            if (sortedDict.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder buffer = new StringBuilder();

            foreach (string key in sortedDict.Keys)
            {
                buffer.AppendFormat("{0}={1}", key, sortedDict[key]);
            }

            return buffer.ToString();
        }

        /// <summary>
        /// 将列表转换为逗号分隔的字符串。
        /// </summary>
        /// <param name="collection">列表。</param>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <returns>逗号分隔的字符串。</returns>
        public static string ToCommaString<T>(IList<T> collection)
        {
            if (collection.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < collection.Count; i++)
            {
                if (i == collection.Count - 1)
                {
                    buffer.Append(collection[i]);
                }
                else
                {
                    buffer.AppendFormat("{0},", collection[i]);
                }
            }

            return buffer.ToString();
        }
    }
}