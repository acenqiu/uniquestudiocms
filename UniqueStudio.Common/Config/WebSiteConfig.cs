//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：网站设置类。
// 完成日期：2010年04月10日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 网站设置类。
    /// </summary>
    public class WebSiteConfig : SystemConfig
    {
        /// <summary>
        /// 初始化<see cref="WebSiteConfig"/>类的实例。
        /// </summary>
        public WebSiteConfig()
        {
            //默认构造函数
        }

        //Boolean
        private bool isDeleteChildCategories = false;

        //string
        private string webName = string.Empty;

        //Boolean
        /// <summary>
        /// 是否删除分类的同时删除其子分类。
        /// </summary>
        public bool IsDeleteChildCategories
        {
            get { return isDeleteChildCategories; }
            set { isDeleteChildCategories = value; }
        }
        
        //string
        /// <summary>
        /// 网站名称。
        /// </summary>
        public string WebName
        {
            get { return webName; }
            set { webName = value; }
        }
    }
}
