using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using UniqueStudio.Core.Permission;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.Menu
{
    public class MenuManager
    {
        private static readonly IMenu provider = DALFactory.CreateMenu();

        public MenuManager()
        {
            //默认构造函数
        }

        public MenuItemInfo AddMenuItem(UserInfo currentUser, MenuItemInfo item)
        {
            if (!PermissionManager.HasPermission(currentUser, "EditMenu"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.AddMenuItem(item);
        }

        public MenuInfo CreateMenu(UserInfo currentUser, MenuInfo menu)
        {
            if (!PermissionManager.HasPermission(currentUser, "CreateMenu"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.CreateMenu(menu);
        }

        public bool DeleteMenu(UserInfo currentUser, int menuId)
        {
            if (!PermissionManager.HasPermission(currentUser, "DeleteMenu"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.DeleteMenu(menuId);
        }

        public MenuInfo GetMenu(int menuId)
        {
            return provider.GetMenu(menuId);
        }

        public MenuCollection GetAllMenus()
        {
            return provider.GetAllMenus();
        }

        public bool RemoveMenuItem(UserInfo currentUser, int itemId)
        {
            //全局配置
            return RemoveMenuItem(currentUser, itemId, true);
        }

        public bool RemoveMenuItem(UserInfo currentUser, int itemId, bool isRemoveChildItems)
        {
            if (!PermissionManager.HasPermission(currentUser, "EditMenu"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.RemoveMenuItem(itemId, isRemoveChildItems);
        }

        public bool UpdateMenu(UserInfo currentUser, MenuInfo menu)
        {
            throw new NotImplementedException();
        }

        public MenuItemInfo GetMenuTree(List<MenuItemInfo> menus)
        {
            Hashtable idTable = new Hashtable();
            MenuItemInfo head = new MenuItemInfo();
            head.Depth = 0;
            head.Id = 0;
            menus.Insert(0, head);
            foreach (MenuItemInfo item in menus)
            {
                idTable.Add(item.Id, menus.IndexOf(item));
            }
            foreach (MenuItemInfo item in menus)
            {
                menus[Convert.ToInt16(idTable[item.ParentItemId])].ChildItems.Add(item);
            }
            menus[0].ChildItems.Remove(menus[0]);
            return head;
        }
        public string GetMenuHtml(MenuItemInfo head)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>").Append("\r\n");
            foreach (MenuItemInfo child in head.ChildItems)
            {
                sb.Append(getHTML(child,new StringBuilder("menuPath=").Append(child.Id.ToString()).ToString())).Append("\r\n");
            }
            sb.Append("</ul>").Append("\r\n");
            return sb.ToString();
        }
        private string getHTML(MenuItemInfo node,string menuPath)
        {
            StringBuilder sb = new StringBuilder();

            if (node.ChildItems.Count > 0)
            {
                sb.Append("<li onmouseover='show(this)' onmouseout='hide(this)' class='li-node'>").Append("\r\n");
                sb.Append("<div class='candy-menu'>").Append("\r\n");
                sb.Append("<ul>").Append("\r\n");
                foreach (MenuItemInfo child in node.ChildItems)
                {
                    sb.Append(getHTML(child,menuPath+","+child.Id)).Append("\r\n");
                }
                sb.Append("</ul>").Append("\r\n");
                sb.Append("</div>").Append("\r\n");
            }
            else
            {
                sb.Append("<li>").Append("\r\n");
            }
            if ((node.Link != null) && (!node.Link.Equals(String.Empty)))
            {
                sb.Append("<a href='").Append(node.Link);
                if (node.Link.IndexOf("?") < 0)
                {
                    sb.Append("?");
                }
                else
                {
                    sb.Append("&");
                }
                sb.Append(menuPath).Append("'>").Append("\r\n");
                sb.Append(node.ItemName).Append("\r\n");
                sb.Append("</a>").Append("\r\n");
            }
            else
            {
                sb.Append(node.ItemName).Append("\r\n");
            }

            sb.Append("</li>").Append("\r\n");
            return sb.ToString();
        }
    }
}
