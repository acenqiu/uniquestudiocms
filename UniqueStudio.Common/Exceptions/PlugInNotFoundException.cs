//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：插件不存在异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

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
        /// 以插件名称初始化<see cref="PlugInNotFoundException"/>类的实例。
        /// </summary>
        /// <param name="plugInName">插件名称。</param>
        public PlugInNotFoundException(string plugInName)
            : base(string.Format("以下插件不存在：{0}。", plugInName))
        {

        }
    }
}
