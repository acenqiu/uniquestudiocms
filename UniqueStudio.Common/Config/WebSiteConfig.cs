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

        //string
        private string webName = string.Empty;
        private string timeFormatOfSectionPostList = "yyyy-MM-dd";

        //int
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

        //string
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName
        {
            get { return webName; }
            set { webName = value; }
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

        //int
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
