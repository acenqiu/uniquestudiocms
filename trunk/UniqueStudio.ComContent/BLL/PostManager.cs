//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供文章管理的方法。
// 完成日期：2010年03月21日
// 版本：v0.8
// 作者：任浩玮
//=================================================================
using System;
using System.Data.Common;

using UniqueStudio.ComContent.DAL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Permission;
using UniqueStudio.Core.User;

namespace UniqueStudio.ComContent.BLL
{
    /// <summary>
    /// 提供文章管理的方法。
    /// </summary>
    public class PostManager
    {
        private PostProvider provider = new PostProvider();

        /// <summary>
        /// 初始化<see cref="PostManager"/>类的实例。
        /// </summary>
        public PostManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 发表一篇文章。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="post">文章信息。</param>
        /// <returns>如果发表成功，返回文章的Uri，否则返回零。</returns>
        public long AddPost(UserInfo currentUser, PostInfo post)
        {
            Validator.CheckNotPositive(post.SiteId, "post.SiteId");
            Validator.CheckStringNull(post.Title, "post.Title");
            Validator.CheckNegative(post.PostDisplay, "post.PostDisplay");

            //权限检查
            if (!PostPermissionManager.HasAddPermission(currentUser, post.SiteId))
            {
                throw new InvalidPermissionException("AddPost", "发表文章");
            }

            try
            {
                if (post.NewsImage == null)
                {
                    post.NewsImage = string.Empty;
                }
                Int64 uri = provider.AddPost(post);
                if (uri == 0)
                {
                    return 0;
                }
                else
                {
                    //TODO:此次需要修改，通过OnPostAdded事件触发

                    //新闻图片更新
                    if (string.IsNullOrEmpty(post.NewsImage))
                    {
                        foreach (CategoryInfo item in post.Categories)
                        {
                            if (item.CategoryId == PictureNewsManager.CATEGOEY_ID)
                            {
                                PictureNewsManager.UpdatePictureNews();
                                break;
                            }
                        }
                    }
                    return uri;
                }
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 删除一篇文章。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="uri">文章Uri。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeletePost(UserInfo currentUser, long uri)
        {
            if (uri <= 119900101000000)
            {
                throw new ArgumentException();
            }
            if (!PostPermissionManager.HasDeletePermission(currentUser, uri))
            {
                throw new InvalidPermissionException("DeletePost", "删除文章");
            }

            try
            {
                return provider.DeletePost(uri);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 编辑指定文章。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="post">文章信息。</param>
        /// <returns>是否编辑成功。</returns>
        public bool EditPost(UserInfo currentUser, PostInfo post)
        {
            if (post.Uri <= 119900101000000)
            {
                throw new ArgumentException();
            }
            Validator.CheckStringNull(post.Title, "post.Title");
            Validator.CheckNegative(post.PostDisplay, "post.PostDisplay");

            if (!PostPermissionManager.HasEditPermission(currentUser, post.Uri))
            {
                throw new InvalidPermissionException("EditPost", "编辑文章");
            }

            try
            {
                return provider.EditPost(post);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定文章。
        /// </summary>
        /// <remarks>仅用于返回已发布文章。</remarks>
        /// <param name="uri">文章Uri。</param>
        /// <returns>文章信息。</returns>
        public PostInfo GetPost(long uri)
        {
            return GetPost(null, uri);
        }

        /// <summary>
        /// 返回指定文章。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="uri">文章Uri。</param>
        /// <returns>文章信息。</returns>
        public PostInfo GetPost(UserInfo currentUser, long uri)
        {
            if (uri <= 119900101000000)
            {
                throw new ArgumentException();
            }

            try
            {
                PostInfo post = provider.GetPost(uri);
                if (post != null)
                {
                    if (!PostPermissionManager.HasViewPermission(currentUser, post.SiteId, post.AddUserName, post.IsPublished))
                    {
                        throw new InvalidPermissionException();
                    }
                    else
                    {
                        return post;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (InvalidPermissionException)
            {
                throw;
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回文章列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="pageIndex">页码，从1起始。</param>
        /// <param name="pageSize">每页条目数。</param>
        /// <param name="isIncludeSummary">是否返回文章摘要。</param>
        /// <param name="postListType">文章类型。</param>
        /// <param name="IsNeedCategoryInfo">是否返回分类信息。</param>
        /// <returns>文章列表。</returns>
        public PostCollection GetPostList(UserInfo currentUser, int siteId, int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, bool IsNeedCategoryInfo)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckNotPositive(pageSize, "pageSize");
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            try
            {
                if (PermissionManager.HasPermission(currentUser, siteId, "EditAllDraftAPost"))
                {
                    return provider.GetPostList(siteId, pageIndex, pageSize, isIncludeSummary, postListType, IsNeedCategoryInfo);
                }
                else if (PermissionManager.HasPermission(currentUser, siteId, "EditOwnDraftAPost"))
                {
                    return provider.GetPostListByUserPermission(siteId, pageIndex, pageSize, isIncludeSummary, postListType, IsNeedCategoryInfo, currentUser.UserName);
                }
                else
                {
                    throw new InvalidPermissionException("查看文章");
                }
            }
            catch (InvalidPermissionException)
            {
                throw;
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 根据分类ID返回文章列表。
        /// </summary>
        /// <param name="pageIndex">页码，从1起始。</param>
        /// <param name="pageSize">每页条目数。</param>
        /// <param name="isIncludeSummary">是否返回文章摘要。</param>
        /// <param name="postListType">文章类型。</param>
        /// <param name="categoryId">分类ID。</param>
        /// <returns>文章列表。</returns>
        public PostCollection GetPostListByCatId(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, int categoryId)
        {
            Validator.CheckNotPositive(pageSize, "pageSize");
            Validator.CheckNotPositive(categoryId, "categoryId");
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            try
            {
                return provider.GetPostListByCatId(pageIndex, pageSize, isIncludeSummary, postListType, categoryId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回文章数量统计信息。
        /// </summary>
        /// <param name="isByYear">是否按年统计文章数量。</param>
        /// <returns>文章数量信息的集合。</returns>
        public PostStatCollection GetPostStat(bool isByYear)
        {
            try
            {
                return provider.GetPostStat(isByYear);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
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
            Validator.CheckNotPositive(siteId, "siteId");

            try
            {
                return provider.GetRecentPosts(siteId, number, offset, isIncludeSummary, postListType);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 将指定文字的阅读数量增加1。
        /// </summary>
        /// <param name="uri">文章Uri。</param>
        public void IncPostReadCount(long uri)
        {
            try
            {
                provider.IncPostReadCount(uri);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
            }
        }

        /// <summary>
        /// 返回作者署名中包含指定文字的文章列表。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="author">作者署名关键词。</param>
        /// <returns>文章列表。</returns>
        public PostCollection SearchPostsByAuthor(int siteId, string author)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(author, "author");

            try
            {
                return provider.SearchPostsByAuthor(siteId, author);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回标题中包含指定文字的文章列表。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="title">文章标题关键字。</param>
        /// <returns>文章列表。</returns>
        public PostCollection SearchPostsByTitle(int siteId, string title)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(title, "title");

            try
            {
                return provider.SearchPostsByTitle(siteId, title);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定时间段内的文章列表。
        /// </summary>
        /// <remarks>开始时间和结束时间不能同时为“空”（时间的最小值）。</remarks>
        /// <param name="siteId">网站ID。</param>
        /// <param name="startTime">开始时间。</param>
        /// <param name="endTime">结束时间。</param>
        /// <returns>文章列表。</returns>
        public PostCollection SearchPostsByTime(int siteId, DateTime startTime, DateTime endTime)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            if (startTime == DateTime.MinValue && endTime == DateTime.MinValue)
            {
                throw new ArgumentNullException();
            }
            if (startTime >= endTime)
            {
                throw new ArgumentException("开始时间不能晚于结束时间！");
            }
            if (startTime == DateTime.MinValue)
            {
                startTime = new DateTime(1900, 1, 1);
            }
            if (endTime == DateTime.MinValue)
            {
                endTime = DateTime.Now.AddDays(1d);
            }

            try
            {
                return provider.SearchPostsByTime(siteId, startTime, endTime);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }
    }
}
