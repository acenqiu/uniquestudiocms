//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要： 网站的集合。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System.Collections.Generic;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 网站的集合。
    /// </summary>
    public class SiteCollection : List<SiteInfo>
    {
        /// <summary>
        /// 初始化<see cref="SiteCollection"/>类的实例。
        /// </summary>
        public SiteCollection()
            : base()
        {

        }

        /// <summary>
        /// 以集合大小初始化<see cref="SiteCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity"></param>
        public SiteCollection(int capacity)
            : base(capacity)
        {

        }
    }
}
