using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;
using System.Web;
using UniqueStudio.ComContent.BLL;

namespace UniqueStudio.ComContent.ApiLayer
{
    #region structs
    public struct BlogInfo
    {
        public string blogid;
        public string url;
        public string blogName;
    }
    public struct Category
    {
        public string categoryId;
        public string categoryName;
    }
    [Serializable]
    public struct CategoryInfo
    {
        public string description;
        public string htmlUrl;
        public string rssUrl;
        public string title;
        public string categoryid;
    }
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Enclosure
    {
        public int length;
        public string type;
        public string url;
    }
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public DateTime dateCreated;
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string description;
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string title;
        public string[] categories;
        public Enclosure enclosure;
        public string link;
        public string permalink;
        [XmlRpcMember(
          Description = "Not required when posting. Depending on server may "
          + "be either string or integer. "
          + "Use Convert.ToInt32(postid) to treat as integer or "
          + "Convert.ToString(postid) to treat as string")]
        public object postid;
        public Source source;
        public string userid;
    }
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Source
    {
        public string name;
        public string url;
    }
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct MediaObject
    {
        public string name;
        public string type;
        public byte[] bits;
    }
    [Serializable]
    public struct MediaObjectInfo
    {
        public string url;
    }
    #endregion
    interface IMetaWeblog
    {
        #region MetaWeblogAPI Members
        [XmlRpcMethod("metaWeblog.newPost")]
        string AddPost(string blogid, string username, string password, Post post, bool publish);
        [XmlRpcMethod("metaWeblog.editPost")]
        bool UpdatePost(string postid, string username, string password, Post post, bool publish);
        [XmlRpcMethod("metaWeblog.getPost")]
        Post GetPost(string postid, string username, string password);
        [XmlRpcMethod("metaWeblog.getCategories")]
        CategoryInfo[] GetCategories(string blogid, string username, string password);
        [XmlRpcMethod("metaWeblog.getRecentPosts")]
        Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);
        [XmlRpcMethod("metaWeblog.newMediaObject")]
        MediaObjectInfo NewMediaObject(string blogid, string username, string password,
          MediaObject mediaObject);
        #endregion
        #region BloggerAPI Members
        [XmlRpcMethod("blogger.deletePost")]
        [return: XmlRpcReturnValue(Description = "Returns true.")]
        bool DeletePost(string key, string postid, string username, string password, [XmlRpcParameter(Description = "Where applicable, this specifies whether the blog "
                + "should be republished after the post has been deleted.")] bool publish);
        [XmlRpcMethod("blogger.getUsersBlogs", Description = "Returns information on all the blogs a given user "
           + "is a member.")]
        BlogInfo[] GetUsersBlogs(string key, string username, string password);
        #endregion
    }
}
