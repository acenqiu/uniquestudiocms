namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个网站的实体类。
    /// </summary>
    public class SiteInfo
    {
        private int siteId;
        private string siteName;
        private string relativePath;
        private string config;

        /// <summary>
        /// 初始化<see cref="SiteInfo"/>类的实例。
        /// </summary>
        public SiteInfo()
        {
            //默认构造函数
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
        /// 网站相对路径。
        /// </summary>
        public string RelativePath
        {
            get { return relativePath; }
            set { relativePath = value; }
        }
        /// <summary>
        /// 网站配置。
        /// </summary>
        public string Config
        {
            get { return config; }
            set { config = value; }
        }
    }
}
