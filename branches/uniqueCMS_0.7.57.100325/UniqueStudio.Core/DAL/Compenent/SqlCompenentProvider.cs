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
    /// <summary>
    /// 提供组件管理在Sql Server上的实现方法
    /// </summary>
    internal class SqlCompenentProvider : ICompenent
    {
        private const string CREATE_COMPENENT = "CreateCompenent";
        private const string GET_ALL_COMPENENTS = "GetAllCompenents";

        /// <summary>
        /// 初始化<see cref="SqlCompenentProvider"/>类的实例
        /// </summary>
        public SqlCompenentProvider()
        {
        }

        /// <summary>
        /// 创建组件
        /// </summary>
        /// <remarks>在数据库中插入组件记录</remarks>
        /// <param name="compenent">组件信息</param>
        /// <returns>如果创建成功，返回组件信息，否则返回空</returns>
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

        /// <summary>
        /// 返回所有组件的信息
        /// </summary>
        /// <returns>包含所有信息的组件集合</returns>
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

        /// <summary>
        /// 删除组件
        /// </summary> 
        /// <remarks>在数据库中删除组件信息</remarks>
        /// <param name="compenentId">待删除组件ID</param>
        /// <returns>是否删除成功</returns>
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
