using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.ComContent.DAL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Core.User;
using UniqueStudio.Core.Permission;

namespace UniqueStudio.ComContent.BLL
{
    /// <summary>
    /// 提供文章管理的方法
    /// </summary>
    public class PostManager
    {
        PostProvider provider;
        PostPermissionManager ppm;
        UserInfo realUser;
        UserManager um;

        /// <summary>
        /// 初始化<see cref="Blog"/>类的实例
        /// </summary>
        public PostManager()
        {
            //默认构造函数
            provider = new PostProvider();
        }

        /// <summary>
        /// 发表一篇文章
        /// </summary>
        /// <param name="post"><see cref="PostInfo"/></param>
        /// <param name="execId">执行发表的用户ID</param>
        /// <returns>如果发表成功，返回文章的Uri，否则返回错误代码</returns>
        public Int64 AddPost(UserInfo user, PostInfo post)
        {
            //权限检查
            um = new UserManager(user);
            ppm = new PostPermissionManager();
            realUser = um.GetUserInfo(user.UserId);
            if (!ppm.HasAddPermission(realUser))
            {
                throw new InvalidPermissionException("The user doesn't have addpost permission");
            }
            //参数检查
            if (string.IsNullOrEmpty(post.Title))
            {
                return 0;
            }

            try
            {
                if (post.NewsImage == null)
                {
                    post.NewsImage = "";
                }
                Int64 uri = provider.AddPost(post, true);
                if (uri == 0)
                {
                    return 0;
                }
                else
                {
                    //StatisticsCache.RefreshCache();
                    //新闻图片更新
                    if (post.NewsImage != null && post.NewsImage != string.Empty)
                    {
                        foreach (CategoryInfo item in post.Categories)
                        {
                            if (item.CategoryId == PictureNewsManager.CATEGOEY_ID)
                            {
                                PictureNewsManager.UpdatePictureNews();
                                return uri;
                            }
                        }
                    }
                    return uri;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="uri">文章Uri</param>
        /// <param name="execId">执行删除的用户ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeletePost(UserInfo user, Int64 uri)
        {
            um = new UserManager(user);
            ppm = new PostPermissionManager();
            realUser = um.GetUserInfo(user.UserId);
            //权限检查
            ppm = new PostPermissionManager();
            if (!ppm.HasDeletePermission(realUser, uri))
            {
                throw new InvalidPermissionException("The user doesn't have deletepost permission");
            }
            try
            {
                bool r = provider.DeletePost(uri);
                //StatisticsCache.RefreshCache();
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 编辑一篇文章
        /// </summary>
        /// <param name="post"><see cref="PostInfo"/></param>
        /// <param name="execId">执行编辑的用户ID</param>
        /// <returns>是否编辑成功</returns>
        public bool EditPost(UserInfo user, PostInfo post)
        {
            um = new UserManager(user);
            ppm = new PostPermissionManager();
            realUser = um.GetUserInfo(user.UserId);
            //权限检查
            ppm = new PostPermissionManager();
            if (!ppm.HasEditPermission(realUser, post.Uri))
            {
                throw new InvalidPermissionException("The user doesn't have edit permission");
            }
            //参数检查
            if (string.IsNullOrEmpty(post.Title))
            {
                return false;
            }
            if (string.IsNullOrEmpty(post.Content))
            {
                return false;
            }
            try
            {
                bool r = provider.EditPost(post);
                //StatisticsCache.RefreshCache();
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 返回指定文章
        /// </summary>
        /// <param name="uri">文章Uri</param>
        /// <returns><see cref="PostInfo"/></returns>
        public PostInfo GetPost(Int64 uri)
        {
            try
            {
                return provider.GetPost(uri);
            }
            catch
            {
                return null;
            }
        }

        public string GetPostTitle(Int64 uri)
        {
            try
            {
                return provider.GetPostTitle(uri);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 返回文章列表
        /// </summary>
        /// <param name="quantity">返回的最新文章数</param>
        /// <returns></returns>
        public PostCollection GetPostList(int quantity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回文章列表
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否返回文章摘要</param>
        /// <param name="postListType"></param>
        /// <returns>文章列表</returns>
        public PostCollection GetPostList(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, bool IsNeedCategoryInfo, UserInfo user)
        {
            if (user.UserName == string.Empty || user.UserName == null)
            {
                return null;
            }
            if (pageIndex <= 0 || pageSize <= 0)
            {
                return null;
            }
            if (PermissionManager.HasPermission(user, "EditAllDraftAPost"))
            {
                try
                {
                    return provider.GetPostList(pageIndex, pageSize, isIncludeSummary, postListType, IsNeedCategoryInfo);
                }
                catch
                {
                    return null;
                }
            }
            else if (PermissionManager.HasPermission(user, "EditOwnDraftAPost"))
            {
                try
                {
                    return provider.GetPostListByUserPermission(pageIndex, pageSize, isIncludeSummary, postListType, IsNeedCategoryInfo, user.UserName);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 返回文章列表
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否返回文章摘要</param>
        /// <param name="postListType"></param>
        /// <param name="date"></param>
        /// <returns>文章列表</returns>
        public PostCollection GetPostList(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, DateTime date)
        {
            //粒度：granularity，增加这个参数，指定是根据年、月、日。
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回文章列表
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否返回文章摘要</param>
        /// <param name="postListType"></param>
        /// <param name="year">年</param>
        /// <returns>文章列表</returns>
        public PostCollection GetPostList(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, int year)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回文章列表
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否返回文章摘要</param>
        /// <param name="postListType"></param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>文章列表</returns>
        public PostCollection GetPostList(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, int year, int month)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回文章列表
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否返回文章摘要</param>
        /// <param name="postListType"></param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns>文章列表</returns>
        public PostCollection GetPostList(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, int year, int month, int day)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  返回文章列表
        /// </summary>
        /// <param name="pageIndex">页码，从1起始</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="isIncludeSummary">是否返回文章摘要</param>
        /// <param name="postListType"></param>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>文章列表</returns>
        public PostCollection GetPostList(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public PostCollection GetPostListByCatId(int pageIndex, int pageSize, bool isIncludeSummary, PostListType postListType, int categoryId)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                return null;
            }
            try
            {
                return provider.GetPostListByCatId(pageIndex, pageSize, isIncludeSummary, postListType, categoryId);
            }
            catch
            {
                return null;
            }
        }

        public PostStatCollection GetPostStat(bool isByYear)
        {
            try
            {
                return provider.GetPostStat(isByYear);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        public PostCollection GetRecentPosts(int number, int offset, bool isIncludeSummary, PostListType postListType)
        {
            try
            {
                return provider.GetRecentPosts(number, offset, isIncludeSummary, postListType);
            }
            catch
            {
                return null;
            }
        }

        public void IncPostReadCount(long uri)
        {
            provider.IncPostReadCount(uri);
        }

        public PostCollection SearchPostsByAuthor(string author)
        {
            if (string.IsNullOrEmpty(author))
            {
                throw new ArgumentNullException(author);
            }
            //TODO:正则验证
            try
            {
                return provider.SearchPostsByAuthor(author);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        public PostCollection SearchPostsByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            //正则表达式验证
            try
            {
                return provider.SearchPostsByTitle(title);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        public PostCollection SearchPostsByTime(DateTime startTime, DateTime endTime)
        {
            if (startTime == new DateTime() && endTime == new DateTime())
            {
                throw new ArgumentNullException();
            }
            if (startTime >= endTime)
            {
                throw new ArgumentException("开始时间不能晚于结束时间！");
            }
            if (startTime == new DateTime())
            {
                startTime = new DateTime(1900, 1, 1);
            }
            if (endTime == new DateTime())
            {
                endTime = DateTime.Now.AddDays(1d);
            }

            try
            {
                return provider.SearchPostsByTime(startTime, endTime);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }
    }
}
