using System;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.DatabaseHelper;

namespace UniqueStudio.ComContent.DAL
{
    public class AutoSaveProvider
    {
        private const string AUTOSAVE_POST = "AutoSavePost";
        private const string SET_AUTOSAVE_EFT = "SetAutoSaveEft";
        private const string GET_AUTOSAVED_POST = "GetAutoSavedPost";
        private const string IS_POST_SAVED = "IsPostSaved";

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
        public bool AutoSaveFile(Guid userId, PostInfo post, Int64 userUri)
        {
            SqlParameter[] parms = new SqlParameter[] { 
                                                       new SqlParameter("@UserID", userId),
                                                       new SqlParameter("@UserUri",userUri),
                                                       new SqlParameter("@PostUri",post.Uri),
                                                       new SqlParameter("@Title",post.Title),
                                                       new SqlParameter("@SubTitle",post.SubTitle),
                                                       new SqlParameter("@Author",post.Author),
                                                       new SqlParameter("@AddUserName",post.AddUserName),
                                                       new SqlParameter("@Content",post.Content),
                                                       new SqlParameter("@Summary",post.Summary)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, AUTOSAVE_POST, parms) > 0;
        }

        /// <summary>
        /// 根据用户获得自动保存文章
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>文章信息</returns>
        public PostInfo GetAutoSavedPost(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_AUTOSAVED_POST, parm))
            {
                if (reader.Read())
                {
                    PostInfo post = new PostInfo();
                    post.Uri = Convert.ToInt64(reader["PostUri"]);//文章实际发表时Uri，并不是用户的自动保存Uri
                    if (reader["Title"] != DBNull.Value)
                    {
                        post.Title = (string)reader["Title"];
                    }
                    if (reader["SubTitle"] != DBNull.Value)
                    {
                        post.SubTitle = (string)reader["SubTitle"];
                    }
                    if (reader["Author"] != DBNull.Value)
                    {
                        post.Author = (string)reader["Author"];
                    }
                    if (reader["AddUserName"] != DBNull.Value)
                    {
                        post.AddUserName = (string)reader["AddUserName"];
                    }
                    post.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
                    if (reader["Content"] != DBNull.Value)
                    {
                        post.Content = (string)reader["Content"];
                    }
                    if (reader["Summary"] != DBNull.Value)
                    {
                        post.Summary = (string)reader["Summary"];
                    }
                    return post;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsPostSaved(Guid userId)
        {
            SqlParameter parm = new SqlParameter("@UserID", userId);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_POST_SAVED, parm);
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
        /// 设置自动保存文章的有效性
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public bool SetAutoSavePostEft(Guid userId, bool isEffictive)
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@UserID",userId),
                new SqlParameter("@IsEffictive",isEffictive)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, SET_AUTOSAVE_EFT, parms) > 0;
        }
    }
}
