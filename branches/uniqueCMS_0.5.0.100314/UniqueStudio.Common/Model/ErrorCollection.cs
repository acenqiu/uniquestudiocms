using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 系统异常的集合
    /// </summary>
    [Serializable]
    public class ErrorCollection:List<ErrorInfo>
    {
        /// <summary>
        /// 初始化<see cref="ErrorCollection"/>类的实例
        /// </summary>
        public ErrorCollection()
            :base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="ErrorCollection"/>类的实例
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public ErrorCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
