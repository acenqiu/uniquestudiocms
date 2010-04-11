//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：控件不存在异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
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
        /// 以控件名称初始化<see cref="ControlNotFoundException"/>类的实例。
        /// </summary>
        /// <param name="controlName">控件名称。</param>
        public ControlNotFoundException(string controlName)
            : base(string.Format("以下控件不存在：{0}。", controlName))
        {

        }
    }
}
