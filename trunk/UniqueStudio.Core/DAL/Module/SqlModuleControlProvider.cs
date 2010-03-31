//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供模块控件管理在Sql Server上的实现方法。
// 完成日期：2010年03月29日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;
using UniqueStudio.Common.Config;

namespace UniqueStudio.DAL.Module
{
    /// <summary>
    /// 提供模块控件管理在Sql Server上的实现方法。
    /// </summary>
    internal class SqlModuleControlProvider : IModuleControl
    {
        private const string CREATE_MODULE_CONTROL = "CreateModuleControl";
        private const string DELETE_MODULE_CONTROL = "DeleteModuleControl";
        private const string GET_ALL_MODULE_CONTROLS = "GetAllModuleControls";
        private const string GET_MODULE_CONFIG = "GetModuleConfig";
        private const string GET_MODULE_CONTROL = "GetModuleControl";
        private const string GET_MODULE_CONTROL_BY_NAME = "GetModuleControlByName";
        private const string IS_MODULE_CONTROL_EXISTS = "IsModuleControlExists";
        private const string SET_MODULE_CONTROL_STATUS = "SetModuleControlStatus";
        private const string UPDATE_MODULE_CONTROL = "UpdateModuleControl";

        /// <summary>
        /// 初始化<see cref="SqlModuleControlProvider"/>类的实例。
        /// </summary>
        public SqlModuleControlProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建控件。
        /// </summary>
        /// <param name="moduleControl">控件信息。</param>
        /// <returns>是否创建成功。</returns>
        public bool CreateModuleControl(ModuleControlInfo control)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GET_MODULE_CONFIG, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ModuleID", control.ModuleId);

                    conn.Open();
                    object o = cmd.ExecuteScalar();
                    if (o != null && o != DBNull.Value)
                    {
                        string config = (string)o;

                        cmd.Parameters.AddWithValue("@SiteID", control.SiteId);
                        cmd.Parameters.AddWithValue("@ControlName", control.ControlName);
                        cmd.Parameters.AddWithValue("@IsEnabled", control.IsEnabled);
                        cmd.Parameters.AddWithValue("@Config", config);
                        cmd.CommandText = CREATE_MODULE_CONTROL;

                        return cmd.ExecuteNonQuery() > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 删除指定控件。
        /// </summary>
        /// <param name="controlId">控件ID。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteModuleControl(int controlId)
        {
            SqlParameter parm = new SqlParameter("@ControlID", controlId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_MODULE_CONTROL, parm) > 0;
        }

        /// <summary>
        /// 删除多个控件。
        /// </summary>
        /// <param name="controlIds">待删除控件ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteModuleControls(int[] controlIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_MODULE_CONTROL, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ControlID", SqlDbType.Int);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;

                        try
                        {
                            foreach (int controlId in controlIds)
                            {
                                cmd.Parameters[0].Value = controlId;
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
        /// 返回指定网站下的所有控件。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>控件的集合。</returns>
        public ModuleControlCollection GetAllModuleControls(int siteId)
        {
            ModuleControlCollection collection = new ModuleControlCollection();

            SqlParameter parm = new SqlParameter("@SiteID", siteId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_MODULE_CONTROLS, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillModuleControlInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回指定控件。
        /// </summary>
        /// <param name="controlId">待获取控件ID。</param>
        /// <returns>控件信息，如果不存在返回空。</returns>
        public ModuleControlInfo GetModuleControl(int controlId)
        {
            SqlParameter parm = new SqlParameter("@ControlID", controlId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_MODULE_CONTROL, parm))
            {
                if (reader.Read())
                {
                    ModuleControlInfo control = FillModuleControlInfo(reader);
                    control.Config = reader["Config"].ToString();
                    return control;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 返回指定控件。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">待获取控件名称。</param>
        /// <returns>控件信息，如果不存在返回空。</returns>
        public ModuleControlInfo GetModuleControl(int siteId, string controlName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@SiteID",siteId),
                                                        new SqlParameter("@ControlName",controlName)};
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_MODULE_CONTROL_BY_NAME, parms))
            {
                if (reader.Read())
                {
                    ModuleControlInfo control = FillModuleControlInfo(reader);
                    control.Config = reader["Config"].ToString();
                    return control;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 返回指定控件是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">控件名称。</param>
        /// <returns>是否存在。</returns>
        public bool IsModuleControlExists(int siteId, string controlName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@SiteID",siteId),
                                                        new SqlParameter("@ControlName",controlName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_MODULE_CONTROL_EXISTS, parms);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean(o);
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 设置指定控件的状态。
        /// </summary>
        /// <param name="controlId">控件ID。</param>
        /// <param name="isEnabled">是否启用。</param>
        /// <returns>是否更新成功。</returns>
        public bool SetModuleControlStatus(int controlId, bool isEnabled)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@ControlID",controlId),
                                                        new SqlParameter("@IsEnabled",isEnabled)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, SET_MODULE_CONTROL_STATUS, parms) > 0;
        }

        /// <summary>
        /// 设置多个控件的状态。
        /// </summary>
        /// <param name="controlIds">待设置控件的集合。</param>
        /// <param name="isEnabled">是否启用。</param>
        /// <returns>是否更新成功。</returns>
        public bool SetModuleControlStatus(int[] controlIds, bool isEnabled)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SET_MODULE_CONTROL_STATUS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ControlID", SqlDbType.Int);
                    cmd.Parameters.AddWithValue("@IsEnabled", isEnabled);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (int controlId in controlIds)
                            {
                                cmd.Parameters[0].Value = controlId;
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
        /// 更新控件信息。
        /// </summary>
        /// <param name="control">待更新控件信息。</param>
        /// <returns>是否更新成功。</returns>
        public bool UpdateModuleControl(ModuleControlInfo control)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                         new SqlParameter("@ControlID",control.ControlId),
                                                         new SqlParameter("@ControlName",control.ControlName),
                                                         new SqlParameter("@IsEnabled",control.IsEnabled),
                                                         new SqlParameter("@Config",control.Config)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_MODULE_CONTROL, parms) > 0;
        }

        private ModuleControlInfo FillModuleControlInfo(SqlDataReader reader)
        {
            ModuleControlInfo control = new ModuleControlInfo();
            control.ControlId = (int)reader["ControlID"];
            control.ControlName = reader["ControlName"].ToString();
            control.ModuleName = reader["ModuleName"].ToString();
            control.IsEnabled = Convert.ToBoolean(reader["IsEnabled"].ToString());
            return control;
        }
    }
}
