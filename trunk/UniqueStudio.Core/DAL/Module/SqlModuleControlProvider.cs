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
    internal class SqlModuleControlProvider : IModuleControl
    {
        private const string CREATE_MODULE_CONTROL = "CreateModuleControl";
        private const string GET_ALL_MODULE_CONTROLS = "GetAllModuleControls";
        private const string GET_MODULE_CONTROL = "GetModuleControl";
        private const string UPDATE_CONTROL_PARAMETERS = "UpdateControlParameters";

        public SqlModuleControlProvider()
        {
            //默认构造函数
        }

        public ModuleControlInfo CreateModuleControl(ModuleControlInfo moduleControl)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@ControlID",moduleControl.ControlId),
                                                    new SqlParameter("@ModuleID",moduleControl.ModuleId),
                                                    new SqlParameter("@IsEnabled",moduleControl.IsEnabled),
                                                    new SqlParameter("@Parameters",moduleControl.Parameters)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_MODULE_CONTROL, parms);
            if (o != null && o != DBNull.Value)
            {
                return moduleControl;
            }
            else
            {
                throw new Exception();
            }
        }

        public ModuleControlCollection GetAllModuleControls()
        {
            ModuleControlCollection collection = new ModuleControlCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_MODULE_CONTROLS, null))
            {
                while (reader.Read())
                {
                    //去除返回 parameters
                    collection.Add(FillModuleControlInfo(reader));
                }
            }
            return collection;
        }

        public ModuleControlInfo GetModuleControl(string controlId)
        {
            SqlParameter parm = new SqlParameter("@ControlID", controlId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_MODULE_CONTROL, parm))
            {
                if (reader.Read())
                {
                    return FillModuleControlInfo(reader);
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public bool UpdateControlParameters(string controlId, string parameters)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                         new SqlParameter("@ControlID",controlId),
                                                         new SqlParameter("@Parameters",parameters)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_CONTROL_PARAMETERS, parms) > 0;
        }

        private ModuleControlInfo FillModuleControlInfo(SqlDataReader reader)
        {
            ModuleControlInfo control = new ModuleControlInfo();
            control.ControlId = reader["ControlID"].ToString();
            control.ModuleId = (int)reader["ModuleID"];
            control.ModuleName = reader["ModuleName"].ToString();
            control.IsEnabled = Convert.ToBoolean(reader["IsEnabled"].ToString());
            control.Parameters = reader["Parameters"].ToString();
            return control;
        }

    }
}
