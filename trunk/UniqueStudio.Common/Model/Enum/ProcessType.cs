using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 页面处理方式
    /// </summary>
    public enum ProcessType
    {
        /// <summary>
        /// 使用模板引擎解析
        /// </summary>
        ByEngine,

        /// <summary>
        /// 使用asp.net解析
        /// </summary>
        ByAspnet
    }
}
