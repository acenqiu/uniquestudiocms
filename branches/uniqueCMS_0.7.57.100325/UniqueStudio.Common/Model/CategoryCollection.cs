//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：分类集合。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 分类集合。
    /// </summary>
    [Serializable]
    public class CategoryCollection : List<CategoryInfo>
    {
        /// <summary>
        /// 初始化<see cref="CategoryCollection"/>类的实例。
        /// </summary>
        public CategoryCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合初始容量初始化<see cref="CategoryCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public CategoryCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// 将各个分类以空格分隔的形式输出分类名。
        /// </summary>
        /// <remarks>可能在后续版本中移除。</remarks>
        /// <returns>以空格分隔的分类名。</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (CategoryInfo item in this)
            {
                sb.Append(item.CategoryName + " ");
            }
            return sb.ToString();
        }
    }
}
