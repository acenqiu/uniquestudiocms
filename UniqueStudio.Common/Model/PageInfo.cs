using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个页面的实体类。
    /// </summary>
    public class PageInfo
    {
        private int id;
        private int siteId;
        private string name;
        private PageType pageType;
        private string url=string.Empty;
        private string urlRegex;
        private string pagePath;
        private string parameters=string.Empty;
        private string staticPagePath=string.Empty;
        private ProcessType processType;
        private int subOf;
        private int depth;

        /// <summary>
        /// 初始化PageInfo类的实例
        /// </summary>
        public PageInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 页面ID
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 网站ID
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// 页面名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 页面类型
        /// </summary>
        public PageType PageType
        {
            get { return pageType; }
            set { pageType = value; }
        }

        /// <summary>
        /// 访问该页面的URL
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// 用于Url重写的正则表达式形式
        /// </summary>
        public string UrlRegex
        {
            get { return urlRegex; }
            set { urlRegex = value; }
        }

        /// <summary>
        /// 实际页面路径
        /// </summary>
        public string PagePath
        {
            get { return pagePath; }
            set { pagePath = value; }
        }

        /// <summary>
        /// 页面调用参数
        /// </summary>
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        /// <summary>
        /// 静态页面存放路径
        /// </summary>
        public string StaticPagePath
        {
            get { return staticPagePath; }
            set { staticPagePath = value; }
        }

        /// <summary>
        /// 页面处理方式
        /// </summary>
        public ProcessType ProcessType
        {
            get { return processType; }
            set { processType = value; }
        }

        /// <summary>
        /// 从属于
        /// </summary>
        public int SubOf
        {
            get { return subOf; }
            set { subOf = value; }
        }

        /// <summary>
        /// 页面层次
        /// </summary>
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }
    }
}
