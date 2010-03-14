using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 模块控件的集合
    /// </summary>
    [Serializable]
    public class ModuleControlCollection:List<ModuleControlInfo>
    {
        /// <summary>
        /// 初始化<see cref="ModuleControlCollection"/>类的实例
        /// </summary>
        public ModuleControlCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="ModuleControlCollection"/>类的实例
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public ModuleControlCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
