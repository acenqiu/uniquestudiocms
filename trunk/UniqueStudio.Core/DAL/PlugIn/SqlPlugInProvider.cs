﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.PlugIn
{
    internal class SqlPlugInProvider : IPlugIn
    {
        private const string CREATE_PLUGIN = "CreatePlugIn";
        private const string DELETE_PLUGIN = "DeletePlugIn";
        private const string GET_ALL_PLUGIN_INFO = "GetAllPlugIns_Info";
        private const string GET_ALL_PLUGIN_INIT = "GetAllPlugIns_Init";

        /// <summary>
        /// 初始化<see cref="SqlPlugInProvider"/>类的实例
        /// </summary>
        public SqlPlugInProvider()
        {
            //默认构造函数
        }

        public PlugInInfo CreatePlugIn(PlugInInfo plugIn)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PlugInName",plugIn.PlugInName),
                                                    new SqlParameter("@DisplayName",plugIn.DisplayName),
                                                    new SqlParameter("@PlugInAuthor",plugIn.PlugInAuthor),
                                                    new SqlParameter("@Description",plugIn.Description),
                                                    new SqlParameter("@IsEnabled",plugIn.IsEnabled),
                                                    new SqlParameter("@ClassPath",plugIn.ClassPath),
                                                    new SqlParameter("@Assembly",plugIn.Assembly),
                                                    new SqlParameter("@PlugInCategory",plugIn.PlugInCategory),
                                                    new SqlParameter("@PlugInOrdering",plugIn.PlugInOrdering),
                                                    new SqlParameter("@InstallFilePath",plugIn.InstallFilePath),
                                                    new SqlParameter("@Parameters",plugIn.Parameters)};
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

        public bool DeletePlugIn(int plugInId)
        {
            throw new NotImplementedException();
        }

        public PlugInCollection GetAllPlugIns()
        {
            PlugInCollection collection = new PlugInCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_PLUGIN_INFO, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillPlugInInfo(reader, false));
                }
            }
            return collection;
        }

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

        private PlugInInfo FillPlugInInfo(SqlDataReader reader, bool isIncludeParameters)
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
            plugIn.IsEnabled = Convert.ToBoolean(reader["IsEnabled"]);
            plugIn.ClassPath = reader["ClassPath"].ToString();
            plugIn.Assembly = reader["Assembly"].ToString();
            if (reader["PlugInCategory"] != DBNull.Value)
            {
                plugIn.PlugInCategory = reader["PlugInCategory"].ToString();
            }
            plugIn.PlugInOrdering = (int)reader["PlugInOrdering"];
            plugIn.InstallFilePath = reader["InstallFilePath"].ToString();
            if (isIncludeParameters)
            {
                plugIn.Parameters = reader["Parameters"].ToString();
            }
            return plugIn;
        }
    }
}