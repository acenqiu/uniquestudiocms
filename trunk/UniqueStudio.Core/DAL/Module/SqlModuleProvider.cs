using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.Module
{
    internal class SqlModuleProvider : IModule
    {
        private const string CREATE_MODULE = "CreateModule";
        private const string GET_ALL_MODULES = "GetAllModules";
        private const string GET_MODULE_BY_ID = "GetModuleByID";

        public SqlModuleProvider()
        {
            //默认构造函数
        }
 
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

        public ModuleInfo GetModule(int moduleId)
        {
            SqlParameter parm = new SqlParameter("@ModuleID", moduleId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_MODULE_BY_ID, parm))
            {
                if (reader.Read())
                {
                    ModuleInfo module = FillModuleInfo(reader);
                    module.Parameters = reader["Parameters"].ToString();
                    return module;
                }
                else
                {
                    return null;
                }
            }
        }

        public ModuleInfo CreateModule(ModuleInfo module)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@ModuleName",module.ModuleName),
                                                    new SqlParameter("@DisplayName",module.DisplayName),
                                                    new SqlParameter("@ModuleAuthor",module.ModuleAuthor),
                                                    new SqlParameter("@Description",module.Description),
                                                    new SqlParameter("@InstallFilePath",module.InstallFilePath),
                                                    new SqlParameter("@Parameters",module.Parameters)};
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

        private ModuleInfo FillModuleInfo(SqlDataReader reader)
        {
            ModuleInfo module = new ModuleInfo();
            module.ModuleId = (int)reader["ModuleID"];
            module.ModuleName = reader["ModuleName"].ToString();
            module.DisplayName = reader["DisplayName"].ToString();
            module.ModuleAuthor = reader["ModuleAuthor"].ToString();
            module.Description = reader["Description"].ToString();
            module.InstallFilePath = reader["InstallFilePath"].ToString();

            return module;
        }
    }
}
