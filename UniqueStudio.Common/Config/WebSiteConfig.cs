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
    public class WebSiteConfig : SystemConfig
    {
        /// <summary>
        /// 初始化<see cref="WebSiteConfig"/>类的实例。
        /// </summary>
        public WebSiteConfig()
        {
        }

        //Boolean
        private bool isDeleteChildCategories = false;
        private bool isDisplayTime = true;

        //string
        private string webName = string.Empty;

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
    }
}
