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
    internal class SqlPageVisitProvider : IPageVisit
    {
        private static string ADD_PAGEVISIT = "AddPageVisit";
        private static string GET_PAGEVISIT_COUNT = "GetPvCount";
        private static string GET_PAGEVISIT_LIST = "GetPageVisitList";

        public SqlPageVisitProvider()
        {
        }

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
