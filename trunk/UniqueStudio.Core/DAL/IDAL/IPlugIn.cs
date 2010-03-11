using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 插件管理提供类需实现的方法
    /// </summary>
    internal interface IPlugIn
    {
        /// <summary>
        /// 创建插件
        /// </summary>
        /// <param name="plugIn">待安装插件信息</param>
        /// <returns>如果安装成功返回插件信息，否则返回空</returns>
        PlugInInfo CreatePlugIn(PlugInInfo plugIn);

        /// <summary>
        /// 删除插件
        /// </summary>
        /// <param name="plugInId">待删除插件ID</param>
        /// <returns>是否删除成功</returns>
        bool DeletePlugIn(int plugInId);

        /// <summary>
        /// 返回插件列表
        /// </summary>
        /// <returns>插件列表</returns>
        PlugInCollection GetAllPlugIns();

        /// <summary>
        /// 返回插件列表
        /// </summary>
        /// <remarks>该方法只在程序系统启动时调用，用于完成插件的初始化工作</remarks>
        /// <returns>类集合</returns>
        ClassCollection GetAllPlugInsForInit();
    }
}
