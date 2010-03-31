//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：模块管理提供类需实现的方法。
// 完成日期：2010年03月31日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 模块管理提供类需实现的方法。
    /// </summary>
    internal interface IModule
    {
        /// <summary>
        /// 创建模块。
        /// </summary>
        /// <param name="module">模块信息。</param>
        /// <returns>如果创建成功，返回模块信息，否则返回空。</returns>
        ModuleInfo CreateModule(ModuleInfo module);

        /// <summary>
        /// 删除指定模块。
        /// </summary>
        /// <param name="moduleId">待删除模块ID。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteModule(int moduleId);

        /// <summary>
        /// 删除多个模块。
        /// </summary>
        /// <param name="moduleIds">待删除模块ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteModules(int[] moduleIds);

        /// <summary>
        /// 返回所有模块。
        /// </summary>
        /// <returns>模块的集合。</returns>
        ModuleCollection GetAllModules();

        /// <summary>
        /// 返回指定模块。
        /// </summary>
        /// <remarks>仅返回ClassPath,Assembly。</remarks>
        /// <param name="moduleId">模块ID。</param>
        /// <returns>模块信息，如果获取失败，返回空。</returns>
        ModuleInfo GetModule(int moduleId);

        /// <summary>
        /// 返回指定模块。
        /// </summary>
        /// <remarks>仅返回ClassPath,Assembly。</remarks>
        /// <param name="moduleName">模块名称。</param>
        /// <returns>模块信息，如果获取失败，返回空。</returns>
        ModuleInfo GetModule(string moduleName);

        /// <summary>
        /// 返回模块名称。
        /// </summary>
        /// <param name="moduleId">模块ID。</param>
        /// <returns>模块名称。</returns>
        string GetModuleName(int moduleId);

        /// <summary>
        /// 返回指定模块是否存在。
        /// </summary>
        /// <param name="moduleName">模块名。</param>
        /// <returns>是否存在。</returns>
        bool IsModuleExists(string moduleName);
    }
}
