﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示菜单项的实体类
    /// </summary>
    [Serializable]
    public class MenuItemInfo
    {
        private int id;
        private int menuId;
        private string itemName;
        private string link = null;
        private string target = string.Empty;
        private int depth;
        private int ordering = 0;
        private int parentItemId;
        private string parentItemName = string.Empty;
        private MenuItemCollection childItems;

        /// <summary>
        /// 初始化<see cref="MenuItemInfo"/>类的实例
        /// </summary>
        public MenuItemInfo()
        {
            //TODO:去除声明时初始化
            childItems = new MenuItemCollection();
        }

        /// <summary>
        /// 菜单项ID
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
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
        /// 菜单项名称
        /// </summary>
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        /// <summary>
        /// 菜单项链接
        /// </summary>
        /// <remarks>当该项为空时表示该项菜单无链接</remarks>
        public string Link
        {
            get { return link; }
            set { link = value; }
        }
        /// <summary>
        /// 链接目标
        /// </summary>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }
        /// <summary>
        /// 菜单项层次
        /// </summary>
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        /// <summary>
        /// 菜单项顺序
        /// </summary>
        public int Ordering
        {
            get { return ordering; }
            set { ordering = value; }
        }
        /// <summary>
        /// 父菜单项ID
        /// </summary>
        public int ParentItemId
        {
            get { return parentItemId; }
            set { parentItemId = value; }
        }
        /// <summary>
        /// 父菜单项名称
        /// </summary>
        /// <remarks>该项可能没有赋值</remarks>
        public string ParentItemName
        {
            get { return parentItemName; }
            set { parentItemName = value; }
        }
        /// <summary>
        /// 子菜单项
        /// </summary>
        public MenuItemCollection ChildItems
        {
            get { return childItems; }
            set { childItems = value; }
        }
    }
}
