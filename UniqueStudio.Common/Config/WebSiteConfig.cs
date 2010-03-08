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
    public class WebSiteConfig : SystemConfig
    {
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
        private static string baseAddress = "";
        private static string webName = "基本物理量测量国家重点实验室";
        private static string timeFormatOfIndexPostList = "yyyy-MM-dd";
        private static string timeFormatOfSectionPostList = "yyyy-MM-dd";
        private static string enclosureExtension = ".doc;.docx;.rar;.zip;.7z";

        //int
        private static int pageSizeOfIndexPostList = 9;
        private static int pageSizeOfSectionPostList = 20;
        private static int newImageThreshold = 7;

        //Boolean
        public static bool IsDeleteChildCategories
        {
            get { return isDeleteChildCategories; }
            set { isDeleteChildCategories = value; }
        }
        public static bool IsDisplayTime
        {
            get { return isDisplayTime; }
            set { isDisplayTime = value; }
        }
        public static bool EnableLoginByUserName
        {
            get { return enableLoginByUserName; }
            set { enableLoginByUserName = value; }
        }
        public static bool EnableRegister
        {
            get { return enableRegister; }
            set { enableRegister = value; }
        }
        public static bool IsApprovedAfterRegister
        {
            get { return isApprovedAfterRegister; }
            set { isApprovedAfterRegister = value; }
        }
        public static bool PostStatByYear
        {
            get { return postStatByYear; }
            set { postStatByYear = value; }
        }

        //string
        public static string BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }
        public static string WebName
        {
            get { return webName; }
            set { webName = value; }
        }
        public static string TimeFormatOfIndexPostList
        {
            get { return timeFormatOfIndexPostList; }
            set { timeFormatOfIndexPostList = value; }
        }
        public static string TimeFormatOfSectionPostList
        {
            get { return timeFormatOfSectionPostList; }
            set { timeFormatOfSectionPostList = value; }
        }
        public static string EnclosureExtension
        {
            get { return enclosureExtension; }
            set { enclosureExtension = value; }
        }

        //int
        public static int PageSizeOfIndexPostList
        {
            get { return pageSizeOfIndexPostList; }
            set { pageSizeOfIndexPostList = value; }
        }
        public static int PageSizeOfSectionPostList
        {
            get { return pageSizeOfSectionPostList; }
            set { pageSizeOfSectionPostList = value; }
        }
        public static int NewImageThreshold
        {
            get { return newImageThreshold; }
            set { newImageThreshold = value; }
        }
    }
}
