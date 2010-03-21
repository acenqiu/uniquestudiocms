using System;
using CookComputing.XmlRpc;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.User;

namespace UniqueStudio.ComContent.ApiLayer
{
    public class MetaWeblog : XmlRpcService, IMetaWeblog
    {
        private PostManager postManager;
        private PostInfo postInfo;
        private UserInfo userInfo;
        private CategoryManager cm;

        public MetaWeblog()
        {
        }

        private UserInfo ValidateUser(string username, string password)
        {
            UserManager um = new UserManager();
            UserInfo ui = um.UserAuthorization(username, password);
            if (ui != null)
            {
                return ui;
            }
            else
            {
                throw new XmlRpcFaultException(0, "User does not exist.");
            }
        }
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="blogid">博客ID</param>
        /// <param name="username">用户邮箱</param>
        /// <param name="password">用户密码</param>
        /// <param name="post">博客信息</param>
        /// <param name="publish">是否发布</param>
        /// <returns>博客id</returns>
        public string AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            userInfo = ValidateUser(username, password);
            if (userInfo != null)
            {
                postInfo = new PostInfo();
                postInfo.AddUserName = username;
                postInfo.CreateDate = post.dateCreated;
                postInfo.Author = username;
                CategoryCollection cates = new CategoryCollection();
                for (int i = 0; i < post.categories.Length; i++)
                {
                    //cates.Add(cm.GetCategory(post.categories[i]));
                }
                postInfo.Categories = cates;
                postInfo.Content = post.description;
                postInfo.IsAllowComment = false;
                postInfo.IsHot = false;
                postInfo.IsPublished = publish;
                postInfo.IsRecommend = false;
                postInfo.IsTop = false;
                postInfo.LastEditDate = post.dateCreated;
                postInfo.LastEditUserName = username;
                postInfo.SubTitle = string.Empty;
                postInfo.Summary = post.description;
                postInfo.Title = post.title;
                //TODO:some fields of postInfo
                postManager = new PostManager();
                try
                {
                    return postManager.AddPost(userInfo, postInfo).ToString();
                }
                catch
                {
                    throw new XmlRpcFaultException(1, "Add new blog failed");
                }
            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }
        /// <summary>
        /// 更新博客
        /// </summary>
        /// <param name="postid">博客id</param>
        /// <param name="username">用户邮箱</param>
        /// <param name="password">用户密码</param>
        /// <param name="post">博客信息</param>
        /// <param name="publish">是否发布</param>
        /// <returns>是否成功更新</returns>
        public bool UpdatePost(string postid, string username, string password, Post post, bool publish)
        {
            userInfo = ValidateUser(username, password);
            if (userInfo != null)
            {
                postInfo = new PostInfo();
                CategoryCollection cates = new CategoryCollection();
                for (int i = 0; i < post.categories.Length; i++)
                {
                    //cates.Add(cm.GetCategory(post.categories[i]));
                }
                postInfo.Categories = cates;
                postInfo.Content = post.description;
                postInfo.IsAllowComment = false;
                postInfo.IsHot = false;
                postInfo.IsPublished = publish;
                postInfo.IsRecommend = false;
                postInfo.IsTop = false;
                postInfo.LastEditDate = post.dateCreated;
                postInfo.LastEditUserName = username;
                postInfo.SubTitle = string.Empty;
                postInfo.Summary = post.description;
                postInfo.Title = post.title;
                //TODO:some fields of postInfo
                postManager = new PostManager();
                try
                {
                    return postManager.EditPost(userInfo, postInfo);
                }
                catch
                {
                    throw new XmlRpcFaultException(1, "Update post failed");
                }
            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }
        /// <summary>
        /// 获得文章
        /// </summary>
        /// <param name="postid">文章ID</param>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public Post GetPost(string postid, string username, string password)
        {
            userInfo = ValidateUser(username, password);
            if (userInfo != null)
            {
                postManager = new PostManager();
                postInfo = new PostInfo();
                try
                {
                    postInfo = postManager.GetPost(userInfo, Convert.ToInt64(postid));
                }
                catch
                {
                    throw new XmlRpcFaultException(1, "Get Post Failed");
                }
                Post getPost = new Post();
                //getPost.categories=postInfo.Categories;
                string[] cates = new string[postInfo.Categories.Count];
                for (int i = 0; i < postInfo.Categories.Count; i++)
                {
                    cates[i] = postInfo.Categories[i].CategoryName;
                }
                getPost.categories = cates;
                getPost.dateCreated = postInfo.CreateDate;
                getPost.description = postInfo.Summary;
                //getPost.link
                // getPost.permalink
                getPost.postid = postid;
                //getPost.source
                getPost.title = postInfo.Title;
                getPost.userid = postInfo.AddUserName;
                getPost.postid = postInfo.Uri.ToString();
                return getPost;
            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }
        /// <summary>
        /// 获得博客分类
        /// </summary>
        /// <param name="blogid">博客ID</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>分类信息</returns>
        public CategoryInfo[] GetCategories(string blogid, string username, string password)
        {
            userInfo = ValidateUser(username, password);
            if (userInfo != null)
            {
                //TODO:categories
                throw new XmlRpcFaultException(1, "No categories");
            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }

        public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            userInfo = ValidateUser(username, password);
            if (userInfo != null)
            {
                Post[] posts = new Post[numberOfPosts];
                postManager = new PostManager();
                //TODO:offset and PostListType
                try
                {
                    int siteId = Converter.IntParse(blogid, 0);
                    PostCollection postCollection = postManager.GetRecentPosts(siteId, numberOfPosts, 0, true, PostListType.PublishedOnly);
                    for (int i = 0; i < numberOfPosts; i++)
                    {
                        string[] cates = new string[postCollection[i].Categories.Count];
                        for (int j = 0; j < postCollection[i].Categories.Count; j++)
                        {
                            cates[i] = postCollection[i].Categories[i].CategoryName;
                        }
                        posts[i].categories = cates;
                        posts[i].dateCreated = postCollection[i].CreateDate;
                        posts[i].description = postCollection[i].Summary;
                        //posts[i].enclosure
                        //posts[i].link
                        //posts[i].permalink
                        posts[i].postid = postCollection[i].Uri.ToString();
                        //posts[i].source
                        posts[i].title = postCollection[i].Title;
                        posts[i].userid = postCollection[i].AddUserName;
                    }
                    return posts;
                }
                catch (Exception)
                {
                    throw new XmlRpcFaultException(1, "Get RecentPosts Failed");
                }

            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }

        public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            if (ValidateUser(username, password) != null)
            {
                throw new XmlRpcFaultException(1, "No media object");
            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }

        public bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            userInfo = ValidateUser(username, password);
            if (userInfo != null)
            {
                postManager = new PostManager();
                //TODO:guid
                try
                {
                    return postManager.DeletePost(userInfo, Convert.ToInt64(postid));
                }
                catch
                {
                    throw new XmlRpcFaultException(1, "Delete Posts Failed");
                }
            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }

        public BlogInfo[] GetUsersBlogs(string key, string username, string password)
        {
            if (ValidateUser(username, password) != null)
            {
                throw new XmlRpcFaultException(1, "Can't get blogs");
            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }
    }
}
