namespace UniqueStudio.Core.PlugIn
{
    /// <summary>
    /// 插件需实现的接口。
    /// </summary>
    public interface IPlugIn
    {
        /// <summary>
        /// 注册相应的事件。
        /// </summary>
        void Register();

        /// <summary>
        /// 反注册事件。
        /// </summary>
        void UnRegister();
    }
}
