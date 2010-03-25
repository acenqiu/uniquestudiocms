using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 插件的集合
    /// </summary>
    public class PlugInCollection:List<PlugInInfo>
    {
        /// <summary>
        /// 初始化<see cref="PlugInCollection"/>类的实例
        /// </summary>
        public PlugInCollection()
            : base()
        {
        }
    }
}
