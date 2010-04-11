//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：非正数异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 非正数异常。
    /// </summary>
    public class NotPositiveNumberException : ArithmeticException
    {
        /// <summary>
        /// 初始化<see cref="NotPositiveNumberException"/>类的实例。
        /// </summary>
        /// <remarks>默认错误信息为：该参数不是正数。</remarks>
        public NotPositiveNumberException()
            : base("该参数不是正数。")
        {

        }

        /// <summary>
        /// 以参数名初始化<see cref="NotPositiveNumberException"/>类的实例。
        /// </summary>
        /// <remarks>以下参数非正：{paramName}。</remarks>
        /// <param name="paramName">参数名。</param>
        public NotPositiveNumberException(string paramName)
            : base(string.Format("以下参数非正：{0}。", paramName))
        {

        }
    }
}
