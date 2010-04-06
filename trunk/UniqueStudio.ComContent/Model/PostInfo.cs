using System;
using System.Collections.Generic;
using System.Text;
using UniqueStudio.Common.Model;
namespace UniqueStudio.ComContent.Model
{
    /// <summary>
    /// 表示一个帖子。
    /// </summary>
    [Serializable]
    public class PostInfo
    {
        private long uri = 0;
        private int siteId;
        private string addUserName = string.Empty;
        private string lastEditUserName = string.Empty;
        private DateTime createDate;
        private DateTime lastEditDate;
        private int type;
        private string title = string.Empty;
        private string subTitle = string.Empty;
        private string author = string.Empty;
        private string summary = string.Empty;
        private string content = string.Empty;
        private int comments;
        private bool isRecommend = false;
        private bool isHot = false;
        private bool isTop = false;
        private bool isAllowComment = true;
        private bool isPublished = false;
        private int count;
        private string settings;
        private int postDisplay;
        private string newsImage;
        private CategoryCollection categories;

        /// <summary>
        /// 初始化<see cref="PostInfo"/>类的新实例。
        /// </summary>
        public PostInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 唯一标示uri
        /// </summary>
        public long Uri
        {
            get { return uri; }
            set { uri = value; }
        }
        /// <summary>
        /// 网站ID。
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// 文章发布作者
        /// </summary>
        public string AddUserName
        {
            get { return addUserName; }
            set { addUserName = value; }
        }
        /// <summary>
        /// 最近编辑文章作者
        /// </summary>
        public string LastEditUserName
        {
            get { return lastEditUserName; }
            set { lastEditUserName = value; }
        }
        /// <summary>
        /// 文章创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        /// <summary>
        /// 文章最近更新日期
        /// </summary>
        public DateTime LastEditDate
        {
            get { return lastEditDate; }
            set { lastEditDate = value; }
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// 文章副标题
        /// </summary>
        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }
        /// <summary>
        /// 文章作者
        /// </summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        /// <summary>
        /// 文章概述
        /// </summary>
        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// 评论
        /// </summary>
        public int Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend
        {
            get { return isRecommend; }
            set { isRecommend = value; }
        }
        /// <summary>
        /// 是否热门
        /// </summary>
        public bool IsHot
        {
            get { return isHot; }
            set { isHot = value; }
        }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop
        {
            get { return isTop; }
            set { isTop = value; }
        }
        /// <summary>
        /// 是否允许评论
        /// </summary>
        public bool IsAllowComment
        {
            get { return isAllowComment; }
            set { isAllowComment = value; }
        }
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublished
        {
            get { return isPublished; }
            set { isPublished = value; }
        }
        /// <summary>
        /// 访问量
        /// </summary>
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        /// <summary>
        /// 设置信息
        /// </summary>
        public string Settings
        {
            get { return settings; }
            set { settings = value; }
        }
        /// <summary>
        /// 文章分类信息
        /// </summary>
        public CategoryCollection Categories
        {
            get { return categories; }
            set { categories = value; }
        }
        /// <summary>
        /// 控制显示文章标题及其它信息(值为0显示所有信息，1不显示文章标题，2不显示其它信息，3只显示文章内容)
        /// </summary>
        public int PostDisplay
        {
            get { return postDisplay; }
            set { postDisplay = value; }
        }
        /// <summary>
        /// 新闻图片
        /// </summary>
        public string NewsImage
        {
            get { return newsImage; }
            set { newsImage = value; }
        }
    }
}
