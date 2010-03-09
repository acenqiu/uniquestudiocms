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
        private static bool isDeleteChildCategories = false;
        private static bool isDisplayTime = true;
        private static bool enableLoginByUserName = true;
        private static bool enableRegister = false;
        private static bool isApprovedAfterRegister = false;
        private static bool postStatByYear = false;

        //string
        private static string baseAddress = string.Empty;
        private static string webName = string.Empty;
        private static string timeFormatOfIndexPostList = "yyyy-MM-dd";
        private static string timeFormatOfSectionPostList = "yyyy-MM-dd";
        private static string enclosureExtension = ".doc;.docx;.rar;.zip;.7z";

        //int
        private static int pageSizeOfIndexPostList = 9;
        private static int pageSizeOfSectionPostList = 20;
        private static int newImageThreshold = 7;

        //Boolean
        /// <summary>
        /// 是否删除分类的同时删除其子分类
        /// </summary>
        public static bool IsDeleteChildCategories
        {
            get { return isDeleteChildCategories; }
            set { isDeleteChildCategories = value; }
        }
        /// <summary>
        /// 是否在后台显示时间
        /// </summary>
        /// <remarks>可能在后续版本中移除</remarks>
        public static bool IsDisplayTime
        {
            get { return isDisplayTime; }
            set { isDisplayTime = value; }
        }
        /// <summary>
        /// 是否启用用户名方式登录
        /// </summary>
        public static bool EnableLoginByUserName
        {
            get { return enableLoginByUserName; }
            set { enableLoginByUserName = value; }
        }
        /// <summary>
        /// 是否开放注册
        /// </summary>
        public static bool EnableRegister
        {
            get { return enableRegister; }
            set { enableRegister = value; }
        }
        /// <summary>
        /// 外部注册后是否处于激活状态
        /// </summary>
        public static bool IsApprovedAfterRegister
        {
            get { return isApprovedAfterRegister; }
            set { isApprovedAfterRegister = value; }
        }
        /// <summary>
        /// 是否根据年统计文章数量
        /// </summary>
        /// <remarks>可能在后续版本中修改</remarks>
        public static bool PostStatByYear
        {
            get { return postStatByYear; }
            set { postStatByYear = value; }
        }

        //string
        /// <summary>
        /// 网站首页地址
        /// </summary>
        public static string BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }
        /// <summary>
        /// 网站名称
        /// </summary>
        public static string WebName
        {
            get { return webName; }
            set { webName = value; }
        }
        /// <summary>
        /// 首页文章列表时间显示格式
        /// </summary>
        /// <remarks>可能在后续版本中移除</remarks>
        public static string TimeFormatOfIndexPostList
        {
            get { return timeFormatOfIndexPostList; }
            set { timeFormatOfIndexPostList = value; }
        }
        /// <summary>
        /// 子页面文章列表时间显示格式
        /// </summary>
        /// <remarks>可能在后续版本中移除</remarks>
        public static string TimeFormatOfSectionPostList
        {
            get { return timeFormatOfSectionPostList; }
            set { timeFormatOfSectionPostList = value; }
        }
        /// <summary>
        /// 允许上传的附件扩展名
        /// </summary>
        /// <remarks>将在后续版本中转移</remarks>
        public static string EnclosureExtension
        {
            get { return enclosureExtension; }
            set { enclosureExtension = value; }
        }

        //int
        /// <summary>
        /// 首页文章列表显示数量
        /// </summary>
        public static int PageSizeOfIndexPostList
        {
            get { return pageSizeOfIndexPostList; }
            set { pageSizeOfIndexPostList = value; }
        }
        /// <summary>
        /// 子页面文章列表显示数量
        /// </summary>
        public static int PageSizeOfSectionPostList
        {
            get { return pageSizeOfSectionPostList; }
            set { pageSizeOfSectionPostList = value; }
        }
        /// <summary>
        /// 定义为新文章的天数
        /// </summary>
        public static int NewImageThreshold
        {
            get { return newImageThreshold; }
            set { newImageThreshold = value; }
        }
    }
}
