//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：页面访问管理提供类需实现的方法。
// 完成日期：2010年03月20日
// 版本：v0.4
// 作者：邱江毅
//=================================================================
using System;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.PageVisit
{
    /// <summary>
    /// 提供页面访问管理在Sql Server上的实现方法。
    /// </summary>
    internal class SqlPageVisitProvider : IPageVisit
    {
        private static string ADD_PAGEVISIT = "AddPageVisit";
        private static string GET_PAGEVISIT_COUNT = "GetPvCount";
        private static string GET_PV_STAT_BY_MONTH = "GetPvStatByMonth";
        private static string GET_PAGEVISIT_LIST = "GetPageVisitList";

        /// <summary>
        /// 初始化<see cref="SqlPageVisitProvider"/>类的实例。
        /// </summary>
        public SqlPageVisitProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 增加页面访问信息。
        /// </summary>
        /// <param name="pv">页面访问信息。</param>
        public void AddPageVisit(PageVisitInfo pv)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@RawUrl",pv.RawUrl),
                                                    new SqlParameter("@UserHostAddress",pv.UserHostAddress),
                                                    new SqlParameter("@UserHostName",pv.UserHostName),
                                                    new SqlParameter("@UserAgent",pv.UserAgent),
                                                    new SqlParameter("@UrlReferrer",pv.UrlReferrer)};
            SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, ADD_PAGEVISIT, parms);
        }

        /// <summary>
        /// 返回页面访问总量。
        /// </summary>
        /// <returns>页面访问总量。</returns>
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
        /// 返回指定月份每日的访问量信息。
        /// </summary>
        /// <param name="month">月份。</param>
        /// <returns>二维点的集合。</returns>
        public PointCollection<int, int> GetPvStatByMonth(DateTime month)
        {
            int[] daysOfMonth = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int days = daysOfMonth[month.Month - 1];
            if (month.Month == 2 &&
                        ((month.Year % 4 == 0 && month.Year % 100 != 0) || month.Year % 400 == 0))
            {
                days = 29;
            }
            PointCollection<int, int> collection = new PointCollection<int, int>(days);
            for (int i = 0; i < days; i++)
            {
                collection.Add(new PointInfo<int, int>(i + 1, 0));
            }
            collection.MaxX = days;
            collection.MinX = 1;
            collection.MaxY = 0;
            collection.MaxY = 0;

            SqlParameter parm = new SqlParameter("@Month", month);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_PV_STAT_BY_MONTH, parm))
            {
                int day;
                int count;
                while (reader.Read())
                {
                    day = Convert.ToDateTime(reader["Date"]).Day;
                    count = (int)reader["Count"];
                    collection[day - 1].Y = count;
                    collection.MaxY = count > collection.MaxY ? count : collection.MaxY;
                    collection.MinY = count < collection.MinY ? count : collection.MinY;
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回页面访问列表。
        /// </summary>
        /// <param name="pageIndex">页索引。</param>
        /// <param name="pageSize">每页条目数。</param>
        /// <returns>页面访问列表。</returns>
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
