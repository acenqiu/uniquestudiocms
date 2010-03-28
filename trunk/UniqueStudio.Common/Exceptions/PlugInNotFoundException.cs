using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 插件不存在异常。
    /// </summary>
    public class PlugInNotFoundException : Exception
    {
        /// <summary>
        /// 初始化<see cref="PlugInNotFoundException"/>类的实例。
        /// </summary>
        public PlugInNotFoundException()
            : base()
        {

        }

        /// <summary>
        /// 以错误信息初始化<see cref="PlugInNotFoundException"/>类的实例。
        /// </summary>
        /// <param name="message">错误信息。</param>
        public PlugInNotFoundException(string message)
            : base(message)
        {

        }
    }
}
