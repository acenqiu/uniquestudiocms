//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：菜单项的集合。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 菜单项的集合。
    /// </summary>
    [Serializable]
    public class MenuItemCollection:List<MenuItemInfo>
    {
        /// <summary>
        /// 初始化<see cref="MenuItemCollection"/>类的实例。
        /// </summary>
        public MenuItemCollection()
            : base()
        {
            
        }

        /// <summary>
        /// 以集合容量初始化<see cref="MenuItemCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public MenuItemCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
