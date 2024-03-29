﻿//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供网站管理在Sql Server上的实现方法。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.Site
{
    /// <summary>
    /// 提供网站管理在Sql Server上的实现方法。
    /// </summary>
    internal class SqlSiteProvider : IDAL.ISite
    {
        private const string GET_ALL_SITES = "GetAllSites";
        private const string GET_WEBSITE_CONFIG = "GetWebSiteConfig";
        private const string UPDATE_WEBSITE_CONFIG = "UpdateWebSiteConfig";

        /// <summary>
        /// 返回网站列表。
        /// </summary>
        /// <returns>网站列表。</returns>
        public SiteCollection GetAllSites()
        {
            SiteCollection collection = new SiteCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_SITES, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillSiteInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 载入网站配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>网站配置信息。</returns>
        public string LoadConfig(int siteId)
        {
            SqlParameter parm = new SqlParameter("@SiteID", siteId);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, GET_WEBSITE_CONFIG, parm);
            if (o != null && o != DBNull.Value)
            {
                return (string)o;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 保存网站配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="content">网站配置信息。</param>
        /// <returns>是否保存成功。</returns>
        public bool SaveConfig(int siteId, string content)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteId",siteId),
                                                    new SqlParameter("@Config",content)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_WEBSITE_CONFIG, parms) > 0;
        }

        //不含config项
        private SiteInfo FillSiteInfo(SqlDataReader reader)
        {
            SiteInfo site = new SiteInfo();
            site.SiteId = (int)reader["SiteID"];
            site.SiteName = reader["SiteName"].ToString();
            if (DBNull.Value != reader["RelativePath"])
            {
                site.RelativePath = reader["RelativePath"].ToString();
            }
            return site;
        }
    }
}
