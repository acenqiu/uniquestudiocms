using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.SiteMap
{
    /// <summary>
    /// 提供网站地图管理在Sql Server上的实现方法
    /// </summary>
    internal class SqlSiteMapProvider : ISiteMap
    {
        private const string CREATE_PAGE = "CreatePage";
        private const string DELETE_PAGE = "DeletePage";
        private const string GET_ALL_PAGES = "GetAllPages";
        private const string UPDATE_PAGE = "UpdatePage";

        /// <summary>
        /// 初始化<see cref="SqlSiteMapProvider"/>类的实例
        /// </summary>
        public SqlSiteMapProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <param name="page">页面信息</param>
        /// <returns>如果创建成功，返回页面信息，否则返回空</returns>
        public PageInfo CreatePage(PageInfo page)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                       new SqlParameter("@SiteID",page.SiteId),
                                                       new SqlParameter("@Name",page.PageName),
                                                       new SqlParameter("@PageType",Enum.GetName(typeof(PageType),page.PageType)),
                                                       new SqlParameter("@Url",page.Url),
                                                       new SqlParameter("@UrlRegex",page.UrlRegex),
                                                       new SqlParameter("@PagePath",page.PagePath),
                                                       new SqlParameter("@Parameters",page.Parameters),
                                                       new SqlParameter("@StaticPagePath",page.StaticPagePath),
                                                       new SqlParameter("@ProcessType",Enum.GetName(typeof(ProcessType),page.ProcessType)),
                                                       new SqlParameter("@SubOf",page.ParentPageId),
                                                       new SqlParameter("@Depth",page.Depth)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_PAGE, parms);
            if (o != null && o != DBNull.Value)
            {
                page.Id = Convert.ToInt32(o);
                return page;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 删除指定页面
        /// </summary>
        /// <param name="pageId">待删除页面ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeletePage(int pageId)
        {
            SqlParameter parm = new SqlParameter("@ID", pageId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_PAGE, parm) > 0;
        }

        /// <summary>
        /// 返回所有页面
        /// </summary>
        /// <returns>页面的集合</returns>
        public PageCollection GetAllPages()
        {
            PageCollection collection = new PageCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_PAGES, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillPageInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 更新页面信息
        /// </summary>
        /// <param name="page">页面信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdatePage(PageInfo page)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                       new SqlParameter("@ID",page.Id),
                                                       new SqlParameter("@SiteID",page.SiteId),
                                                       new SqlParameter("@Name",page.PageName),
                                                       new SqlParameter("@PageType",Enum.GetName(typeof(PageType),page.PageType)),
                                                       new SqlParameter("@Url",page.Url),
                                                       new SqlParameter("@UrlRegex",page.UrlRegex),
                                                       new SqlParameter("@PagePath",page.PagePath),
                                                       new SqlParameter("@Parameters",page.Parameters),
                                                       new SqlParameter("@StaticPagePath",page.StaticPagePath),
                                                       new SqlParameter("@ProcessType",Enum.GetName(typeof(ProcessType),page.ProcessType)),
                                                       new SqlParameter("@SubOf",page.ParentPageId),
                                                       new SqlParameter("@Depth",page.Depth)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_PAGE, parms) > 0;
        }

        private PageInfo FillPageInfo(SqlDataReader reader)
        {
            PageInfo page = new PageInfo();
            page.Id = (int)reader["ID"];
            page.SiteId = (int)reader["SiteID"];
            page.PageName = reader["Name"].ToString();
            page.PageType = (PageType)Enum.Parse(typeof(PageType), reader["PageType"].ToString());
            page.Url = reader["Url"].ToString();
            page.UrlRegex = reader["UrlRegex"].ToString();
            page.PagePath = reader["PagePath"].ToString();
            if (DBNull.Value != reader["Parameters"])
            {
                page.Parameters = reader["Parameters"].ToString();
            }
            page.StaticPagePath = reader["StaticPagePath"].ToString();
            page.ProcessType = (ProcessType)Enum.Parse(typeof(ProcessType), reader["ProcessType"].ToString());
            page.ParentPageId = (int)reader["SubOf"];
            page.Depth = (int)reader["Depth"];
            return page;
        }
    }
}
