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
    internal class SqlSiteMapProvider : ISiteMap
    {
        private const string CREATE_PAGE = "CreatePage";
        private const string DELETE_PAGE = "DeletePage";
        private const string GET_ALL_PAGES = "GetAllPages";
        private const string UPDATE_PAGE = "UpdatePage";

        public SqlSiteMapProvider()
        {
            //默认构造函数
        }

        public PageInfo CreatePage(PageInfo page)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                       new SqlParameter("@SiteID",page.SiteId),
                                                       new SqlParameter("@Name",page.Name),
                                                       new SqlParameter("@PageType",Enum.GetName(typeof(PageType),page.PageType)),
                                                       new SqlParameter("@Url",page.Url),
                                                       new SqlParameter("@UrlRegex",page.UrlRegex),
                                                       new SqlParameter("@PagePath",page.PagePath),
                                                       new SqlParameter("@Parameters",page.Parameters),
                                                       new SqlParameter("@StaticPagePath",page.StaticPagePath),
                                                       new SqlParameter("@ProcessType",Enum.GetName(typeof(ProcessType),page.ProcessType)),
                                                       new SqlParameter("@SubOf",page.SubOf),
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

        public bool DeletePage(int pageId)
        {
            SqlParameter parm = new SqlParameter("@ID", pageId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_PAGE, parm) > 0;
        }

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

        public bool UpdatePage(PageInfo page)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                       new SqlParameter("@ID",page.Id),
                                                       new SqlParameter("@SiteID",page.SiteId),
                                                       new SqlParameter("@Name",page.Name),
                                                       new SqlParameter("@PageType",Enum.GetName(typeof(PageType),page.PageType)),
                                                       new SqlParameter("@Url",page.Url),
                                                       new SqlParameter("@UrlRegex",page.UrlRegex),
                                                       new SqlParameter("@PagePath",page.PagePath),
                                                       new SqlParameter("@Parameters",page.Parameters),
                                                       new SqlParameter("@StaticPagePath",page.StaticPagePath),
                                                       new SqlParameter("@ProcessType",Enum.GetName(typeof(ProcessType),page.ProcessType)),
                                                       new SqlParameter("@SubOf",page.SubOf),
                                                       new SqlParameter("@Depth",page.Depth)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_PAGE, parms) > 0;
        }

        private PageInfo FillPageInfo(SqlDataReader reader)
        {
            PageInfo page = new PageInfo();
            page.Id = (int)reader["ID"];
            page.SiteId = (int)reader["SiteID"];
            page.Name = reader["Name"].ToString();
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
            page.SubOf = (int)reader["SubOf"];
            page.Depth = (int)reader["Depth"];
            return page;
        }
    }
}
