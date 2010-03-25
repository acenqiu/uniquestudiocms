using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 模块管理提供类需实现的方法
    /// </summary>
    internal interface IModule
    {
        /// <summary>
        /// 返回所有模块
        /// </summary>
        /// <returns>模块的集合</returns>
        ModuleCollection GetAllModules();

        /// <summary>
        /// 返回指定模块
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns>模块信息，如果获取失败，返回空</returns>
        ModuleInfo GetModule(int moduleId);

        /// <summary>
        /// 创建模块
        /// </summary>
        /// <param name="module">模块信息</param>
        /// <returns>如果创建成功，返回模块信息，否则返回空</returns>
        ModuleInfo CreateModule(ModuleInfo module);
    }
}
