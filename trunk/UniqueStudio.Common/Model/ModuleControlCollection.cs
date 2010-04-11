//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：模块控件的集合。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 模块控件的集合。
    /// </summary>
    [Serializable]
    public class ModuleControlCollection:List<ModuleControlInfo>
    {
        /// <summary>
        /// 初始化<see cref="ModuleControlCollection"/>类的实例。
        /// </summary>
        public ModuleControlCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="ModuleControlCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public ModuleControlCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
