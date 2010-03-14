using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Core.PlugIn
{
    /// <summary>
    /// 插件需实现的接口
    /// </summary>
    public interface IPlugIn
    {
        /// <summary>
        /// 初始化该接口，注册相应的事件
        /// </summary>
        void Init();
    }
}
