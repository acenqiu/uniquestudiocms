//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：模块控件管理提供类需实现的方法。
// 完成日期：2010年03月29日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 模块控件管理提供类需实现的方法。
    /// </summary>
    public interface IModuleControl
    {
        /// <summary>
        /// 创建控件。
        /// </summary>
        /// <param name="moduleControl">控件信息。</param>
        /// <returns>是否创建成功。</returns>
        bool CreateModuleControl(ModuleControlInfo control);

        /// <summary>
        /// 删除指定控件。
        /// </summary>
        /// <param name="controlId">控件ID。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteModuleControl(int controlId);

        /// <summary>
        /// 删除多个控件。
        /// </summary>
        /// <param name="controlIds">待删除控件ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteModuleControls(int[] controlIds);

        /// <summary>
        /// 返回指定网站下的所有控件。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>控件的集合。</returns>
        ModuleControlCollection GetAllModuleControls(int siteId);

        /// <summary>
        /// 返回指定控件。
        /// </summary>
        /// <param name="controlId">待获取控件ID。</param>
        /// <returns>控件信息，如果不存在返回空。</returns>
        ModuleControlInfo GetModuleControl(int controlId);

        /// <summary>
        /// 返回指定控件。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">待获取控件名称。</param>
        /// <returns>控件信息，如果不存在返回空。</returns>
        ModuleControlInfo GetModuleControl(int siteId, string controlName);

        /// <summary>
        /// 返回指定控件是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">控件名称。</param>
        /// <returns>是否存在。</returns>
        bool IsModuleControlExists(int siteId, string controlName);

        /// <summary>
        /// 设置指定控件的状态。
        /// </summary>
        /// <param name="controlId">控件ID。</param>
        /// <param name="isEnabled">是否启用。</param>
        /// <returns>是否更新成功。</returns>
        bool SetModuleControlStatus(int controlId, bool isEnabled);

        /// <summary>
        /// 设置多个控件的状态。
        /// </summary>
        /// <param name="controlIds">待设置控件的集合。</param>
        /// <param name="isEnabled">是否启用。</param>
        /// <returns>是否更新成功。</returns>
        bool SetModuleControlStatus(int[] controlIds, bool isEnabled);

        /// <summary>
        /// 更新控件信息。
        /// </summary>
        /// <param name="control">待更新控件信息。</param>
        /// <returns>是否更新成功。</returns>
        bool UpdateModuleControl(ModuleControlInfo control);
    }
}
