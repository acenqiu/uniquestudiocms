using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class MenuItemInfo
    {
        private int id;
        private int menuId;
        private string itemName;
        private string parentItemName=string.Empty;
        private string link;
        private string target = string.Empty;
        private int depth;
        private int ordering = 0;
        private int subOf;

        public MenuItemInfo()
        {
            ChildList = new List<MenuItemInfo>();
            //默认构造函数
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public string ParentItemName
        {
            get { return parentItemName; }
            set { parentItemName = value; }
        }
        public string Link
        {
            get { return link; }
            set { link = value; }
        }
        public string Target
        {
            get { return target; }
            set { target = value; }
        }
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        public int Ordering
        {
            get { return ordering; }
            set { ordering = value; }
        }
        public int SubOf
        {
            get { return subOf; }
            set { subOf = value; }
        }
        public List<MenuItemInfo> ChildList;
    }
}
