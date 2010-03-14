using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 菜单项的集合
    /// </summary>
    [Serializable]
    public class MenuItemCollection:List<MenuItemInfo>
    {
        /// <summary>
        /// 初始化<see cref="MenuItemCollection"/>类的实例
        /// </summary>
        public MenuItemCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="MenuItemCollection"/>类的实例
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public MenuItemCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
