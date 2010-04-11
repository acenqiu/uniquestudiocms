//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：不能为负数异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 不能为负数异常。
    /// </summary>
    public class NegativeNumberException:ArithmeticException
    {
        /// <summary>
        /// 初始化<see cref="NegativeNumberException"/>类的实例。
        /// </summary>
        /// <remarks>默认错误信息为：该参数不能为负。</remarks>
        public NegativeNumberException()
            : base("该参数不能为负。")
        {

        }

        /// <summary>
        /// 以参数名初始化<see cref="NegativeNumberException"/>类的实例。
        /// </summary>
        /// <remarks>默认错误信息为：以下参数不能为负：{paramName}。</remarks>
        /// <param name="paramName">参数名。</param>
        public NegativeNumberException(string paramName)
            :base(string.Format("以下参数不能为负：{0}。",paramName))
        {

        }
    }
}
