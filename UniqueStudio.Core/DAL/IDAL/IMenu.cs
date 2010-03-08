using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface IMenu
    {        
        MenuItemInfo AddMenuItem(MenuItemInfo item);

        MenuInfo CreateMenu(MenuInfo menu);

        bool DeleteMenu(int menuId);

        MenuInfo GetMenu(int menuId);

        MenuCollection GetAllMenus();

        bool RemoveMenuItem(int itemId, bool isRemoveChildItems);

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <remarks>
        /// 如果Items不为空，则同时更新菜单项
        /// </remarks>
        /// <param name="menu"></param>
        /// <returns></returns>
        bool UpdateMenu(MenuInfo menu);
    }
}
