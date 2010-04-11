//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：组件集合。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 组件集合。
    /// </summary>
    [Serializable]
    public class CompenentCollection:List<CompenentInfo>
    {
        /// <summary>
        /// 初始化<see cref="CompenentCollection"/>类的实例。
        /// </summary>
        public CompenentCollection()
            : base()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以集合容量初始化<see cref="CompenentCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public CompenentCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
