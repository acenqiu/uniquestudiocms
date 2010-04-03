//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供组件管理在Sql Server上的实现方法。
// 完成日期：2010年04月02日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.DAL.Permission;

namespace UniqueStudio.DAL.Compenent
{
    /// <summary>
    /// 提供组件管理在Sql Server上的实现方法。
    /// </summary>
    internal class SqlCompenentProvider : ICompenent
    {
        private const string CREATE_COMPENENT = "CreateCompenent";
        private const string DELETE_COMPENENT = "DeleteCompenent";
        private const string GET_ALL_COMPENENTS = "GetAllCompenents";
        private const string GET_COMPENENT = "GetCompenent";
        private const string GET_COMPENENT_CONFIG_BY_ID = "GetCompenentConfigByID";
        private const string GET_COMPENENT_CONFIG_BY_NAME = "GetCompenentConfigByName";
        private const string IS_COMPENENT_EXISTS = "IsCompenentExists";
        private const string UPDATE_COMPENENT_CONFIG_BY_ID = "UpdateCompenentConfigByID";
        private const string UPDATE_COMPENENT_CONFIG_BY_NAME = "UpdateCompenentConfigByName";

        /// <summary>
        /// 初始化<see cref="SqlCompenentProvider"/>类的实例。
        /// </summary>
        public SqlCompenentProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建组件。
        /// </summary>
        /// <remarks>在数据库中插入组件记录。</remarks>
        /// <param name="compenent">组件信息。</param>
        /// <returns>如果创建成功，返回组件信息，否则返回空。</returns>
        public CompenentInfo CreateCompenent(CompenentInfo compenent)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(CREATE_COMPENENT, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            cmd.Parameters.AddWithValue("@SiteID", compenent.SiteId);
                            cmd.Parameters.AddWithValue("@CompenentName", compenent.CompenentName);
                            cmd.Parameters.AddWithValue("@DisplayName", compenent.DisplayName);
                            cmd.Parameters.AddWithValue("@CompenentAuthor", compenent.CompenentAuthor);
                            cmd.Parameters.AddWithValue("@Description", compenent.Description);
                            cmd.Parameters.AddWithValue("@ClassPath", compenent.ClassPath);
                            cmd.Parameters.AddWithValue("@Assembly", compenent.Assembly);
                            cmd.Parameters.AddWithValue("@WorkingPath", compenent.WorkingPath);
                            cmd.Parameters.AddWithValue("@Config", compenent.Config);
                            object o = cmd.ExecuteScalar();
                            if (o != null && o != DBNull.Value)
                            {
                                compenent.CompenentId = Convert.ToInt32(o);
                                if ((new SqlPermissionProvider()).CreatePermissions(conn, cmd, compenent.Permissions))
                                {
                                    trans.Commit();
                                    return compenent;
                                }
                                else
                                {
                                    trans.Rollback();
                                    return null;
                                }
                            }
                            else
                            {
                                trans.Rollback();
                                return null;
                            }
                        }
                        catch
                        {
                            trans.Rollback();
                            return null;
                        }
                    }//end of using transaction
                }
            }
        }

        /// <summary>
        /// 删除指定组件。
        /// </summary>
        /// <param name="compenentId">待删除组件ID。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteCompenent(int compenentId)
        {
            SqlParameter parm = new SqlParameter("@CompenentID", compenentId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_COMPENENT, parm) > 0;
        }

        /// <summary>
        /// 删除多个组件。
        /// </summary>
        /// <param name="compenentId">待删除组件ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteCompenents(int[] compenentIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_COMPENENT, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompenentID", SqlDbType.Int);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;

                        try
                        {
                            foreach (int compenentId in compenentIds)
                            {
                                cmd.Parameters[0].Value = compenentId;
                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回所有组件的信息。
        /// </summary>
        /// <returns>包含所有信息的组件集合。</returns>
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
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <param name="compenentId">组件ID。</param>
        /// <returns>配置信息。</returns>
        public string GetCompenentConfig(int compenentId)
        {
            SqlParameter parm = new SqlParameter("@CompenentID", compenentId);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, GET_COMPENENT_CONFIG_BY_ID, parm);
            if (o != null && o != DBNull.Value)
            {
                return (string)o;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <returns>配置信息。</returns>
        public string GetCompenentConfig(int siteId, string compenentName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteID",siteId),
                                                    new SqlParameter("@CompenentName",compenentName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, GET_COMPENENT_CONFIG_BY_NAME, parms);
            if (o != null && o != DBNull.Value)
            {
                return (string)o;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回组件信息。
        /// </summary>
        /// <param name="compenentId">组件ID。</param>
        /// <returns>组件信息。</returns>
        public CompenentInfo GetCompenent(int compenentId)
        {
            SqlParameter parm = new SqlParameter("@CompenentID", compenentId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_COMPENENT, parm))
            {
                if (reader.Read())
                {
                    return FillCompenentInfo(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 返回指定组件是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <returns>是否存在。</returns>
        public bool IsCompenentExists(int siteId, string compenentName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteID",siteId),
                                                    new SqlParameter("@CompenentName",compenentName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_COMPENENT_EXISTS, parms);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean((int)o);
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="compenentId">组件ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        public bool UpdateCompenentConfig(int compenentId, string config)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@CompenentID", compenentId),
                                                            new SqlParameter("@Config",config)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_COMPENENT_CONFIG_BY_ID, parms) > 0;
        }

        /// <summary>
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        public bool UpdateCompenentConfig(int siteId, string compenentName, string config)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteID",siteId),
                                                    new SqlParameter("@CompenentName",compenentName),
                                                    new SqlParameter("@Config",config)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_COMPENENT_CONFIG_BY_NAME, parms) > 0;
        }

        private CompenentInfo FillCompenentInfo(SqlDataReader reader)
        {
            CompenentInfo compenent = new CompenentInfo();
            compenent.CompenentId = (int)reader["CompenentID"];
            compenent.SiteId = (int)reader["SiteID"];
            compenent.CompenentName = reader["CompenentName"].ToString();
            compenent.DisplayName = reader["DisplayName"].ToString();
            compenent.CompenentAuthor = reader["CompenentAuthor"].ToString();
            compenent.Description = reader["Description"].ToString();
            compenent.WorkingPath = reader["WorkingPath"].ToString();
            compenent.SiteName = reader["SiteName"].ToString();
            return compenent;
        }
    }
}
