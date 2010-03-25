//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示页面访问的实体类。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示页面访问的实体类。
    /// </summary>
    public class PageVisitInfo
    {
        private int id;
        private int siteId;
        private string rawUrl;
        private string userHostAddress;
        private string userHostName;
        private string userAgent;
        private string urlReferrer=string.Empty;
        private DateTime time;

        /// <summary>
        /// 初始化<see cref="PageVisitInfo"/>类的实例。
        /// </summary>
        public PageVisitInfo()
        {
            //默认
        }

        /// <summary>
        /// 初始化<see cref="PageVisitInfo"/>类的实例。
        /// </summary>
        /// <param name="rawUrl">原始URL。</param>
        /// <param name="userHostAddress">用户主机IP地址。</param>
        /// <param name="userHostName">用户主机名。</param>
        /// <param name="userAgent">用户代理。</param>
        /// <param name="urlReferrer">前一页面地址。</param>
        public PageVisitInfo(string rawUrl, string userHostAddress, string userHostName, string userAgent, string urlReferrer)
        {
            this.rawUrl = rawUrl;
            this.userHostAddress = userHostAddress;
            this.userHostName = userHostName;
            this.userAgent = userAgent;
            this.urlReferrer = urlReferrer;
        }

        /// <summary>
        /// 初始化<see cref="PageVisitInfo"/>类的实例。
        /// </summary>
        /// <param name="rawUrl">原始URL。</param>
        /// <param name="userHostAddress">用户主机IP地址。</param>
        /// <param name="userHostName">用户主机名。</param>
        /// <param name="userAgent">用户代理。</param>
        /// <param name="urlReferrer">前一页面地址。</param>
        /// <param name="time">访问时间。</param>
        public PageVisitInfo(string rawUrl, string userHostAddress, string userHostName, string userAgent,string urlReferrer,DateTime time)
            :this(rawUrl,userHostAddress,userHostName,userAgent,urlReferrer)
        {
            this.time = time;
        }

        /// <summary>
        /// 初始化<see cref="PageVisitInfo"/>类的实例。
        /// </summary>
        /// <param name="id">页面访问ID。</param>
        /// <param name="rawUrl">原始URL。</param>
        /// <param name="userHostAddress">用户主机IP地址。</param>
        /// <param name="userHostName">用户主机名。</param>
        /// <param name="userAgent">用户代理。</param>
        /// <param name="urlReferrer">前一页面地址。</param>
        /// <param name="time">访问时间。</param>
        public PageVisitInfo(int id,string rawUrl, string userHostAddress, string userHostName, string userAgent,string urlReferrer, DateTime time)
            :this(rawUrl,userHostAddress,userHostName,userAgent,urlReferrer)
        {
            this.id = id;
            this.time = time;
        }

        /// <summary>
        /// 页面访问ID。
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
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
        /// 原始URL。
        /// </summary>
        public string RawUrl
        {
            get { return rawUrl; }
            set { rawUrl = value; }
        }
        /// <summary>
        /// 用户主机IP地址。
        /// </summary>
        public string UserHostAddress
        {
            get { return userHostAddress; }
            set { userHostAddress = value; }
        }
        /// <summary>
        /// 用户主机名。
        /// </summary>
        public string UserHostName
        {
            get { return userHostName; }
            set { userHostName = value; }
        }
        /// <summary>
        /// 用户代理。
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }
        /// <summary>
        /// 前一页面地址。
        /// </summary>
        public string UrlReferrer
        {
            get { return urlReferrer; }
            set { urlReferrer = value; }
        }
        /// <summary>
        /// 访问时间。
        /// </summary>
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
