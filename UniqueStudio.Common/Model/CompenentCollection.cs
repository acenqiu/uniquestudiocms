using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 组件集合
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
    }
}
