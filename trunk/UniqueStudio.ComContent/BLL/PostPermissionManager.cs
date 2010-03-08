using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Core.Permission;
using UniqueStudio.Common.Model;
using UniqueStudio.ComContent.Model;

namespace UniqueStudio.ComContent.BLL
{
    public class PostPermissionManager
    {
        PostManager pm = new PostManager();
        PostInfo post = new PostInfo();
        /// <summary>
        /// 判断用户是否具有添加文章权限
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否具有添加文章权限</returns>
        public bool HasAddPermission(UserInfo user)
        {
            if (user == null)
            {
                return false;
            }
            return PermissionManager.HasPermission(user, "AddPost");
        }
        /// <summary>
        /// 判断用户是否具有查看权限
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="addUser">原作者</param>
        /// <param name="publish">是否发布</param>
        /// <returns>是否具有查看权限</returns>
        public bool HasViewPermission(UserInfo user, string addUser, bool publish)
        {
            if (user == null)
            {
                if (publish)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (PermissionManager.HasPermission(user, "EditAllDraftAPost"))
                {
                    return true;
                }
                else
                {
                    if (user.UserName == addUser)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// 判断用户是否具有删除权限
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="uri">文章URI</param>
        /// <returns>是否具有删除权限</returns>
        public bool HasDeletePermission(UserInfo user, long uri)
        {
            post = pm.GetPost(uri);
            return HasDeletePermission(user, post.AddUserName, post.IsPublished);
        }
        /// <summary>
        /// 判断用户是否具有删除权限
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="addUser">原作者</param>
        /// <param name="publish">是否发布</param>
        /// <returns>是否具有权限</returns>
        public bool HasDeletePermission(UserInfo user, string addUser, bool publish)
        {
            if (user == null)
            {
                return false;
            }
            if (PermissionManager.HasPermission(user, "DeleteAllDraftAPost"))
            {
                return true;
            }
            else
            {
                if (addUser == user.UserName)
                {
                    if (PermissionManager.HasPermission(user, "DeleteOwnDraftAPost"))
                    {
                        return true;
                    }
                    else
                    {
                        if (publish == false && PermissionManager.HasPermission(user, "DeleteOwnDraftOnly"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 判断用户是否具有编辑权限
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="uri">文章URI</param>
        /// <returns>是否具有编辑权限</returns>
        public bool HasEditPermission(UserInfo user, long uri)
        {
            post = pm.GetPost(uri);
            return HasEditPermission(user, post.AddUserName, post.IsPublished);
        }
        /// <summary>
        /// 判断用户是否具有编辑文章权限
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="addUser">原作者</param>
        /// <param name="publish">是否发布</param>
        /// <returns>是否具有权限</returns>
        public bool HasEditPermission(UserInfo user, string addUser, bool publish)
        {
            if (user == null)
            {
                return false;
            }
            if (PermissionManager.HasPermission(user, "EditAllDraftAPost"))
            {
                return true;
            }
            else
            {
                if (addUser == user.UserName)
                {
                    if (PermissionManager.HasPermission(user, "EditOwnDraftAPost"))
                    {
                        return true;
                    }
                    else
                    {
                        if (publish == false && PermissionManager.HasPermission(user, "EditOwnDraftOnly"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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

