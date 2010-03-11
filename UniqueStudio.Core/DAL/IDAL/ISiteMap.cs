using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 网站地图管理提供类需实现的方法
    /// </summary>
    internal interface ISiteMap
    {
        /// <summary>
        /// 创建页面
        /// </summary>
        /// <param name="page">页面信息</param>
        /// <returns>如果创建成功，返回页面信息，否则返回空</returns>
        PageInfo CreatePage(PageInfo page);

        /// <summary>
        /// 删除指定页面
        /// </summary>
        /// <param name="pageId">待删除页面ID</param>
        /// <returns>是否删除成功</returns>
        bool DeletePage(int pageId);

        /// <summary>
        /// 返回所有页面
        /// </summary>
        /// <returns>页面的集合</returns>
        PageCollection GetAllPages();

        /// <summary>
        /// 更新页面信息
        /// </summary>
        /// <param name="page">页面信息</param>
        /// <returns>是否更新成功</returns>
        bool UpdatePage(PageInfo page);
    }
}
