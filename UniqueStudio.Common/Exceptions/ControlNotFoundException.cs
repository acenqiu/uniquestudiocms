using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 控件不存在异常。
    /// </summary>
    public class ControlNotFoundException : Exception
    {
        /// <summary>
        /// 初始化<see cref="ControlNotFoundException"/>类的实例。
        /// </summary>
        public ControlNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// 以错误信息初始化<see cref="ControlNotFoundException"/>类的实例。
        /// </summary>
        /// <param name="message">错误信息。</param>
        public ControlNotFoundException(string message)
            : base(message)
        {
        }
    }
}
