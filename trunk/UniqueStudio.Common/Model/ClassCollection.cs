//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：类的集合。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 类的集合。
    /// </summary>
    [Serializable]
    public class ClassCollection : List<ClassInfo>
    {
        /// <summary>
        /// 初始化<see cref="ClassCollection"/>类的实例。
        /// </summary>
        public ClassCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合初始容量初始化<see cref="ClassCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public ClassCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
