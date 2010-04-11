//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：插件集合。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System.Collections.Generic;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 插件集合。
    /// </summary>
    public class PlugInCollection:List<PlugInInfo>
    {
        /// <summary>
        /// 初始化<see cref="PlugInCollection"/>类的实例。
        /// </summary>
        public PlugInCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="PlugInCollection"/>类的实例
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public PlugInCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
