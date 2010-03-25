//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供对文章权限管理的方法。
// 完成日期：2010年03月21日
// 版本：v0.8
// 作者：任浩玮
//=================================================================
using System.Data.Common;
using UniqueStudio.ComContent.Model;
using UniqueStudio.ComContent.DAL;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Permission;
using System;

namespace UniqueStudio.ComContent.BLL
{
    /// <summary>
    /// 提供对文章权限管理的方法。
    /// </summary>
    public class PostPermissionManager
    {
        private static readonly PostProvider provider = new PostProvider();

        /// <summary>
        /// 判断用户是否具有添加文章权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否具有添加文章权限。</returns>
        public static bool HasAddPermission(UserInfo currentUser, int siteId)
        {
            return PermissionManager.HasPermission(currentUser, siteId, "AddPost");
        }

        /// <summary>
        /// 判断用户是否具有查看权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="addUserName">原作者。</param>
        /// <param name="isPublished">是否发布。</param>
        /// <returns>是否具有查看权限。</returns>
        public static bool HasViewPermission(UserInfo currentUser, int siteId, string addUserName, bool isPublished)
        {
            if (isPublished)
            {
                return true;
            }

            if (PermissionManager.HasPermission(currentUser, siteId, "EditAllDraftAPost"))
            {
                return true;
            }
            else
            {
                return currentUser.UserName == addUserName;
            }
        }

        /// <summary>
        /// 判断用户是否具有删除权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="uri">文章URI。</param>
        /// <returns>是否具有删除权限。</returns>
        public static bool HasDeletePermission(UserInfo currentUser, long uri)
        {
            PostInfo post = null;
            try
            {
                post = provider.GetPost(uri);
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
            if (post != null)
            {
                return HasDeletePermission(currentUser, post.SiteId, post.AddUserName, post.IsPublished);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断用户是否具有删除权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="addUserName">原作者。</param>
        /// <param name="isPublished">是否发布。</param>
        /// <returns>是否具有权限。</returns>
        public static bool HasDeletePermission(UserInfo currentUser, int siteId, string addUserName, bool isPublished)
        {
            if (PermissionManager.HasPermission(currentUser, siteId, "DeleteAllDraftAPost"))
            {
                return true;
            }
            else
            {
                if (addUserName == currentUser.UserName)
                {
                    if (PermissionManager.HasPermission(currentUser, siteId, "DeleteOwnDraftAPost"))
                    {
                        return true;
                    }
                    else
                    {
                        return (isPublished == false && PermissionManager.HasPermission(currentUser, siteId, "DeleteOwnDraftOnly"));
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断用户是否具有编辑权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="uri">文章URI。</param>
        /// <returns>是否具有编辑权限。</returns>
        public static bool HasEditPermission(UserInfo currentUser, long uri)
        {
            PostInfo post = null;
            try
            {
                post = provider.GetPost(uri);
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

            if (post != null)
            {
                return HasEditPermission(currentUser, post.SiteId, post.AddUserName, post.IsPublished);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断用户是否具有编辑文章权限。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="addUserName">原作者。</param>
        /// <param name="isPublished">是否发布。</param>
        /// <returns>是否具有权限。</returns>
        public static bool HasEditPermission(UserInfo currentUser, int siteId, string addUserName, bool isPublished)
        {
            if (currentUser == null)
            {
                return false;
            }

            if (PermissionManager.HasPermission(currentUser, siteId, "EditAllDraftAPost"))
            {
                return true;
            }
            else
            {
                if (addUserName == currentUser.UserName)
                {
                    if (PermissionManager.HasPermission(currentUser, siteId, "EditOwnDraftAPost"))
                    {
                        return true;
                    }
                    else
                    {
                        return (isPublished == false &&
                                         PermissionManager.HasPermission(currentUser, siteId, "EditOwnDraftOnly"));
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

