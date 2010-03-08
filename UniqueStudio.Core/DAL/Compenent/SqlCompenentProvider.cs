using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.Compenent
{
    internal class SqlCompenentProvider : ICompenent
    {
        private const string CREATE_COMPENENT = "CreateCompenent";
        private const string GET_ALL_COMPENENTS = "GetAllCompenents";

        public SqlCompenentProvider()
        {
        }

        public CompenentInfo CreateCompenent(CompenentInfo compenent)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@CompenentName",compenent.CompenentName),
                                                    new SqlParameter("@DisplayName",compenent.DisplayName),
                                                    new SqlParameter("@CompenentAuthor",compenent.CompenentAuthor),
                                                    new SqlParameter("@Description",compenent.Description),
                                                    new SqlParameter("@InstallFilePath",compenent.InstallFilePath)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_COMPENENT, parms);
            if (o != null && o != DBNull.Value)
            {
                compenent.CompenentId = Convert.ToInt32(o);
                return compenent;
            }
            else
            {
                throw new Exception();
            }
        }

        public CompenentCollection GetAllCompenents()
        {
            CompenentCollection collection = new CompenentCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_COMPENENTS, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillCompenentInfo(reader));
                }
            }
            return collection;
        }

        public bool DeleteCompenent(int compenentId)
        {
            throw new NotImplementedException();
        }

        private CompenentInfo FillCompenentInfo(SqlDataReader reader)
        {
            CompenentInfo compenent = new CompenentInfo();
            compenent.CompenentId = (int)reader["CompenentId"];
            compenent.CompenentName = reader["CompenentName"].ToString();
            compenent.DisplayName = reader["DisplayName"].ToString();
            compenent.CompenentAuthor = reader["CompenentAuthor"].ToString();
            compenent.Description = reader["Description"].ToString();
            compenent.InstallFilePath = reader["InstallFilePath"].ToString();
            return compenent;
        }
    }
}
