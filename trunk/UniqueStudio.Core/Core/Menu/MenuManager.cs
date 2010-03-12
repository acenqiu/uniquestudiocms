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
    /// <summary>
    /// 提供菜单管理的方法
    /// </summary>
    public class MenuManager
    {
        private static readonly IMenu provider = DALFactory.CreateMenu();

        /// <summary>
        /// 初始化<see cref="MenuManager"/>类的实例
        /// </summary>
        public MenuManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="item">菜单项信息</param>
        /// <returns>如果添加成功，返回该菜单项信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有添加菜单项的权限时抛出该异常</exception>
        public MenuItemInfo AddMenuItem(UserInfo currentUser, MenuItemInfo item)
        {
            if (!PermissionManager.HasPermission(currentUser, "EditMenu"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.AddMenuItem(item);
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="menu">菜单信息</param>
        /// <returns>如果添加成功，返回该菜单信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建菜单的权限时抛出该异常</exception>
        public MenuInfo CreateMenu(UserInfo currentUser, MenuInfo menu)
        {
            if (!PermissionManager.HasPermission(currentUser, "CreateMenu"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.CreateMenu(menu);
        }

        /// <summary>
        /// 删除指定菜单
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="menuId">待删除菜单ID</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除菜单的权限时抛出该异常</exception>
        public bool DeleteMenu(UserInfo currentUser, int menuId)
        {
            if (!PermissionManager.HasPermission(currentUser, "DeleteMenu"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.DeleteMenu(menuId);
        }

        /// <summary>
        /// 获取指定菜单
        /// </summary>
        /// <remarks>包含所有菜单项</remarks>
        /// <param name="menuId">菜单ID</param>
        /// <returns>菜单信息</returns>
        public MenuInfo GetMenu(int menuId)
        {
            return provider.GetMenu(menuId);
        }

        /// <summary>
        /// 返回菜单列表
        /// </summary>
        /// <remarks>不含菜单项</remarks>
        /// <returns>菜单的集合</returns>
        public MenuCollection GetAllMenus()
        {
            return provider.GetAllMenus();
        }

        /// <summary>
        /// 返回菜单链
        /// </summary>
        /// <param name="MenuItemId">该菜单链中任一菜单项的ID</param>
        /// <returns>菜单链的根节点</returns>
        public MenuItemInfo GetMenuChain(int menuItemId)
        {
            try
            {
                MenuItemCollection collection = provider.GetMenuChain(menuItemId);
                if (collection != null)
                {
                    MenuItemInfo head = GetMenuTree(collection);
                    if (head.ChildItems.Count > 0)
                    {
                        return head.ChildItems[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new DatabaseException();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// 返回菜单链
        /// </summary>
        /// <param name="chainId">该菜单链的ID</param>
        /// <returns>菜单链的根节点</returns>
        public MenuItemInfo GetMenuChain(Guid chainId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除菜单项
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="itemId">菜单项ID</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常</exception>
        public bool RemoveMenuItem(UserInfo currentUser, int itemId)
        {
            //全局配置
            return RemoveMenuItem(currentUser, itemId, true);
        }

        /// <summary>
        /// 移除菜单项
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="itemId">菜单项ID</param>
        /// <param name="isRemoveChildItems">是否同时移除其子菜单项</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常</exception>
        public bool RemoveMenuItem(UserInfo currentUser, int itemId, bool isRemoveChildItems)
        {
            if (!PermissionManager.HasPermission(currentUser, "EditMenu"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.RemoveMenuItem(itemId, isRemoveChildItems);
        }

        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="menu">菜单信息</param>
        /// <returns>是否更新成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常</exception>
        public bool UpdateMenu(UserInfo currentUser, MenuInfo menu)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回菜单树形结构
        /// </summary>
        /// <remarks>该根节点为临时创建的节点</remarks>
        /// <param name="menuItems">菜单项的集合</param>
        /// <returns>菜单树形结构的根节点</returns>
        public MenuItemInfo GetMenuTree(MenuItemCollection menuItems)
        {
            Hashtable idTable = new Hashtable();
            MenuItemInfo head = new MenuItemInfo();
            head.Depth = 0;
            head.Id = 0;
            menuItems.Insert(0, head);
            foreach (MenuItemInfo item in menuItems)
            {
                idTable.Add(item.Id, menuItems.IndexOf(item));
            }
            foreach (MenuItemInfo item in menuItems)
            {
                menuItems[Convert.ToInt16(idTable[item.ParentItemId])].ChildItems.Add(item);
            }
            menuItems[0].ChildItems.Remove(menuItems[0]);
            return head;
        }

        /// <summary>
        /// 返回菜单html代码
        /// </summary>
        /// <param name="head">菜单树形结构的根节点</param>
        /// <returns>菜单html代码</returns>
        public string GetMenuHtml(MenuItemInfo head)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>").Append("\r\n");
            foreach (MenuItemInfo child in head.ChildItems)
            {
                sb.Append(GetHtml(child, new StringBuilder("menuPath=").Append(child.Id.ToString()).ToString())).Append("\r\n");
            }
            sb.Append("</ul>").Append("\r\n");
            return sb.ToString();
        }

        private string GetHtml(MenuItemInfo node, string menuPath)
        {
            StringBuilder sb = new StringBuilder();

            if (node.ChildItems.Count > 0)
            {
                sb.Append("<li onmouseover='show(this)' onmouseout='hide(this)' class='li-node'>").Append("\r\n");
                sb.Append("<div class='candy-menu'>").Append("\r\n");
                sb.Append("<ul>").Append("\r\n");
                foreach (MenuItemInfo child in node.ChildItems)
                {
                    sb.Append(GetHtml(child, menuPath + "," + child.Id)).Append("\r\n");
                }
                sb.Append("</ul>").Append("\r\n");
                sb.Append("</div>").Append("\r\n");
            }
            else
            {
                sb.Append("<li>").Append("\r\n");
            }

            if (!string.IsNullOrEmpty(node.Link))
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
