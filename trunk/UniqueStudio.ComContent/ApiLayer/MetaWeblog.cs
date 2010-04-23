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
using UniqueStudio.Core.Site;

namespace UniqueStudio.ComContent.ApiLayer
{
    public class MetaWeblog : XmlRpcService, IMetaWeblog
    {
        public MetaWeblog()
        {
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
            UserInfo user = (new UserManager()).UserAuthorization(username, password);
            if (user != null)
            {
                PostInfo postInfo = new PostInfo();
                postInfo.SiteId = Converter.IntParse(blogid, 1);
                postInfo.AddUserName = username;
                postInfo.CreateDate = DateTime.Now;
                postInfo.Author = username;

                CategoryCollection cates = new CategoryCollection();
                for (int i = 0; i < post.categories.Length; i++)
                {
                    cates.Add(new Common.Model.CategoryInfo(Converter.IntParse(post.categories[i], 1)));
                }
                postInfo.Categories = cates;
                postInfo.Content = post.description;
                postInfo.IsAllowComment = false;
                postInfo.IsHot = false;
                postInfo.IsPublished = publish;
                postInfo.IsRecommend = false;
                postInfo.IsTop = false;
                postInfo.SubTitle = string.Empty;
                postInfo.Summary = post.description;
                postInfo.Title = post.title;
                postInfo.Settings = string.Empty;
                try
                {
                    long uri = (new PostManager()).AddPost(user, postInfo);
                    if (uri == 0)
                    {
                        throw new XmlRpcFaultException(3, "文章发布失败！");
                    }
                    else
                    {
                        return uri.ToString();
                    }
                }
                catch
                {
                    throw new XmlRpcFaultException(1, "Add new blog failed");
                }
            }
            else
            {
                throw new XmlRpcFaultException(0, "User does not exist");
            }
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
            UserInfo user = (new UserManager()).UserAuthorization(username, password);
            if (user != null)
            {
                PostInfo postInfo = new PostInfo();
                CategoryCollection cates = new CategoryCollection();
                for (int i = 0; i < post.categories.Length; i++)
                {
                    cates.Add(new Common.Model.CategoryInfo(Converter.IntParse(post.categories[i], 0)));
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
                
                try
                {
                    return (new PostManager()).EditPost(user, postInfo);
                }
                catch
                {
                    throw new XmlRpcFaultException(1, "Update post failed");
                }
            }
            else
            {
                throw new XmlRpcFaultException(0, "User does not exist");
            }
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
            UserInfo user = (new UserManager()).UserAuthorization(username, password);
            if (user != null)
            {
                PostManager postManager = new PostManager();
                PostInfo postInfo = new PostInfo();
                try
                {
                    postInfo = postManager.GetPost(user, Convert.ToInt64(postid));
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
            else
            {
                throw new XmlRpcFaultException(0, "User does not exist");
            }
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
            if ((new UserManager()).UserAuthorization(username, password) != null)
            {
                int siteId = Converter.IntParse(blogid, 0);
                CategoryCollection categories = (new CategoryManager()).GetAllCategories(siteId);
                CategoryInfo[] cates = new CategoryInfo[categories.Count];
                for (int i = 0; i < categories.Count; i++)
                {
                    CategoryInfo item = new CategoryInfo();
                    item.categoryid = categories[i].CategoryId.ToString();
                    item.title = categories[i].CategoryName;
                    item.htmlUrl = string.Empty;
                    item.rssUrl = string.Empty;
                    item.description = categories[i].CategoryName;
                    cates[i] = item;
                }
                return cates;
            }
            else
            {
                throw new XmlRpcFaultException(0, "User does not exist");
            }
        }

        public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            UserInfo user = (new UserManager()).UserAuthorization(username, password);
            if (user == null)
            {
                throw new XmlRpcFaultException(0, "User does not exist");
            }
            else
            {
                Post[] posts = new Post[numberOfPosts];

                PostManager manager = new PostManager();
                try
                {
                    int siteId = Converter.IntParse(blogid, 0);
                    if (siteId == 0)
                    {
                        throw new XmlRpcFaultException(2, "指定站点不存在");
                    }

                    PostCollection postCollection = manager.GetPostList(user, siteId, 1, numberOfPosts, DateTime.MinValue, DateTime.MinValue
                                                                                                            , DateTime.MinValue, DateTime.MinValue, 0, PostListType.Both, null);
                    for (int i = 0; i < postCollection.Count; i++)
                    {
                        string[] cates = new string[postCollection[i].Categories.Count];
                        for (int j = 0; j < postCollection[i].Categories.Count; j++)
                        {
                            cates[j] = postCollection[i].Categories[j].CategoryName;
                        }
                        posts[i].categories = cates;
                        posts[i].dateCreated = postCollection[i].CreateDate;
                        posts[i].description = postCollection[i].Summary;
                        //posts[i].enclosure
                        posts[i].link = PathHelper.PathCombine(SiteManager.BaseAddress(siteId), string.Format("view.aspx?uri={0}", postCollection[i].Uri));
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
        }

        public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            if ((new UserManager()).UserAuthorization(username,password) != null)
            {
                throw new XmlRpcFaultException(1, "No media object");
            }
            throw new XmlRpcFaultException(0, "User does not exist");
        }

        public bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            UserInfo user = (new UserManager()).UserAuthorization(username, password);
            if (user != null)
            {
                PostManager postManager = new PostManager();
                //TODO:guid
                try
                {
                    return postManager.DeletePost(user, Convert.ToInt64(postid));
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
            if ((new UserManager()).UserAuthorization(username, password) == null)
            {
                throw new XmlRpcFaultException(0, "指定用户不存在！");
            }
            else
            {
                BlogInfo[] blogs = new BlogInfo[1];
                blogs[0].blogid = "1";
                blogs[0].blogName = "中文站";
                blogs[0].url = "http://localhost:4761/metaweblogapi.ashx";
                return blogs;
                //SiteCollection sites = (new SiteManager()).GetAllSites();
                //    if (sites != null)
                //    {
                //        BlogInfo[] blogs = new BlogInfo[sites.Count];
                //        for (int i = 0; i < sites.Count; i++)
                //        {
                //            BlogInfo blog = new BlogInfo();
                //            blog.blogid = sites[i].SiteId.ToString();
                //            blog.blogName = sites[i].SiteName;
                //            blog.url = SiteManager.BaseAddress(sites[i].SiteId);
                //            blogs[i] = blog;
                //        }
                //        return blogs;
                //    }
                //    else
                //    {
                //        throw new XmlRpcFaultException(1, "无法获取站点信息。");
                //    }
                //}
            }
        }
    }
}
