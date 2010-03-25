using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 插件管理提供类需实现的方法。
    /// </summary>
    internal interface IPlugIn
    {
        /// <summary>
        /// 新增插件实例。
        /// </summary>
        /// <param name="plugInId">插件ID。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否增加成功。</returns>
        bool AddPlugInInstance(int plugInId, int siteId);

        /// <summary>
        /// 创建插件。
        /// </summary>
        /// <param name="plugIn">待安装插件信息。</param>
        /// <returns>如果安装成功返回插件信息，否则返回空。</returns>
        PlugInInfo CreatePlugIn(PlugInInfo plugIn);

        /// <summary>
        /// 删除插件。
        /// </summary>
        /// <param name="plugInId">待删除插件ID。</param>
        /// <returns>是否删除成功。</returns>
        bool DeletePlugIn(int plugInId);

        /// <summary>
        /// 删除插件实例。
        /// </summary>
        /// <param name="instanceId">待删除插件实例ID。</param>
        /// <returns>是否删除成功。</returns>
        bool DeletePlugInInstance(int instanceId);

        /// <summary>
        /// 删除插件实例。
        /// </summary>
        /// <param name="instanceIds">待删除插件实例ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        bool DeletePlugInInstances(int[] instanceIds);

        /// <summary>
        /// 返回插件列表。
        /// </summary>
        /// <returns>插件列表。</returns>
        PlugInCollection GetAllPlugIns();

        /// <summary>
        /// 返回插件列表。
        /// </summary>
        /// <remarks>该方法只在程序系统启动时调用，用于完成插件的初始化工作。</remarks>
        /// <returns>类集合。</returns>
        ClassCollection GetAllPlugInsForInit();

        /// <summary>
        /// 返回指定插件的信息。
        /// </summary>
        /// <param name="plugInId">插件ID。</param>
        /// <returns>插件信息。</returns>
        PlugInInfo GetPlugIn(int plugInId);

        /// <summary>
        /// 获取指定插件实例的配置信息。
        /// </summary>
        /// <param name="instanceId">实例ID。</param>
        /// <returns>配置信息（xml格式）</returns>
        string LoadConfig(int instanceId);

        /// <summary>
        /// 保存插件实例配置信息。
        /// </summary>
        /// <param name="instanceId">插件实例ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否保存成功。</returns>
        bool SaveConfig(int instanceId, string config);

        /// <summary>
        /// 启用指定插件。
        /// </summary>
        /// <param name="instanceId">待启用插件ID。</param>
        /// <returns>是否启用成功。</returns>
        bool StartPlugIn(int instanceId);

        /// <summary>
        /// 启用多个插件。
        /// </summary>
        /// <param name="instanceIds">待启用插件ID的集合。</param>
        /// <returns>是否启用成功。</returns>
        bool StartPlugIns(int[] instanceIds);

        /// <summary>
        /// 停用指定插件。
        /// </summary>
        /// <param name="instanceId">待停用插件ID。</param>
        /// <returns>是否停用成功。</returns>
        bool StopPlugIn(int instanceId);

        /// <summary>
        /// 停用多个插件。
        /// </summary>
        /// <param name="instanceIds">待停用插件ID的集合。</param>
        /// <returns>是否停用成功。</returns>
        bool StopPlugIns(int[] instanceIds);
    }
}
