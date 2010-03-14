using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 模块控件管理提供类需实现的方法
    /// </summary>
    public interface IModuleControl
    {
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="moduleControl">控件信息</param>
        /// <returns>如果创建成功，返回控件信息，否则返回空</returns>
        ModuleControlInfo CreateModuleControl(ModuleControlInfo moduleControl);

        /// <summary>
        /// 返回所有控件
        /// </summary>
        /// <returns>控件的集合</returns>
        ModuleControlCollection GetAllModuleControls();

        /// <summary>
        /// 返回指定控件
        /// </summary>
        /// <param name="controlId">待获取控件ID</param>
        /// <returns>控件信息，如果不存在返回空</returns>
        ModuleControlInfo GetModuleControl(string controlId);

        /// <summary>
        /// 更新控件配置信息
        /// </summary>
        /// <remarks>该方法可能在后续版本中与UpdateModuleControl方法合并</remarks>
        /// <param name="controlId">待更新控件ID</param>
        /// <param name="parameters">控件配置信息（xml格式）</param>
        /// <returns>是否更新成功</returns>
        bool UpdateControlParameters(string controlId, string parameters);
    }
}
