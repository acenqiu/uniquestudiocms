using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.PlugIn
{
    /// <summary>
    /// 提供插件管理在Sql Server上的实现方法。
    /// </summary>
    internal class SqlPlugInProvider : IPlugIn
    {
        private const string ADD_PLUGIN_INSTANCE = "AddPlugInInstance";
        private const string CREATE_PLUGIN = "CreatePlugIn";
        private const string DELETE_PLUGIN = "DeletePlugIn";
        private const string DELETE_PLUGIN_INSTANCE = "DeletePlugInInstance";
        private const string GET_ALL_PLUGIN_INFO = "GetAllPlugIns_Info";
        private const string GET_ALL_PLUGIN_INIT = "GetAllPlugIns_Init";
        private const string GET_ALL_PLUGIN_INSTANCES = "GetAllPlugInInstances";
        private const string GET_PLUGIN = "GetPlugIn";
        private const string GET_PLUGIN_CONFIG = "GetPlugInConfig";
        private const string GET_PLUGIN_INSTANCE_CONFIG = "GetPlugInInstanceConfig";
        private const string SET_PLUGIN_STATUS = "SetPlugInStatus";
        private const string UPDATE_PLUGIN_INSTANCE_CONFIG = "UpdatePlugInInstanceConfig";

        /// <summary>
        /// 初始化<see cref="SqlPlugInProvider"/>类的实例。
        /// </summary>
        public SqlPlugInProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 新增插件实例。
        /// </summary>
        /// <param name="plugInId">插件ID。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否增加成功。</returns>
        public bool AddPlugInInstance(int plugInId, int siteId)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GET_PLUGIN_CONFIG, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PlugInID", plugInId);

                    conn.Open();
                    object o = cmd.ExecuteScalar();
                    if (o != null && o != DBNull.Value)
                    {
                        string config = (string)o;

                        cmd.Parameters.AddWithValue("@SiteID", siteId);
                        cmd.Parameters.AddWithValue("@Config", config);
                        cmd.CommandText = ADD_PLUGIN_INSTANCE;

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
        /// 创建插件。
        /// </summary>
        /// <param name="plugIn">待安装插件信息。</param>
        /// <returns>如果安装成功返回插件信息，否则返回空。</returns>
        public PlugInInfo CreatePlugIn(PlugInInfo plugIn)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PlugInName",plugIn.PlugInName),
                                                    new SqlParameter("@DisplayName",plugIn.DisplayName),
                                                    new SqlParameter("@PlugInAuthor",plugIn.PlugInAuthor),
                                                    new SqlParameter("@Description",plugIn.Description),
                                                    new SqlParameter("@ClassPath",plugIn.ClassPath),
                                                    new SqlParameter("@Assembly",plugIn.Assembly),
                                                    new SqlParameter("@PlugInCategory",plugIn.PlugInCategory),
                                                    new SqlParameter("@PlugInOrdering",plugIn.PlugInOrdering),
                                                    new SqlParameter("@WorkingPath",plugIn.WorkingPath),
                                                    new SqlParameter("@Config",plugIn.Config),
                                                    new SqlParameter("@IsEnabled",plugIn.Instances[0].IsEnabled),
                                                    new SqlParameter("@SiteID",plugIn.Instances[0].SiteId)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_PLUGIN, parms);
            if (o != null && o != DBNull.Value)
            {
                plugIn.PlugInId = Convert.ToInt32(o);
                return plugIn;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 删除插件。
        /// </summary>
        /// <param name="plugInId">待删除插件ID。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeletePlugIn(int plugInId)
        {
            SqlParameter parm = new SqlParameter("@PlugInID", plugInId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_PLUGIN, parm) > 0;
        }

        /// <summary>
        /// 删除插件实例。
        /// </summary>
        /// <param name="instanceId">待删除插件实例ID。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeletePlugInInstance(int instanceId)
        {
            SqlParameter parm = new SqlParameter("@InstanceID", instanceId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_PLUGIN_INSTANCE, parm) > 0;
        }

        /// <summary>
        /// 删除插件实例。
        /// </summary>
        /// <param name="instanceIds">待删除插件实例ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeletePlugInInstances(int[] instanceIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_PLUGIN_INSTANCE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@InstanceID", SqlDbType.Int);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;

                        try
                        {
                            foreach (int instanceId in instanceIds)
                            {
                                cmd.Parameters[0].Value = instanceId;
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
        /// 返回插件列表。
        /// </summary>
        /// <returns>插件列表。</returns>
        public PlugInCollection GetAllPlugIns()
        {
            PlugInCollection collection = new PlugInCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_PLUGIN_INFO, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillPlugInInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回插件列表。
        /// </summary>
        /// <remarks>该方法只在程序系统启动时调用，用于完成插件的初始化工作。</remarks>
        /// <returns>类集合。</returns>
        public ClassCollection GetAllPlugInsForInit()
        {
            ClassCollection collection = new ClassCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_PLUGIN_INIT, null))
            {
                while (reader.Read())
                {
                    ClassInfo item = new ClassInfo();
                    item.ClassPath = reader["ClassPath"].ToString();
                    item.Assembly = reader["Assembly"].ToString();
                    collection.Add(item);
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回指定插件的信息。
        /// </summary>
        /// <param name="plugInId">插件ID。</param>
        /// <returns>插件信息。</returns>
        public PlugInInfo GetPlugIn(int plugInId)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GET_PLUGIN, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PlugInID", plugInId);

                    conn.Open();
                    PlugInInfo plugIn = null;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            plugIn = FillPlugInInfo(reader);
                        }
                        else
                        {
                            return null;
                        }
                        reader.Close();
                    }

                    cmd.CommandText = GET_ALL_PLUGIN_INSTANCES;
                    plugIn.Instances = new List<PlugInInstanceInfo>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlugInInstanceInfo instance = new PlugInInstanceInfo();
                            instance.InstanceId = (int)reader["InstanceID"];
                            instance.SiteId = (int)reader["SiteID"];
                            instance.SiteName = reader["SiteName"].ToString();
                            instance.IsEnabled = Convert.ToBoolean(reader["IsEnabled"]);
                            plugIn.Instances.Add(instance);
                        }
                    }
                    return plugIn;
                }
            }
        }

        /// <summary>
        /// 获取指定插件实例的配置信息。
        /// </summary>
        /// <param name="instanceId">实例ID。</param>
        /// <returns>配置信息（xml格式）。</returns>
        public string LoadConfig(int instanceId)
        {
            SqlParameter parm = new SqlParameter("@InstanceID", instanceId);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, GET_PLUGIN_INSTANCE_CONFIG, parm);
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
        /// 保存插件实例配置信息。
        /// </summary>
        /// <param name="instanceId">插件实例ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否保存成功。</returns>
        public bool SaveConfig(int instanceId, string config)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@InstanceID",instanceId),
                                                    new SqlParameter("@Config",config)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_PLUGIN_INSTANCE_CONFIG, parms) > 0;
        }

        /// <summary>
        /// 启用指定插件。
        /// </summary>
        /// <param name="instanceId">待启用插件ID。</param>
        /// <returns>是否启用成功。</returns>
        public bool StartPlugIn(int instanceId)
        {
            return SetPlugInStatus(instanceId, true);
        }

        /// <summary>
        /// 启用多个插件。
        /// </summary>
        /// <param name="instanceIds">待启用插件ID的集合。</param>
        /// <returns>是否启用成功。</returns>
        public bool StartPlugIns(int[] instanceIds)
        {
            return SetPlugInStatus(instanceIds, true);
        }

        /// <summary>
        /// 停用指定插件。
        /// </summary>
        /// <param name="instanceId">待停用插件ID。</param>
        /// <returns>是否停用成功。</returns>
        public bool StopPlugIn(int instanceId)
        {
            return SetPlugInStatus(instanceId, false);
        }

        /// <summary>
        /// 停用多个插件。
        /// </summary>
        /// <param name="instanceIds">待停用插件ID的集合。</param>
        /// <returns>是否停用成功。</returns>
        public bool StopPlugIns(int[] instanceIds)
        {
            return SetPlugInStatus(instanceIds, false);
        }

        private PlugInInfo FillPlugInInfo(SqlDataReader reader)
        {
            PlugInInfo plugIn = new PlugInInfo();
            plugIn.PlugInId = (int)reader["PlugInID"];
            plugIn.PlugInName = reader["PlugInName"].ToString();
            plugIn.DisplayName = reader["DisplayName"].ToString();
            if (reader["PlugInAuthor"] != DBNull.Value)
            {
                plugIn.PlugInAuthor = reader["PlugInAuthor"].ToString();
            }
            if (reader["Description"] != DBNull.Value)
            {
                plugIn.Description = reader["Description"].ToString();
            }
            if (reader["PlugInCategory"] != DBNull.Value)
            {
                plugIn.PlugInCategory = reader["PlugInCategory"].ToString();
            }
            plugIn.PlugInOrdering = (int)reader["PlugInOrdering"];
            return plugIn;
        }

        private bool SetPlugInStatus(int instanceId, bool isEnabled)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@instanceId",instanceId),
                                                        new SqlParameter("@IsEnabled",isEnabled)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, SET_PLUGIN_STATUS, parms) > 0;
        }

        private bool SetPlugInStatus(int[] instanceIds, bool isEnabled)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SET_PLUGIN_STATUS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@instanceId", SqlDbType.Int);
                    cmd.Parameters.AddWithValue("@IsEnabled", isEnabled);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (int instanceId in instanceIds)
                            {
                                cmd.Parameters[0].Value = instanceId;
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
    }
}
