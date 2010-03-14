using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 未处理的异常
    /// </summary>
    public class UnhandledException:Exception
    {
        /// <summary>
        /// 初始化<see cref="UnhandledException"/>类的实例
        /// </summary>
        public UnhandledException()
            :base("程序遇到了未知的异常，请重试！")
        {

        }

        /// <summary>
        /// 以错误信息初始化<see cref="UnhandledException"/>类的实例
        /// </summary>
        /// <param name="message"></param>
        public UnhandledException(string message)
            :base(message)
        {

        }
    }
}
