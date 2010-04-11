//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示组件的实体类。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示组件的实体类。
    /// </summary>
    [Serializable]
    public class CompenentInfo
    {
        private int compenentId;
        private int siteId;
        private string siteName;
        private string compenentName;
        private string displayName;
        private string compenentAuthor;
        private string description;
        private string classPath;
        private string assembly;
        private string workingPath;
        private string config;
        private PermissionCollection permissions = null;

        /// <summary>
        /// 初始化<see cref="CompenentInfo"/>类的实例。
        /// </summary>
        public CompenentInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 组件ID。
        /// </summary>
        public int CompenentId
        {
            get { return compenentId; }
            set { compenentId = value; }
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
        /// 网站名称。
        /// </summary>
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }
        /// <summary>
        /// 组件名称。
        /// </summary>
        public string CompenentName
        {
            get { return compenentName; }
            set { compenentName = value; }
        }
        /// <summary>
        /// 组件显示名。
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        /// <summary>
        /// 组件作者。
        /// </summary>
        public string CompenentAuthor
        {
            get { return compenentAuthor; }
            set { compenentAuthor = value; }
        }
        /// <summary>
        /// 组件描述。
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// 类名。
        /// </summary>
        /// <remarks>API层类名。</remarks>
        public string ClassPath
        {
            get { return classPath; }
            set { classPath = value; }
        }
        /// <summary>
        /// 程序集名称。
        /// </summary>
        public string Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        }
        /// <summary>
        /// 组件工作路径。
        /// </summary>
        public string WorkingPath
        {
            get { return workingPath; }
            set { workingPath = value; }
        }
        /// <summary>
        /// 配置信息。
        /// </summary>
        public string Config
        {
            get { return config; }
            set { config = value; }
        }
        /// <summary>
        /// 权限列表。
        /// </summary>
        public PermissionCollection Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }
    }
}
