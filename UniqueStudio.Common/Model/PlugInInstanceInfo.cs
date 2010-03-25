namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个插件实例的实体类。
    /// </summary>
    public class PlugInInstanceInfo
    {
        private int instanceId;
        private int plugInId;
        private int siteId;
        private string siteName;
        private bool isEnabled;
        private string config;

        /// <summary>
        /// 初始化<see cref="PlugInInstanceInfo"/>类的实例。
        /// </summary>
        public PlugInInstanceInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 插件实例ID。
        /// </summary>
        public int InstanceId
        {
            get { return instanceId; }
            set { instanceId = value; }
        }
        /// <summary>
        /// 插件ID。
        /// </summary>
        public int PlugInId
        {
            get { return plugInId; }
            set { plugInId = value; }
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
        /// 是否启用。
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
        /// <summary>
        /// 配置信息。
        /// </summary>
        public string Config
        {
            get { return config; }
            set { config = value; }
        }
    }
}
