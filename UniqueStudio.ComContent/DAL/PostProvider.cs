using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Core.Category;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Config;
using UniqueStudio.ComContent.Model;
using UniqueStudio.DAL.Uri;
using UniqueStudio.Common.Model;

namespace UniqueStudio.ComContent.DAL
{
    /// <summary>
    /// 执行文章管理的数据库方法
    /// </summary>
    public class PostProvider
    {
        private const string ADD_POST = "AddPost";
        private const string DELETE_POST = "DeletePost";
        private const string EDIT_POST = "EditPost";
        private const string GET_POST = "GetPost";
        private const string GET_POST_TITLE = "GetPostTitle";
        private const string GET_POSTS_COUNT_ALL = "GetPostsCountAll";
        private const string GET_POSTS_COUNT = "GetPostsCount";
        private const string GET_POST_LIST = "GetPostList";
        private const string GET_POST_LIST_ALL = "GetPostListAll";
        private const string GET_POST_LIST_BY_CATID = "GetPostListByCatId";
        private const string GET_POST_LIST_BY_CATID_ALL = "GetPostListByCatIdAll";
        private const string GET_POSTSTAT = "GetPostStat";
        private const string GET_RECENT_POSTS = "GetRecentPosts";
        private const string GET_RECENT_POSTS_ALL = "GetRecentPostsAll";
        private const string INC_POST_READ_COUNT = "IncPostReadCount";
        private const string SEARCH_POSTS_BY_TITLE = "SearchPostsByTitle";
        private const string SEARCH_POSTS_BY_AUTHOR = "SearchPostsByAuthor";
        private const string SEARCH_POSTS_BY_TIME = "SearchPostsByTime";

        private const string GET_CATEGORYINFO_BY_POSTURI = "GetCategoryInfoByPostUri";
        private const string ADD_POST_CATEGORYID = "AddPostCategoryId";
        private const string EDIT_POST_CATEGORYID = "EditPostCategoryId";
        private const string DELETE_POST_CATEGORYID = "DeletePostCategoryId";
        private const string GET_POST_LIST_BY_USER_PERMISSION = "GetPostListByUserPermision";
        private const string GET_POST_LIST_ALL_BY_USER_PERMISSION = "GetPostListAllByUserPermission";

        /// <summary>
        /// 初始化<see cref="ContentProvider"/>类的实例
        /// </summary>
        public PostProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 添加一篇文章
        /// </summary>
        /// <param name="post"><see cref="PostInfo"/></param>
        /// <returns>如果添加成功，返回文章的Uri，否则抛出异常</returns>
        public Int64 AddPost(PostInfo post, bool isPublish)
        {
            Int64 uri = UriProvider.GetNewUri(ResourceType.Article);
            SqlParameter[] postparms = new SqlParameter[]{
                                                    new SqlParameter("@Uri",uri),
                                                    new SqlParameter("@AddUserName",post.AddUserName),
                                                    new SqlParameter("@CreateDate",post.CreateDate),
                                                    new SqlParameter("@Taxis",post.Taxis),
                                                    new SqlParameter("@Title",post.Title),
                                                    new SqlParameter("@SubTitle",post.SubTitle),
                                                    new SqlParameter("PostDisplay",post.PostDisplay),
                                                    new SqlParameter("@Summary",post.Summary),
                                                    new SqlParameter("@Author",post.Author),
                                                    new SqlParameter("@IsRecommend",post.IsRecommend),
                                                    new SqlParameter("@IsHot",post.IsHot),
                                                    new SqlParameter("@IsTop",post.IsTop),
                                                    new SqlParameter("@IsAllowComment",post.IsAllowComment),
                                                    new SqlParameter("@IsPublished",post.IsPublished),
                                                    new SqlParameter("@Content",post.Content),
                                                    new SqlParameter("@Settings",post.Settings)};
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = ADD_POST;
                            foreach (SqlParameter parm in postparms)
                            {
                                cmd.Parameters.Add(parm);
                            }
                            if (cmd.ExecuteNonQuery() <= 0)
                            {
                                trans.Rollback();
                                cmd.Parameters.Clear();
                                return 0;
                            }
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@PostUri", uri);
                            cmd.Parameters.Add("@CategoryID", SqlDbType.Int);
                            cmd.CommandText = ADD_POST_CATEGORYID;
                            foreach (CategoryInfo cate in post.Categories)
                            {
                                cmd.Parameters[1].Value = cate.CategoryId;
                                if (cmd.ExecuteNonQuery() <= 0)
                                {
                                    trans.Rollback();
                                    cmd.Parameters.Clear();
                                    return 0;
                                }
                            }
                            cmd.Parameters.Clear();
                            trans.Commit();
                            return uri;
                        }
                        catch
                        {
                            trans.Rollback();
                            cmd.Parameters.Clear();
                            return 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="uri">文章Uri</param>
        /// <returns></returns>
        public bool DeletePost(Int64 uri)
        {
            SqlParameter parm = new SqlParameter("@Uri", uri);
            if (SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_POST, parm) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 编辑一篇文章
        /// </summary>
        /// <param name="post"><see cref="PostInfo"/></param>
        /// <returns>是否编辑成功</returns>
        public bool EditPost(PostInfo post)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@Uri",post.Uri),
                                                    new SqlParameter("@LastEditUserName",post.AddUserName),
                                                    new SqlParameter("@Taxis",post.Taxis),
                                                    new SqlParameter("@Title",post.Title),
                                                    new SqlParameter("@SubTitle",post.SubTitle),
                                                    new SqlParameter("@Summary",post.Summary),
                                                    new SqlParameter("@Author",post.Author),
                                                    new SqlParameter("@IsRecommend",post.IsRecommend),
                                                    new SqlParameter("@IsHot",post.IsHot),
                                                    new SqlParameter("@IsTop",post.IsTop),
                                                    new SqlParameter("@IsAllowComment",post.IsAllowComment),
                                                    new SqlParameter("@CreateDate",post.CreateDate),
                                                    new SqlParameter("@IsPublished",post.IsPublished),
                                                    new SqlParameter("@PostDisplay",post.PostDisplay),
                                                    new SqlParameter("@Content",post.Content),
                                                    new SqlParameter("@Settings",post.Settings)};
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = EDIT_POST;
                            foreach (SqlParameter parm in parms)
                            {
                                cmd.Parameters.Add(parm);
                            }
                            if (cmd.ExecuteNonQuery() <= 0)
                            {
                                trans.Rollback();
                                cmd.Parameters.Clear();
                                return false;
                            }
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@PostUri", post.Uri);
                            cmd.CommandText = DELETE_POST_CATEGORYID;
                            if (cmd.ExecuteNonQuery() <= 0)
                            {
                                trans.Rollback();
                                cmd.Parameters.Clear();
                                return false;
                            }
                            cmd.Parameters.Add("@CategoryID", SqlDbType.Int);
                            cmd.CommandText = ADD_POST_CATEGORYID;
                            foreach (CategoryInfo cate in post.Categories)
                            {
                                cmd.Parameters[1].Value = cate.CategoryId;
                                if (cmd.ExecuteNonQuery() <= 0)
                                {
                                    trans.Rollback();
                                    cmd.Parameters.Clear();
                                    return false;
                                }
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            cmd.Parameters.Clear();
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回指定文章
        /// </summary>
        /// <param name="uri">文章Uri</param>
        /// <returns><see cref="PostInfo"/></returns>
        public PostInfo GetPost(Int64 uri)
        {
            PostInfo post = new PostInfo();
            post.Uri = uri;
            SqlParameter parm = new SqlParameter("@Uri", uri);
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = GET_POST;
                            cmd.Parameters.Add(parm);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    post.AddUserName = (string)reader["AddUserName"];
                                    post.CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                                    post.Taxis = (int)reader["Taxis"];
                                    post.Title = (string)reader["Title"];
                                    post.SubTitle = (string)reader["SubTitle"];
                                    post.Summary = (string)reader["Summary"];
                                    post.Author = (string)reader["Author"];
                                    if (reader["Settings"] != DBNull.Value)
                                    {
                                        post.Settings = (string)reader["Settings"];
                                    }
                                    post.IsRecommend = Convert.ToBoolean(reader["IsRecommend"].ToString());
                                    post.IsHot = Convert.ToBoolean(reader["IsHot"].ToString());
                                    post.IsTop = Convert.ToBoolean(reader["IsTop"].ToString());
                                    post.IsAllowComment = Convert.ToBoolean(reader["IsAllowComment"].ToString());
                                    post.IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString());
                                    post.Count = (int)reader["Count"];
                                    post.Content = (string)reader["Content"];
                                    post.PostDisplay = Convert.ToInt32(reader["PostDisplay"]);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            cmd.CommandText = GET_CATEGORYINFO_BY_POSTURI;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                CategoryCollection cates = new CategoryCollection();
                                CategoryInfo cate;
                                while (reader.Read())
                                {
                                    cate = new CategoryInfo();
                                    cate.CategoryId = Convert.ToInt32(reader["CategoryID"]);
                                    cate.CategoryName = (string)reader["CategoryName"];
                                    cate.CategoryNiceName = (string)reader["CategoryNiceName"];
                                    if (reader["SubOF"] != DBNull.Value)
                                    {
                                        cate.ParentCategoryId = Convert.ToInt32(reader["SubOf"]);
                                    }
                                    if (reader["Description"] != DBNull.Value)
                                    {
                                        cate.Description = (string)reader["Description"];
                                    }
                                    cates.Add(cate);
                                }
                                post.Categories = cates;
                            }
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            return null;
                        }

                    }
                }
            }
            return post;
        }

        /// <summary>
        /// 获得文章标题
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string GetPostTitle(Int64 uri)
        {
            SqlParameter parm = new SqlParameter("@Uri", uri);
            object o = SqlHelper.ExecuteScalar(GlobalConfig.SqlConnectionString, CommandType.StoredProcedure, GET_POST_TITLE, parm);
            if (o != null && o != DBNull.Value)
            {
                return o.ToString();
            }
            else
            {
                return null;
            }
        }

        public PostStatCollection GetPostStat(bool isByYear)
        {
            PostStatCollection collection = new PostStatCollection();
            SqlParameter parm = new SqlParameter("@IsByYear", isByYear);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_POSTSTAT, parm))
            {
                if (isByYear)
                {
                    while (reader.Read())
                    {
                        PostStatInfo item = new PostStatInfo();
                        item.Year = (int)reader["Year"];
                        item.Count = (int)reader["Count"];
                        collection.Add(item);
                    }
                }
                else
                {
                    while (reader.Read())
                    {
                        PostStatInfo item = new PostStatInfo();
                        item.Year = (int)reader["Year"];
                        item.Month = (int)reader["Month"];
                        item.Count = (int)reader["Count"];
                        collection.Add(item);
                    }
                }
            }
            return collection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postListType"></param>
        /// <returns></returns>
        public int GetPostsCount(PostListType postListType)
        {
            object o = null;
            if (postListType == PostListType.Both)
            {
                o = SqlHelper.ExecuteScalar(GlobalConfig.SqlConnectionString, CommandType.StoredProcedure, GET_POSTS_COUNT_ALL, null);
            }
            else
            {
                SqlParameter parm = new SqlParameter("@IsPublished", postListType == PostListType.PublishedOnly);
                o = SqlHelper.ExecuteScalar(GlobalConfig.SqlConnectionString, CommandType.StoredProcedure, GET_POSTS_COUNT, parm);
            }
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToInt32(o);
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 根据用户权限获取文章
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否获取文章摘要</param>
        /// <param name="postListType">获取文章类型</param>
        /// <param name="IsNeedCategoryInfo">是否需要分类信息</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public PostCollection GetPostListByUserPermission(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, bool IsNeedCategoryInfo, string userName)
        {
            PostCollection collection = new PostCollection(pageSize);
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Transaction = trans;
                        if (postListType == PostListType.Both)
                        {
                            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PageIndex",pageIndex),
                                                    new SqlParameter("@PageSize",pageSize),
                                                    new SqlParameter("@IsIncludeSummary",isIncludeSummary),
                                                    new SqlParameter("@Amount",SqlDbType.Int),
                                                    new SqlParameter("@AddUserName",userName)};
                            parms[0].Direction = ParameterDirection.InputOutput;
                            parms[3].Direction = ParameterDirection.Output;
                            // SqlHelper.PrepareCommand(cmd, conn, CommandType.StoredProcedure, GET_POST_LIST_ALL, parms);
                            foreach (SqlParameter parm in parms)
                            {
                                cmd.Parameters.Add(parm);
                            }
                            cmd.CommandText = GET_POST_LIST_ALL_BY_USER_PERMISSION;
                            try
                            {


                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        collection.Add(FillPostInfo(reader, isIncludeSummary));
                                    }
                                }
                                if (IsNeedCategoryInfo)
                                {
                                    foreach (PostInfo post in collection)
                                    {
                                        cmd.CommandText = GET_CATEGORYINFO_BY_POSTURI;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@Uri", post.Uri);
                                        using (SqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            CategoryCollection cates = new CategoryCollection();
                                            CategoryInfo cate;
                                            while (reader.Read())
                                            {
                                                cate = new CategoryInfo();
                                                cate.CategoryId = Convert.ToInt32(reader["CategoryID"]);
                                                cate.CategoryName = (string)reader["CategoryName"];
                                                cate.CategoryNiceName = (string)reader["CategoryNiceName"];
                                                if (reader["SubOF"] != DBNull.Value)
                                                {
                                                    cate.ParentCategoryId = Convert.ToInt32(reader["SubOf"]);
                                                }
                                                if (reader["Description"] != DBNull.Value)
                                                {
                                                    cate.Description = (string)reader["Description"];
                                                }
                                                cates.Add(cate);
                                            }
                                            post.Categories = cates;
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                trans.Rollback();
                                return null;
                            }
                            collection.PageIndex = (int)parms[0].Value;
                            collection.Amount = (int)parms[3].Value;
                            collection.PageSize = pageSize;
                            cmd.Parameters.Clear();
                        }
                        else
                        {
                            bool isPublished = (postListType == PostListType.PublishedOnly);
                            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PageIndex",pageIndex),
                                                    new SqlParameter("@PageSize",pageSize),
                                                    new SqlParameter("@IsIncludeSummary",isIncludeSummary),
                                                    new SqlParameter("@IsPublished",isPublished),
                                                    new SqlParameter("@Amount",SqlDbType.Int),
                                                    new SqlParameter("@AddUserName",userName)};
                            parms[0].Direction = ParameterDirection.InputOutput;
                            parms[4].Direction = ParameterDirection.Output;
                            //  SqlHelper.PrepareCommand(cmd, conn, CommandType.StoredProcedure, GET_POST_LIST, parms);
                            cmd.CommandText = GET_POST_LIST_BY_USER_PERMISSION;
                            foreach (SqlParameter parm in parms)
                            {
                                cmd.Parameters.Add(parm);
                            }
                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        collection.Add(FillPostInfo(reader, isIncludeSummary));
                                    }
                                }
                                if (IsNeedCategoryInfo)
                                {
                                    foreach (PostInfo post in collection)
                                    {
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@Uri", post.Uri);
                                        cmd.CommandText = GET_CATEGORYINFO_BY_POSTURI;
                                        using (SqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            CategoryCollection cates = new CategoryCollection();
                                            CategoryInfo cate;
                                            while (reader.Read())
                                            {
                                                cate = new CategoryInfo();
                                                cate.CategoryId = Convert.ToInt32(reader["CategoryID"]);
                                                cate.CategoryName = (string)reader["CategoryName"];
                                                cate.CategoryNiceName = (string)reader["CategoryNiceName"];
                                                if (reader["SubOF"] != DBNull.Value)
                                                {
                                                    cate.ParentCategoryId = Convert.ToInt32(reader["SubOf"]);
                                                }
                                                if (reader["Description"] != DBNull.Value)
                                                {
                                                    cate.Description = (string)reader["Description"];
                                                }
                                                cates.Add(cate);
                                            }
                                            post.Categories = cates;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                trans.Rollback();
                                return null;
                            }
                            collection.PageIndex = (int)parms[0].Value;
                            collection.Amount = (int)parms[4].Value;
                            collection.PageSize = pageSize;
                            cmd.Parameters.Clear();
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回文章列表
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否返回文章摘要</param>
        /// <returns>文章列表</returns>
        public PostCollection GetPostList(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, bool IsNeedCategoryInfo)
        {
            PostCollection collection = new PostCollection(pageSize);
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Transaction = trans;
                        if (postListType == PostListType.Both)
                        {
                            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PageIndex",pageIndex),
                                                    new SqlParameter("@PageSize",pageSize),
                                                    new SqlParameter("@IsIncludeSummary",isIncludeSummary),
                                                    new SqlParameter("@Amount",SqlDbType.Int)};
                            parms[0].Direction = ParameterDirection.InputOutput;
                            parms[3].Direction = ParameterDirection.Output;
                            // SqlHelper.PrepareCommand(cmd, conn, CommandType.StoredProcedure, GET_POST_LIST_ALL, parms);
                            foreach (SqlParameter parm in parms)
                            {
                                cmd.Parameters.Add(parm);
                            }
                            cmd.CommandText = GET_POST_LIST_ALL;
                            try
                            {


                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        collection.Add(FillPostInfo(reader, isIncludeSummary));
                                    }
                                }
                                if (IsNeedCategoryInfo)
                                {
                                    foreach (PostInfo post in collection)
                                    {
                                        cmd.CommandText = GET_CATEGORYINFO_BY_POSTURI;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@Uri", post.Uri);
                                        using (SqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            CategoryCollection cates = new CategoryCollection();
                                            CategoryInfo cate;
                                            while (reader.Read())
                                            {
                                                cate = new CategoryInfo();
                                                cate.CategoryId = Convert.ToInt32(reader["CategoryID"]);
                                                cate.CategoryName = (string)reader["CategoryName"];
                                                cate.CategoryNiceName = (string)reader["CategoryNiceName"];
                                                if (reader["SubOF"] != DBNull.Value)
                                                {
                                                    cate.ParentCategoryId = Convert.ToInt32(reader["SubOf"]);
                                                }
                                                if (reader["Description"] != DBNull.Value)
                                                {
                                                    cate.Description = (string)reader["Description"];
                                                }
                                                cates.Add(cate);
                                            }
                                            post.Categories = cates;
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                trans.Rollback();
                                return null;
                            }
                            collection.PageIndex = (int)parms[0].Value;
                            collection.Amount = (int)parms[3].Value;
                            collection.PageSize = pageSize;
                            cmd.Parameters.Clear();
                        }
                        else
                        {
                            bool isPublished = (postListType == PostListType.PublishedOnly);
                            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@PageIndex",pageIndex),
                                                    new SqlParameter("@PageSize",pageSize),
                                                    new SqlParameter("@IsIncludeSummary",isIncludeSummary),
                                                    new SqlParameter("@IsPublished",isPublished),
                                                    new SqlParameter("@Amount",SqlDbType.Int)};
                            parms[0].Direction = ParameterDirection.InputOutput;
                            parms[4].Direction = ParameterDirection.Output;
                            //  SqlHelper.PrepareCommand(cmd, conn, CommandType.StoredProcedure, GET_POST_LIST, parms);
                            cmd.CommandText = GET_POST_LIST;
                            foreach (SqlParameter parm in parms)
                            {
                                cmd.Parameters.Add(parm);
                            }
                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        collection.Add(FillPostInfo(reader, isIncludeSummary));
                                    }
                                }
                                if (IsNeedCategoryInfo)
                                {
                                    foreach (PostInfo post in collection)
                                    {
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@Uri", post.Uri);
                                        cmd.CommandText = GET_CATEGORYINFO_BY_POSTURI;
                                        using (SqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            CategoryCollection cates = new CategoryCollection();
                                            CategoryInfo cate;
                                            while (reader.Read())
                                            {
                                                cate = new CategoryInfo();
                                                cate.CategoryId = Convert.ToInt32(reader["CategoryID"]);
                                                cate.CategoryName = (string)reader["CategoryName"];
                                                cate.CategoryNiceName = (string)reader["CategoryNiceName"];
                                                if (reader["SubOF"] != DBNull.Value)
                                                {
                                                    cate.ParentCategoryId = Convert.ToInt32(reader["SubOf"]);
                                                }
                                                if (reader["Description"] != DBNull.Value)
                                                {
                                                    cate.Description = (string)reader["Description"];
                                                }
                                                cates.Add(cate);
                                            }
                                            post.Categories = cates;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                trans.Rollback();
                                return null;
                            }
                            collection.PageIndex = (int)parms[0].Value;
                            collection.Amount = (int)parms[4].Value;
                            collection.PageSize = pageSize;
                            cmd.Parameters.Clear();
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            return collection;
        }

        /// <summary>
        /// 根据分类ID返回文章
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否返回文章摘要</param>
        /// <param name="postListType">返回文章类型，是否发表</param>
        /// <param name="categoryId">分类ID</param>
        /// <returns>文章列表</returns>
        public PostCollection GetPostListByCatId(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, int categoryId)
        {
            PostCollection collection = new PostCollection(pageSize);

            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@IsIncludeSummary", isIncludeSummary);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    cmd.Parameters.Add("@Amount", SqlDbType.Int);
                    cmd.Parameters["@PageIndex"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@Amount"].Direction = ParameterDirection.Output;

                    if (postListType == PostListType.Both)
                    {
                        cmd.CommandText = GET_POST_LIST_BY_CATID_ALL;
                    }
                    else
                    {
                        bool isPublished = (postListType == PostListType.PublishedOnly);
                        cmd.Parameters.AddWithValue("@IsPublished", isPublished);
                        cmd.CommandText = GET_POST_LIST_BY_CATID;
                    }

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            collection.Add(FillPostInfo(reader, isIncludeSummary));
                        }
                        reader.Close();
                    }
                    collection.PageIndex = (int)cmd.Parameters["@PageIndex"].Value;
                    collection.Amount = (int)cmd.Parameters["@Amount"].Value;
                }
            }
            return collection;
        }
        /// <summary>
        /// 获得近期文章
        /// </summary>
        /// <param name="number">数量</param>
        /// <param name="offset"></param>
        /// <param name="isIncludeSummary"></param>
        /// <param name="postListType"></param>
        /// <returns></returns>
        public PostCollection GetRecentPosts(int number, int offset, bool isIncludeSummary, PostListType postListType)
        {
            PostCollection collection = new PostCollection(number);
            SqlParameter[] parms = null;
            SqlDataReader reader = null;

            if (postListType == PostListType.Both)
            {
                parms = new SqlParameter[]{
                                                    new SqlParameter("@number",number),
                                                    new SqlParameter("@offset",offset),
                                                    new SqlParameter("@IsIncludeSummary",isIncludeSummary)};
                reader = SqlHelper.ExecuteReader(GlobalConfig.SqlConnectionString, CommandType.StoredProcedure, GET_POST_LIST_ALL, parms);
            }
            else
            {
                bool isPublished = (postListType == PostListType.PublishedOnly);
                parms = new SqlParameter[]{
                                                    new SqlParameter("@number",number),
                                                    new SqlParameter("@offset",offset),
                                                    new SqlParameter("@IsIncludeSummary",isIncludeSummary),
                                                    new SqlParameter("@IsPublished",isPublished)};
                reader = SqlHelper.ExecuteReader(GlobalConfig.SqlConnectionString, CommandType.StoredProcedure, GET_RECENT_POSTS, parms);
            }
            while (reader.Read())
            {
                collection.Add(FillPostInfo(reader, isIncludeSummary));
            }
            reader.Close();
            return collection;
        }

        public void IncPostReadCount(long uri)
        {
            SqlParameter parm = new SqlParameter("@Uri", uri);
            SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, INC_POST_READ_COUNT, parm);
        }
        /// <summary>
        /// 通过作者搜索文章
        /// </summary>
        /// <param name="author">作者</param>
        /// <returns>返回文章列表</returns>
        public PostCollection SearchPostsByAuthor(string author)
        {
            SqlParameter parm = new SqlParameter("@Author", author);
            PostCollection collection = new PostCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, SEARCH_POSTS_BY_AUTHOR, parm))
            {
                while (reader.Read())
                {
                    PostInfo post = new PostInfo();
                    post.Uri = (long)reader["Uri"];
                    if (reader["LastEditDate"] != DBNull.Value)
                    {
                        post.LastEditDate = Convert.ToDateTime(reader["LastEditDate"]);
                    }
                    post.Title = (string)reader["Title"];
                    if (reader["SubTitle"] != DBNull.Value)
                    {
                        post.SubTitle = (string)reader["SubTitle"];
                    }
                    if (reader["Author"] != DBNull.Value)
                    {
                        post.Author = (string)reader["Author"];
                    }
                    if (reader["Summary"] != DBNull.Value)
                    {
                        post.Summary = (string)reader["Summary"];
                    }
                    post.PostDisplay = Convert.ToInt32(reader["PostDisplay"]);
                    collection.Add(post);
                }
            }
            return collection;
        }
        /// <summary>
        /// 通过文章标题搜索文章
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <returns>返回文章列表</returns>
        public PostCollection SearchPostsByTitle(string title)
        {
            SqlParameter parm = new SqlParameter("@Title", title);
            PostCollection collection = new PostCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, SEARCH_POSTS_BY_TITLE, parm))
            {
                while (reader.Read())
                {
                    PostInfo post = new PostInfo();
                    post.Uri = (long)reader["Uri"];
                    if (reader["LastEditDate"] != DBNull.Value)
                    {
                        post.LastEditDate = Convert.ToDateTime(reader["LastEditDate"]);
                    }
                    post.Title = (string)reader["Title"];
                    if (reader["SubTitle"] != DBNull.Value)
                    {
                        post.SubTitle = (string)reader["SubTitle"];
                    }
                    if (reader["Author"] != DBNull.Value)
                    {
                        post.Author = (string)reader["Author"];
                    }
                    if (reader["Summary"] != DBNull.Value)
                    {
                        post.Summary = (string)reader["Summary"];
                    }
                    post.PostDisplay = Convert.ToInt32(reader["PostDisplay"]);
                    collection.Add(post);
                }
            }
            return collection;
        }
        /// <summary>
        /// 根据时间获取文章
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回文章列表</returns>
        public PostCollection SearchPostsByTime(DateTime startTime, DateTime endTime)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@StartDate", startTime),
                                                        new SqlParameter("@EndDate",endTime)};
            PostCollection collection = new PostCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, SEARCH_POSTS_BY_TIME, parms))
            {
                while (reader.Read())
                {
                    PostInfo post = new PostInfo();
                    post.Uri = (long)reader["Uri"];
                    if (reader["LastEditDate"] != DBNull.Value)
                    {
                        post.LastEditDate = Convert.ToDateTime(reader["LastEditDate"]);
                    }
                    post.Title = (string)reader["Title"];
                    if (reader["SubTitle"] != DBNull.Value)
                    {
                        post.SubTitle = (string)reader["SubTitle"];
                    }
                    if (reader["Author"] != DBNull.Value)
                    {
                        post.Author = (string)reader["Author"];
                    }
                    if (reader["Summary"] != DBNull.Value)
                    {
                        post.Summary = (string)reader["Summary"];
                    }
                    post.PostDisplay = Convert.ToInt32(reader["PostDisplay"]);
                    collection.Add(post);
                }
            }
            return collection;
        }

        private PostInfo FillPostInfo(SqlDataReader reader, bool isIncludeSummary)
        {
            PostInfo post = new PostInfo();
            post.Uri = (Int64)reader["Uri"];
            post.AddUserName = (string)reader["AddUserName"];
            if (reader["LastEditUserName"] != DBNull.Value)
            {
                post.LastEditUserName = (string)reader["LastEditUserName"];
            }
            post.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
            if (reader["LastEditDate"] != DBNull.Value)
            {
                post.LastEditDate = Convert.ToDateTime(reader["LastEditDate"]);
            }
            post.Taxis = (int)reader["Taxis"];
            post.Title = (string)reader["Title"];
            if (reader["SubTitle"] != DBNull.Value)
            {
                post.SubTitle = (string)reader["SubTitle"];
            }
            if (reader["Author"] != DBNull.Value)
            {
                post.Author = (string)reader["Author"];
            }
            post.IsRecommend = Convert.ToBoolean(reader["IsRecommend"].ToString());
            post.IsHot = Convert.ToBoolean(reader["IsHot"].ToString());
            post.IsTop = Convert.ToBoolean(reader["IsTop"].ToString());
            post.IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString());
            if (isIncludeSummary)
            {
                post.Summary = (string)reader["Summary"];
            }
            post.Count = (int)reader["Count"];
            post.PostDisplay = Convert.ToInt32(reader["PostDisplay"]);
            if (reader["Settings"] != DBNull.Value)
            {
                post.Settings = (string)reader["Settings"];
            }
            return post;
        }
    }
}
