namespace UniqueStudio.Core.PlugIn
{
    /// <summary>
    /// 插件需实现的接口。
    /// </summary>
    public interface IPlugIn
    {
        /// <summary>
        /// 启用插件，注册相应的事件。
        /// </summary>
        void Start();

        /// <summary>
        /// 停用插件，反注册事件。
        /// </summary>
        void Stop();
    }
}
