//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：页面处理方式。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 页面处理方式。
    /// </summary>
    public enum ProcessType
    {
        /// <summary>
        /// 使用模板引擎解析。
        /// </summary>
        ByEngine,

        /// <summary>
        /// 使用asp.net解析。
        /// </summary>
        ByAspnet
    }
}
