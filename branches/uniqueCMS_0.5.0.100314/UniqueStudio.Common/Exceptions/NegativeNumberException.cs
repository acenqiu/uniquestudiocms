using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 不能为负数异常
    /// </summary>
    public class NegativeNumberException:ArithmeticException
    {
        /// <summary>
        /// 初始化<see cref="NegativeNumberException"/>类的实例
        /// </summary>
        /// <remarks>默认错误信息为：该参数不能为负。</remarks>
        public NegativeNumberException()
            : base("该参数不能为负。")
        {

        }

        /// <summary>
        /// 以参数名初始化<see cref="NegativeNumberException"/>类的实例
        /// </summary>
        /// <remarks>默认错误信息为：以下参数不能为负：{paramName}。</remarks>
        /// <param name="paramName">参数名</param>
        public NegativeNumberException(string paramName)
            :base(string.Format("以下参数不能为负：{0}。",paramName))
        {

        }
    }
}
