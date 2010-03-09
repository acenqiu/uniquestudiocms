using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 模块集合
    /// </summary>
    [Serializable]
    public class ModuleCollection : List<ModuleInfo>
    {
        /// <summary>
        /// 初始化<see cref="ModuleCollection"/>类的实例
        /// </summary>
        public ModuleCollection()
            : base()
        {
        }
    }
}
