//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：未处理的异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 未处理的异常。
    /// </summary>
    public class UnhandledException : Exception
    {
        /// <summary>
        /// 初始化<see cref="UnhandledException"/>类的实例。
        /// </summary>
        public UnhandledException()
            : base("程序遇到了未知的异常，请重试！")
        {

        }

        /// <summary>
        /// 以错误信息初始化<see cref="UnhandledException"/>类的实例。
        /// </summary>
        /// <param name="message">错误信息。</param>
        public UnhandledException(string message)
            : base(message)
        {

        }
    }
}
