using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.DAL.Uri;
using UniqueStudio.Common.Config;

namespace UniqueStudio.ComContent.DAL
{
    public class AutoSaveProvider
    {
        private const string GETUSERURI = "GetUserUri";
        private const string SETAUTOSAVEEFT = "SetAutoSaveEft";
        private const string GETAUTOSAVEDFILE = "GetAutoSavedFile";
        private const string ADDAUTOSAVEFILE = "AddAutoSaveFile";
        private const string UPDATEAUTOSAVEFILE = "UpdateAutoSaveFile";

        public AutoSaveProvider()
        {
        }

        /// <summary>
        /// 文章自动保存
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="post">自动保存的文章信息</param>
        /// <param name="postUri">文章实际保存Uri</param>
        /// <returns></returns>
        public bool AutoSaveFile(Guid userId, PostInfo post, Int64 postUri)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            SqlParameter[] parms = new SqlParameter[] { 
                                                       new SqlParameter("@Title",post.Title),
                                                       new SqlParameter("@SubTitle",post.SubTitle),
                                                       new SqlParameter("@Author",post.Author),
                                                       new SqlParameter("@AddUserName",post.AddUserName),
                                                       new SqlParameter("@Content",post.Content),
                                                       new SqlParameter("@Summary",post.Summary),
                                                       new SqlParameter("@PostUri",postUri)};
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = GETUSERURI;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(parm);
                    try
                    {

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cmd.Parameters.AddRange(parms);
                            cmd.Parameters.Add("@UserUri", SqlDbType.BigInt);
                            if (reader.Read())
                            {
                                if (reader["UserUri"] != DBNull.Value)
                                {
                                    post.Uri = Convert.ToInt64(reader["UserUri"]);
                                }
                                else
                                {
                                    post.Uri = 0;
                                }
                            }
                            else
                            {
                                post.Uri = 0;
                            }
                        }
                        if (post.Uri != 0)
                        {
                            cmd.Parameters[8].Value = post.Uri;
                            cmd.CommandText = UPDATEAUTOSAVEFILE;
                            return cmd.ExecuteNonQuery() > 0;
                        }
                        else
                        {
                            cmd.Parameters[8].Value = UriProvider.GetNewUri(ResourceType.AutoSave);
                            cmd.CommandText = ADDAUTOSAVEFILE;
                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 设置自动保存文章的有效性
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public bool SetAutoSaveFileEft(Guid userId, bool isEffictive)
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@UserID",userId),
                new SqlParameter("@IsEffictive",isEffictive)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, SETAUTOSAVEEFT, parms) > 0;
        }

        /// <summary>
        /// 根据用户获得自动保存文章
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>文章信息</returns>
        public PostInfo GetAutoSavedFile(Guid userId)
        {
            PostInfo post = new PostInfo();
            SqlParameter parm = new SqlParameter("@UserID", userId);
            try
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GETAUTOSAVEDFILE, parm);
                if (reader.Read())
                {
                    post.Uri = Convert.ToInt64(reader["PostUri"]);//文章实际发表时Uri，并不是用户的自动保存Uri
                    post.Title = (string)reader["Title"];
                    post.Content = (string)reader["Content"];
                    post.Author = (string)reader["Author"];
                    post.AddUserName = (string)reader["AddUserName"];
                    post.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
                    //post.LastEditDate = Convert.ToDateTime(reader["LastEditDate"]);
                    return post;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
