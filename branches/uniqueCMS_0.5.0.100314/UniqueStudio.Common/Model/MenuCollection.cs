using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 菜单的集合
    /// </summary>
    [Serializable]
    public class MenuCollection:List<MenuInfo>
    {
        /// <summary>
        /// 初始化<see cref="MenuCollection"/>类的实例
        /// </summary>
        public MenuCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="MenuCollection"/>类的实例
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public MenuCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
