﻿//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供网站管理的方法。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Data.Common;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.Core.Permission;

namespace UniqueStudio.Core.Site
{
    /// <summary>
    /// 提供网站管理的方法。
    /// </summary> 
    public class SiteManager
    {
        private static readonly ISite provider = DALFactory.CreateSite();
        private static string baseAddress = string.Empty;

        private static Dictionary<int, WebSiteConfig> wsConfigs = new Dictionary<int, WebSiteConfig>();
        private static Dictionary<int, string> wsBaseAddresses = new Dictionary<int, string>();
        private static Dictionary<int, string> wsBasePhysicalPath = new Dictionary<int, string>();

        /// <summary>
        /// 初始化<see cref="SiteManager"/>类的实例。
        /// </summary>
        public SiteManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 返回网站配置的实例。
        /// </summary>
        /// <param name="siteId">站点ID。</param>
        /// <returns>网站配置的实例。</returns>
        public static WebSiteConfig Config(int siteId)
        {
            Validator.CheckNotPositive(siteId, "siteId");

            if (wsConfigs.ContainsKey(siteId))
            {
                return wsConfigs[siteId];
            }
            else
            {
                string content = null;
                try
                {
                    content = provider.LoadConfig(siteId);
                }
                catch (DbException ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new DatabaseException();
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new UnhandledException();
                }

                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception();
                }

                WebSiteConfig wsConfig = new WebSiteConfig();
                wsConfig.LoadConfig(content);
                wsConfigs.Add(siteId, wsConfig);

                return wsConfigs[siteId];
            }
        }

        /// <summary>
        /// 返回指定网站的根网址。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>网站根网址。</returns>
        public static string BaseAddress(int siteId)
        {
            Validator.CheckNegative(siteId, "siteId");

            if (baseAddress != ServerConfig.BaseAddress)
            {
                baseAddress = ServerConfig.BaseAddress;
                wsBaseAddresses.Clear();
            }
            if (siteId == 0)
            {
                return ServerConfig.BaseAddress;
            }
            if (!wsBaseAddresses.ContainsKey(siteId))
            {
                SiteCollection sites = (new SiteManager()).GetAllSites();
                wsBaseAddresses.Clear();
                foreach (SiteInfo site in sites)
                {
                    wsBaseAddresses.Add(site.SiteId, PathHelper.PathCombine(ServerConfig.BaseAddress, site.RelativePath).Trim(new char[] { '/', '\\' }));
                }
            }

            if (wsBaseAddresses.ContainsKey(siteId))
            {
                return wsBaseAddresses[siteId];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回指定网站的根物理路径。
        /// </summary>
        /// <param name="siteId">网址ID。</param>
        /// <returns>网站根物理路径。</returns>
        public static string BasePhysicalPath(int siteId)
        {
            Validator.CheckNegative(siteId, "siteId");

            if (siteId == 0)
            {
                return GlobalConfig.BasePhysicalPath;
            }
            if (!wsBasePhysicalPath.ContainsKey(siteId))
            {
                SiteCollection sites = (new SiteManager()).GetAllSites();
                wsBasePhysicalPath.Clear();
                foreach (SiteInfo site in sites)
                {
                    wsBasePhysicalPath.Add(site.SiteId, PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, site.RelativePath).Trim(new char[] { '/', '\\' }));
                }
            }

            if (wsBasePhysicalPath.ContainsKey(siteId))
            {
                return wsBasePhysicalPath[siteId];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回网站列表。
        /// </summary>
        /// <returns>网站列表。</returns>
        public SiteCollection GetAllSites()
        {
            try
            {
                return provider.GetAllSites();
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回网站配置信息。
        /// </summary>
        /// <param name="siteId">站点ID。</param>
        /// <returns>网站配置的实例。</returns>
        public string LoadConfig(int siteId)
        {
            Validator.CheckNotPositive(siteId, "siteId");

            try
            {
                return provider.LoadConfig(siteId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 保存网站配置信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="content">网站配置信息。</param>
        /// <returns>是否保存成功</returns>
        public bool SaveConfig(UserInfo currentUser, int siteId, string content)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(content, "content");
            //验证内容格式

            PermissionManager.CheckPermission(currentUser, "EditSiteConfig", "配置系统设置");

            try
            {
                if (provider.SaveConfig(siteId, content))
                {
                    WebSiteConfig config = Config(siteId);
                    config.LoadConfig(content);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }
    }
}
