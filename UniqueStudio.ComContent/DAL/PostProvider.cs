//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：执行文章管理的数据库方法。
// 完成日期：2010年03月21日
// 版本：v0.8
// 作者：任浩玮
//=================================================================
using System;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.Uri;

namespace UniqueStudio.ComContent.DAL
{
    /// <summary>
    /// 执行文章管理的数据库方法。
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
        /// 初始化<see cref="ContentProvider"/>类的实例。
        /// </summary>
        public PostProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 添加一篇文章。
        /// </summary>
        /// <param name="post">文章信息。</param>
        /// <returns>如果添加成功，返回文章的Uri，否则返回0。</returns>
        public long AddPost(PostInfo post)
        {
            long uri = UriProvider.GetNewUri(ResourceType.Article);
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@Uri",uri),
                                                    new SqlParameter("@SiteID",post.SiteId),
                                                    new SqlParameter("@AddUserName",post.AddUserName),
                                                    new SqlParameter("@NewsImage",post.NewsImage),
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
                    using (SqlCommand cmd = new SqlCommand(ADD_POST, conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parms);

                        try
                        {
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
        /// 删除一篇文章。
        /// </summary>
        /// <param name="uri">文章Uri。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeletePost(long uri)
        {
            SqlParameter parm = new SqlParameter("@Uri", uri);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_POST, parm) > 0;
        }

        /// <summary>
        /// 编辑一篇文章。
        /// </summary>
        /// <param name="post">文章信息。</param>
        /// <returns>是否编辑成功。</returns>
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
                        cmd.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            cmd.CommandText = EDIT_POST;
                            cmd.Parameters.AddRange(parms);
                            if (cmd.ExecuteNonQuery() <= 0)
                            {
                                trans.Rollback();
                                cmd.Parameters.Clear();
                                return false;
                            }

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@PostUri", post.Uri);
                            cmd.CommandText = DELETE_POST_CATEGORYID;
                            cmd.ExecuteNonQuery(); //不判断是否大于0，以避免原文章没有选择分类时报错。

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
        /// 返回指定文章。
        /// </summary>
        /// <param name="uri">文章Uri。</param>
        /// <returns>文章信息。</returns>
        public PostInfo GetPost(long uri)
        {
            PostInfo post = new PostInfo();
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandText = GET_POST;
                    cmd.Parameters.AddWithValue("@Uri", uri);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            post = FillPostInfo(reader, true);
                            post.Content = (string)reader["Content"];
                            post.PostDisplay = Convert.ToInt32(reader["PostDisplay"]);
                            if (reader["NewsImage"] != DBNull.Value)
                            {
                                post.NewsImage = (string)reader["NewsImage"];
                            }
                        }
                        else
                        {
                            return null;
                        }

                        reader.Close();
                    }

                    cmd.CommandText = GET_CATEGORYINFO_BY_POSTURI;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Uri", uri);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        post.Categories = new CategoryCollection();
                        CategoryInfo category;
                        while (reader.Read())
                        {
                            category = new CategoryInfo();
                            category.CategoryId = (int)reader["CategoryID"];
                            category.CategoryName = (string)reader["CategoryName"];
                            category.CategoryNiceName = (string)reader["CategoryNiceName"];
                            post.Categories.Add(category);
                        }
                        reader.Close();
                    }
                    return post;
                }
            }
        }

        /// <summary>
        /// 获得指定文章标题。
        /// </summary>
        /// <param name="uri">文章Uri。</param>
        /// <returns>文章标题。</returns>
        public string GetPostTitle(long uri)
        {
            SqlParameter parm = new SqlParameter("@Uri", uri);
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, GET_POST_TITLE, parm);
            if (o != null && o != DBNull.Value)
            {
                return o.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回文章数量统计信息。
        /// </summary>
        /// <param name="isByYear">是否按年统计文章数量。</param>
        /// <returns>文章数量信息的集合。</returns>
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
        /// 根据用户权限获取文章。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="pageIndex">页码，从1起始。</param>
        /// <param name="pageSize">每页条目数。</param>
        /// <param name="isIncludeSummary">是否获取文章摘要。</param>
        /// <param name="postListType">文章类型。</param>
        /// <param name="IsNeedCategoryInfo">是否返回分类信息。</param>
        /// <param name="userName">用户名。</param>
        /// <returns>文章列表。</returns>
        public PostCollection GetPostListByUserPermission(int siteId, int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, bool IsNeedCategoryInfo, string userName)
        {
            PostCollection collection = new PostCollection(pageSize);
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@SiteID", siteId);
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@IsIncludeSummary", isIncludeSummary);
                    cmd.Parameters.Add("@Amount", SqlDbType.Int);
                    cmd.Parameters["@PageIndex"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@Amount"].Direction = ParameterDirection.Output;

                    if (postListType == PostListType.Both)
                    {
                        if (userName == null)
                        {
                            cmd.CommandText = GET_POST_LIST_ALL;
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@AddUserName", userName);
                            cmd.CommandText = GET_POST_LIST_ALL_BY_USER_PERMISSION;
                        }
                    }
                    else
                    {
                        bool isPublished = (postListType == PostListType.PublishedOnly);
                        cmd.Parameters.AddWithValue("@IsPublished", isPublished);

                        if (userName == null)
                        {
                            cmd.CommandText = GET_POST_LIST;
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@AddUserName", userName);
                            cmd.CommandText = GET_POST_LIST_BY_USER_PERMISSION;
                        }
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            collection.Add(FillPostInfo(reader, isIncludeSummary));
                        }
                        reader.Close();
                    }

                    collection.PageIndex = (int)cmd.Parameters["@PageIndex"].Value;
                    collection.Amount = (int)cmd.Parameters["@Amount"].Value;
                    collection.PageSize = pageSize;

                    if (IsNeedCategoryInfo)
                    {
                        cmd.CommandText = GET_CATEGORYINFO_BY_POSTURI;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@Uri", SqlDbType.BigInt);

                        foreach (PostInfo post in collection)
                        {
                            cmd.Parameters[0].Value = post.Uri;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                post.Categories = new CategoryCollection();
                                CategoryInfo category;
                                while (reader.Read())
                                {
                                    category = new CategoryInfo();
                                    category.CategoryId = (int)reader["CategoryID"];
                                    category.CategoryName = (string)reader["CategoryName"];
                                    category.CategoryNiceName = (string)reader["CategoryNiceName"];
                                    post.Categories.Add(category);
                                }
                                reader.Close();
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回文章列表。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="pageIndex">页码，从1起始。</param>
        /// <param name="pageSize">每页条目数。</param>
        /// <param name="isIncludeSummary">是否返回文章摘要。</param>
        /// <param name="postListType">文章类型。</param>
        /// <param name="IsNeedCategoryInfo">是否返回分类信息。</param>
        /// <returns>文章列表。</returns>
        public PostCollection GetPostList(int siteId, int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, bool IsNeedCategoryInfo)
        {
            return GetPostListByUserPermission(siteId, pageIndex, pageSize, isIncludeSummary, postListType, IsNeedCategoryInfo, null);
        }

        /// <summary>
        /// 根据分类ID返回文章。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="pageIndex">页码，从1起始。</param>
        /// <param name="pageSize">每页条目数。</param>
        /// <param name="isIncludeSummary">是否返回文章摘要。</param>
        /// <param name="postListType">文章类型。</param>
        /// <param name="categoryId">分类ID。</param>
        /// <returns>文章列表。</returns>
        public PostCollection GetPostListByCatId(int siteId, int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, int categoryId)
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
        /// 返回最近的文章列表。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="number">条目数。</param>
        /// <param name="offset">偏移数。</param>
        /// <param name="isIncludeSummary">是否返回摘要。</param>
        /// <param name="postListType">文章类型。</param>
        /// <returns>文章列表。</returns>
        public PostCollection GetRecentPosts(int siteId, int number, int offset, bool isIncludeSummary, PostListType postListType)
        {
            PostCollection collection = new PostCollection(number);
            SqlParameter[] parms = null;
            SqlDataReader reader = null;

            if (postListType == PostListType.Both)
            {
                parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteID",siteId),
                                                    new SqlParameter("@number",number),
                                                    new SqlParameter("@offset",offset),
                                                    new SqlParameter("@IsIncludeSummary",isIncludeSummary)};
                reader = SqlHelper.ExecuteReader(GlobalConfig.SqlConnectionString, CommandType.StoredProcedure, GET_POST_LIST_ALL, parms);
            }
            else
            {
                bool isPublished = (postListType == PostListType.PublishedOnly);
                parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteID",siteId),
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
            reader.Dispose();
            return collection;
        }

        /// <summary>
        /// 将指定文字的阅读数量增加1。
        /// </summary>
        /// <param name="uri">文章Uri。</param>
        public void IncPostReadCount(long uri)
        {
            SqlParameter parm = new SqlParameter("@Uri", uri);
            SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, INC_POST_READ_COUNT, parm);
        }

        /// <summary>
        /// 返回作者署名中包含指定文字的文章列表。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="author">作者署名关键词。</param>
        /// <returns>文章列表。</returns>
        public PostCollection SearchPostsByAuthor(int siteId, string author)
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
        /// 返回标题中包含指定文字的文章列表。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="title">文章标题关键字。</param>
        /// <returns>文章列表。</returns>
        public PostCollection SearchPostsByTitle(int siteId, string title)
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
        /// 返回指定时间段内的文章列表。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="startTime">开始时间。</param>
        /// <param name="endTime">结束时间。</param>
        /// <returns>文章列表。</returns>
        public PostCollection SearchPostsByTime(int siteId, DateTime startTime, DateTime endTime)
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
            post.Uri = (long)reader["Uri"];
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
            if (reader["NewsImage"] != DBNull.Value)
            {
                post.NewsImage = (string)reader["NewsImage"];
            }
            return post;
        }
    }
}
