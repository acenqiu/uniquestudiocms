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
    /// <summary>
    /// 提供模块控件管理在Sql Server上的实现方法
    /// </summary>
    internal class SqlModuleControlProvider : IModuleControl
    {
        private const string CREATE_MODULE_CONTROL = "CreateModuleControl";
        private const string GET_ALL_MODULE_CONTROLS = "GetAllModuleControls";
        private const string GET_MODULE_CONTROL = "GetModuleControl";
        private const string UPDATE_CONTROL_PARAMETERS = "UpdateControlParameters";

        /// <summary>
        /// 初始化<see cref="SqlModuleControlProvider"/>类的实例
        /// </summary>
        public SqlModuleControlProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="moduleControl">控件信息</param>
        /// <returns>如果创建成功，返回控件信息，否则返回空</returns>
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

        /// <summary>
        /// 返回所有控件
        /// </summary>
        /// <returns>控件的集合</returns>
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

        /// <summary>
        /// 返回指定控件
        /// </summary>
        /// <param name="controlId">待获取控件ID</param>
        /// <returns>控件信息，如果不存在返回空</returns>
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

        /// <summary>
        /// 更新控件配置信息
        /// </summary>
        /// <remarks>该方法可能在后续版本中与UpdateModuleControl方法合并</remarks>
        /// <param name="controlId">待更新控件ID</param>
        /// <param name="parameters">控件配置信息（xml格式）</param>
        /// <returns>是否更新成功</returns>
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
