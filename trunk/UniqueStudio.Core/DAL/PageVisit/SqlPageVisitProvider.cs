using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.PageVisit
{
    /// <summary>
    /// 提供页面访问管理在Sql Server上的实现方法
    /// </summary>
    internal class SqlPageVisitProvider : IPageVisit
    {
        private static string ADD_PAGEVISIT = "AddPageVisit";
        private static string GET_PAGEVISIT_COUNT = "GetPvCount";
        private static string GET_PAGEVISIT_LIST = "GetPageVisitList";

        /// <summary>
        /// 初始化<see cref="SqlPageVisitProvider"/>类的实例
        /// </summary>
        public SqlPageVisitProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 增加页面访问信息
        /// </summary>
        /// <param name="pv">页面访问信息</param>
        /// <returns>是否增加成功</returns>
        public bool AddPageVisit(PageVisitInfo pv)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@RawUrl",pv.RawUrl),
                                                    new SqlParameter("@UserHostAddress",pv.UserHostAddress),
                                                    new SqlParameter("@UserHostName",pv.UserHostName),
                                                    new SqlParameter("@UserAgent",pv.UserAgent),
                                                    new SqlParameter("@UrlReferrer",pv.UrlReferrer)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, ADD_PAGEVISIT, parms) > 0;
        }

        /// <summary>
        /// 返回页面访问总量
        /// </summary>
        /// <returns>页面访问总量</returns>
        public int GetPageVisitCount()
        {
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, GET_PAGEVISIT_COUNT, null);
            if (o != null && o != DBNull.Value)
            {
                return (int)o;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 返回页面访问列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页条目数</param>
        /// <returns>页面访问列表</returns>
        public PageVisitCollection GetPageVisitList(int pageIndex, int pageSize)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PageIndex",pageIndex),
                                                    new SqlParameter("@PageSize",pageSize),
                                                    new SqlParameter("@Amount",SqlDbType.Int)};
                    parms[0].Direction = ParameterDirection.InputOutput;
                    parms[2].Direction = ParameterDirection.Output;
                    SqlHelper.PrepareCommand(cmd, conn, CommandType.StoredProcedure, GET_PAGEVISIT_LIST, parms);

                    PageVisitCollection collection = new PageVisitCollection();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            collection.Add(FillPageVisitInfo(reader));
                        }
                        reader.Close();
                    }

                    collection.PageIndex = (int)parms[0].Value;
                    collection.Amount = (int)parms[2].Value;
                    collection.PageSize = pageSize;
                    cmd.Parameters.Clear();

                    return collection;
                }
            }
        }

        private PageVisitInfo FillPageVisitInfo(SqlDataReader reader)
        {
            PageVisitInfo pv = new PageVisitInfo();
            pv.RawUrl = reader["RawUrl"].ToString();
            pv.UserHostAddress = reader["UserHostAddress"].ToString();
            pv.UserHostName = reader["UserHostName"].ToString();
            pv.UserAgent = reader["UserAgent"].ToString();
            if (DBNull.Value != reader["UrlReferrer"])
            {
                pv.UrlReferrer = reader["UrlReferrer"].ToString();
            }
            pv.Time = Convert.ToDateTime(reader["Time"].ToString());
            return pv;
        }
    }
}
