//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：网站管理提供类需实现的方法。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 网站管理提供类需实现的方法。
    /// </summary>
    internal interface ISite
    {
        /// <summary>
        /// 返回网站列表。
        /// </summary>
        /// <returns>网站列表。</returns>
        SiteCollection GetAllSites();

        /// <summary>
        /// 载入网站配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>网站配置信息。</returns>
        string LoadConfig(int siteId);

        /// <summary>
        /// 保存网站配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="content">网站配置信息。</param>
        /// <returns>是否保存成功。</returns>
        bool SaveConfig(int siteId, string content);
    }
}
