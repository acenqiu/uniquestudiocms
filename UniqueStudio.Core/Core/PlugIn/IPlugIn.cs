//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：插件需实现的接口。
// 完成日期：2010年03月28日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
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
