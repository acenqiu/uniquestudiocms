//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供模块管理在Sql Server上的实现方法。
// 完成日期：2010年03月31日
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

namespace UniqueStudio.DAL.Module
{
    /// <summary>
    /// 提供模块管理在Sql Server上的实现方法。
    /// </summary>
    internal class SqlModuleProvider : IModule
    {
        private const string CREATE_MODULE = "CreateModule";
        private const string DELETE_MODULE = "DeleteModule";
        private const string GET_ALL_MODULES = "GetAllModules";
        private const string GET_MODULE_BY_ID = "GetModuleByID";
        private const string GET_MODULE_BY_NAME = "GetModuleByName";
        private const string GET_MODULE_NAME = "GetModuleName";
        private const string IS_MODULE_EXISTS = "IsModuleExists";

        /// <summary>
        /// 初始化<see cref="SqlModuleProvider"/>类的实例。
        /// </summary>
        public SqlModuleProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建模块。
        /// </summary>
        /// <param name="module">模块信息。</param>
        /// <returns>如果创建成功，返回模块信息，否则返回空。</returns>
        public ModuleInfo CreateModule(ModuleInfo module)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@ModuleName",module.ModuleName),
                                                    new SqlParameter("@DisplayName",module.DisplayName),
                                                    new SqlParameter("@ModuleAuthor",module.ModuleAuthor),
                                                    new SqlParameter("@Description",module.Description),
                                                    new SqlParameter("@ClassPath",module.ClassPath),
                                                    new SqlParameter("@Assembly",module.Assembly),
                                                    new SqlParameter("@WorkingPath",module.WorkingPath),
                                                    new SqlParameter("@Config",module.Config)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_MODULE, parms);
            if (o != null && o != DBNull.Value)
            {
                module.ModuleId = Convert.ToInt32(o);
                return module;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 删除指定模块。
        /// </summary>
        /// <param name="moduleId">待删除模块ID。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteModule(int moduleId)
        {
            SqlParameter parm = new SqlParameter("@ModuleID", moduleId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_MODULE, parm) > 0;
        }

        /// <summary>
        /// 删除多个模块。
        /// </summary>
        /// <param name="moduleIds">待删除模块ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteModules(int[] moduleIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_MODULE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ModuleID", SqlDbType.Int);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;

                        try
                        {
                            foreach (int moduleId in moduleIds)
                            {
                                cmd.Parameters[0].Value = moduleId;
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
        /// 返回所有模块。
        /// </summary>
        /// <returns>模块的集合。</returns>
        public ModuleCollection GetAllModules()
        {
            ModuleCollection collection = new ModuleCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_MODULES, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillModuleInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回指定模块。
        /// </summary>
        /// <remarks>仅返回ClassPath,Assembly。</remarks>
        /// <param name="moduleId">模块ID。</param>
        /// <returns>模块信息，如果获取失败，返回空。</returns>
        public ModuleInfo GetModule(int moduleId)
        {
            SqlParameter parm = new SqlParameter("@ModuleID", moduleId);
            return GetModule(GET_MODULE_BY_ID, parm);
        }

        /// <summary>
        /// 返回指定模块。
        /// </summary>
        /// <remarks>仅返回ClassPath,Assembly。</remarks>
        /// <param name="moduleName">模块名称。</param>
        /// <returns>模块信息，如果获取失败，返回空。</returns>
        public ModuleInfo GetModule(string moduleName)
        {
            SqlParameter parm = new SqlParameter("@ModuleName", moduleName);
            return GetModule(GET_MODULE_BY_NAME, parm);
        }

        private ModuleInfo GetModule(string cmdText, SqlParameter parm)
        {
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, cmdText, parm))
            {
                if (reader.Read())
                {
                    ModuleInfo module = new ModuleInfo();
                    module.ClassPath = reader["ClassPath"].ToString();
                    module.Assembly = reader["Assembly"].ToString();
                    return module;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 返回模块名称。
        /// </summary>
        /// <param name="moduleId">模块ID。</param>
        /// <returns>模块名称。</returns>
        public string GetModuleName(int moduleId)
        {
            SqlParameter parm = new SqlParameter("@ModuleID", moduleId);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, GET_MODULE_NAME, parm);
            if (o != null && DBNull.Value != null)
            {
                return (string)o;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回指定模块是否存在。
        /// </summary>
        /// <param name="moduleName">模块名。</param>
        /// <returns>是否存在。</returns>
        public bool IsModuleExists(string moduleName)
        {
            SqlParameter parm = new SqlParameter("@ModuleName", moduleName);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_MODULE_EXISTS, parm);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean(o);
            }
            else
            {
                throw new Exception();
            }
        }

        private ModuleInfo FillModuleInfo(SqlDataReader reader)
        {
            ModuleInfo module = new ModuleInfo();
            module.ModuleId = (int)reader["ModuleID"];
            module.ModuleName = reader["ModuleName"].ToString();
            module.DisplayName = reader["DisplayName"].ToString();
            module.ModuleAuthor = reader["ModuleAuthor"].ToString();
            module.Description = reader["Description"].ToString();
            module.WorkingPath = reader["WorkingPath"].ToString();

            return module;
        }
    }
}
