using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示菜单的实体类
    /// </summary>
    [Serializable]
    public class MenuInfo
    {
        private int menuId;
        private int siteId = 1;
        private string menuName;
        private string description = string.Empty;
        private MenuItemCollection items = null;

        /// <summary>
        /// 初始化<see cref="MenuInfo"/>类的实例
        /// </summary>
        public MenuInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以菜单名称、说明初始化<see cref="MenuCollection"/>类的实例
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="description">菜单说明</param>
        public MenuInfo(string menuName, string description)
        {
            this.menuName = menuName;
            this.description = description;
        }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }
        /// <summary>
        /// 网站ID
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }
        /// <summary>
        /// 菜单说明
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// 菜单项的集合
        /// </summary>
        public MenuItemCollection Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
