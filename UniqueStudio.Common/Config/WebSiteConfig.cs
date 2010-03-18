using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Reflection;

using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 系统设置类
    /// </summary>
    /// <remarks>可能在后续版本中移除</remarks>
    public class WebSiteConfig : SystemConfig
    {
        /// <summary>
        /// 初始化<see cref="WebSiteConfig"/>类的实例。
        /// </summary>
        public WebSiteConfig()
        {
            path = @"admin\xml\WebSiteConfig.xml";
        }

        //Boolean
        private bool isDeleteChildCategories = false;
        private bool isDisplayTime = true;
        private bool postStatByYear = false;

        //string
        private string baseAddress = string.Empty;
        private string webName = string.Empty;
        private string timeFormatOfIndexPostList = "yyyy-MM-dd";
        private string timeFormatOfSectionPostList = "yyyy-MM-dd";
        private string enclosureExtension = ".doc;.docx;.rar;.zip;.7z";

        //int
        private int pageSizeOfIndexPostList = 9;
        private int pageSizeOfSectionPostList = 20;
        private int newImageThreshold = 7;

        //Boolean
        /// <summary>
        /// 是否删除分类的同时删除其子分类
        /// </summary>
        public bool IsDeleteChildCategories
        {
            get { return isDeleteChildCategories; }
            set { isDeleteChildCategories = value; }
        }
        /// <summary>
        /// 是否在后台显示时间
        /// </summary>
        /// <remarks>可能在后续版本中移除</remarks>
        public bool IsDisplayTime
        {
            get { return isDisplayTime; }
            set { isDisplayTime = value; }
        }
        /// <summary>
        /// 是否根据年统计文章数量
        /// </summary>
        /// <remarks>可能在后续版本中修改</remarks>
        public bool PostStatByYear
        {
            get { return postStatByYear; }
            set { postStatByYear = value; }
        }

        //string
        /// <summary>
        /// 网站首页地址
        /// </summary>
        public string BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName
        {
            get { return webName; }
            set { webName = value; }
        }
        /// <summary>
        /// 首页文章列表时间显示格式
        /// </summary>
        /// <remarks>可能在后续版本中移除</remarks>
        public string TimeFormatOfIndexPostList
        {
            get { return timeFormatOfIndexPostList; }
            set { timeFormatOfIndexPostList = value; }
        }
        /// <summary>
        /// 子页面文章列表时间显示格式
        /// </summary>
        /// <remarks>可能在后续版本中移除</remarks>
        public string TimeFormatOfSectionPostList
        {
            get { return timeFormatOfSectionPostList; }
            set { timeFormatOfSectionPostList = value; }
        }
        /// <summary>
        /// 允许上传的附件扩展名
        /// </summary>
        /// <remarks>将在后续版本中转移</remarks>
        public string EnclosureExtension
        {
            get { return enclosureExtension; }
            set { enclosureExtension = value; }
        }

        //int
        /// <summary>
        /// 首页文章列表显示数量
        /// </summary>
        public int PageSizeOfIndexPostList
        {
            get { return pageSizeOfIndexPostList; }
            set { pageSizeOfIndexPostList = value; }
        }
        /// <summary>
        /// 子页面文章列表显示数量
        /// </summary>
        public int PageSizeOfSectionPostList
        {
            get { return pageSizeOfSectionPostList; }
            set { pageSizeOfSectionPostList = value; }
        }
        /// <summary>
        /// 定义为新文章的天数
        /// </summary>
        public int NewImageThreshold
        {
            get { return newImageThreshold; }
            set { newImageThreshold = value; }
        }
    }
}
