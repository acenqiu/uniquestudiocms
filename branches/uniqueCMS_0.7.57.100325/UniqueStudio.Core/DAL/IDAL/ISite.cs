using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface ISite
    {
        /// <summary>
        /// 返回网站列表
        /// </summary>
        /// <returns>网站列表</returns>
        SiteCollection GetAllSites();

        /// <summary>
        /// 载入网站配置信息
        /// </summary>
        /// <param name="siteId">网站ID</param>
        /// <returns>网站配置信息</returns>
        string LoadConfig(int siteId);

        /// <summary>
        /// 保存网站配置信息
        /// </summary>
        /// <param name="siteId">网站ID</param>
        /// <param name="content">网站配置信息</param>
        /// <returns>是否保存成功</returns>
        bool SaveConfig(int siteId, string content);
    }
}
