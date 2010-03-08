using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class MenuInfo
    {
        private int menuId;
        private string menuName;
        private string description=string.Empty;
        private MenuItemCollection items= null;

        public MenuInfo()
        {
            //默认构造函数
        }

        public MenuInfo(string menuName, string description)
        {
            this.menuName = menuName;
            this.description = description;
        }

        public int MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public MenuItemCollection Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
