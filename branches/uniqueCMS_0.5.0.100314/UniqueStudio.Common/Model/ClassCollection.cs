using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 类的集合
    /// </summary>
    [Serializable]
    public class ClassCollection:List<ClassInfo>
    {
        /// <summary>
        /// 初始化<see cref="ClassCollection"/>类的实例
        /// </summary>
        public ClassCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合初始容量初始化<see cref="ClassCollection"/>类的实例
        /// </summary>
        /// <param name="capacity"></param>
        public ClassCollection(int capacity)
            :base(capacity)
        {
        }
    }
}
