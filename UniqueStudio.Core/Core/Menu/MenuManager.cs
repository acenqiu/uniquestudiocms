//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供菜单管理的方法。
// 完成日期：2010年03月20日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.Menu
{
    /// <summary>
    /// 提供菜单管理的方法。
    /// </summary>
    public class MenuManager
    {
        private static readonly IMenu provider = DALFactory.CreateMenu();

        private UserInfo currentUser;

        /// <summary>
        /// 初始化<see cref="MenuManager"/>类的实例。
        /// </summary>
        public MenuManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前用户信息初始化<see cref="MenuManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前用户信息。</param>
        public MenuManager(UserInfo currentUser)
        {
            Validator.CheckNull(currentUser, "currentUser");
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 添加菜单项。
        /// </summary>
        /// <param name="item">菜单项信息。</param>
        /// <returns>如果添加成功，返回该菜单项信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有添加菜单项的权限时抛出该异常。</exception>
        public MenuItemInfo AddMenuItem(MenuItemInfo item)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return AddMenuItem(currentUser, item);
        }

        /// <summary>
        /// 添加菜单项。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="item">菜单项信息。</param>
        /// <returns>如果添加成功，返回该菜单项信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有添加菜单项的权限时抛出该异常。</exception>
        public MenuItemInfo AddMenuItem(UserInfo currentUser, MenuItemInfo item)
        {
            Validator.CheckNull(item, "item");
            Validator.CheckStringNull(item.ItemName, "item.ItemName");
            Validator.CheckNotPositive(item.MenuId, "item.MenuId");
            Validator.CheckNegative(item.ParentItemId, "item.ParentItemId");
            if (item.Ordering < 0)
            {
                item.Ordering = 0;
            }
            PermissionManager.CheckPermission(currentUser, "EditMenu", "编辑菜单");

            //不需要如此严格地限定菜单项名称
            //if (IsMenuItemExist(item.MenuId, item.ItemName))
            //{
            //    throw new Exception("该菜单项名称已经存在，请重新设置！");
            //}

            try
            {
                return provider.AddMenuItem(item);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 创建菜单。
        /// </summary>
        /// <param name="menu">菜单信息。</param>
        /// <returns>如果添加成功，返回该菜单信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建菜单的权限时抛出该异常。</exception>
        public MenuInfo CreateMenu(MenuInfo menu)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return CreateMenu(currentUser, menu);
        }

        /// <summary>
        /// 创建菜单。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="menu">菜单信息。</param>
        /// <returns>如果添加成功，返回该菜单信息，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建菜单的权限时抛出该异常。</exception>
        public MenuInfo CreateMenu(UserInfo currentUser, MenuInfo menu)
        {
            Validator.CheckNull(menu, "menu");
            Validator.CheckNotPositive(menu.SiteId, "menu.SiteId");
            Validator.CheckStringNull(menu.MenuName, "menu");
            if (menu.Description == null)
            {
                menu.Description = string.Empty;
            }
            PermissionManager.CheckPermission(currentUser, "CreateMenu", "创建菜单");

            if (IsMenuExist(menu.SiteId, menu.MenuName))
            {
                throw new Exception("该菜单名已经存在，请重新设置！");
            }

            try
            {
                return provider.CreateMenu(menu);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 删除指定菜单。
        /// </summary>
        /// <param name="menuId">待删除菜单ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除菜单的权限时抛出该异常。</exception>
        public bool DeleteMenu(int menuId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return DeleteMenu(currentUser, menuId);
        }

        /// <summary>
        /// 删除指定菜单。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="menuId">待删除菜单ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除菜单的权限时抛出该异常。</exception>
        public bool DeleteMenu(UserInfo currentUser, int menuId)
        {
            Validator.CheckNotPositive(menuId, "menuId");
            PermissionManager.CheckPermission(currentUser, "DeleteMenu", "删除菜单");

            try
            {
                return provider.DeleteMenu(menuId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 删除多个菜单。
        /// </summary>
        /// <param name="menuIds">待删除菜单ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除菜单的权限时抛出该异常。</exception>
        public bool DeleteMenus(int[] menuIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return DeleteMenus(currentUser, menuIds);
        }

        /// <summary>
        /// 删除多个菜单。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="menuIds">待删除菜单ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除菜单的权限时抛出该异常。</exception>
        public bool DeleteMenus(UserInfo currentUser, int[] menuIds)
        {
            foreach (int menuId in menuIds)
            {
                Validator.CheckNotPositive(menuId, "menuIds");
            }
            PermissionManager.CheckPermission(currentUser, "DeleteMenu", "删除菜单");

            try
            {
                return provider.DeleteMenus(menuIds);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 获取指定菜单。
        /// </summary>
        /// <remarks>包含所有菜单项。</remarks>
        /// <param name="menuId">菜单ID。</param>
        /// <returns>菜单信息。</returns>
        public MenuInfo GetMenu(int menuId)
        {
            Validator.CheckNotPositive(menuId, "menuId");

            try
            {
                return provider.GetMenu(menuId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回菜单列表。
        /// </summary>
        /// <remarks>不含菜单项。</remarks>
        /// <param name="siteId">网站ID。</param>
        /// <returns>菜单的集合。</returns>
        public MenuCollection GetAllMenus(int siteId)
        {
            try
            {
                return provider.GetAllMenus(siteId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回菜单链。
        /// </summary>
        /// <param name="MenuItemId">该菜单链中任一菜单项的ID。</param>
        /// <returns>菜单链的根节点。</returns>
        public MenuItemInfo GetMenuChain(int menuItemId)
        {
            Validator.CheckNotPositive(menuItemId, "menuItemId");

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
                    return null;
                }
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回菜单链。
        /// </summary>
        /// <param name="chainId">该菜单链的ID。</param>
        /// <returns>菜单链的根节点。</returns>
        public MenuItemInfo GetMenuChain(Guid chainId)
        {
            Validator.CheckGuid(chainId, "chainId");

            try
            {
                MenuItemCollection collection = provider.GetMenuChain(chainId);
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
                    return null;
                }
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定菜单项信息。
        /// </summary>
        /// <param name="itemId">菜单项ID。</param>
        /// <returns>菜单项信息。</returns>
        public MenuItemInfo GetMenuItem(int itemId)
        {
            Validator.CheckNotPositive(itemId, "itemId");
            try
            {
                return provider.GetMenuItem(itemId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定菜单的所有菜单项。
        /// </summary>
        /// <param name="menuId">菜单ID。</param>
        /// <returns>菜单项的集合。</returns>
        public MenuItemCollection GetMenuItems(int menuId)
        {
            Validator.CheckNotPositive(menuId, "menuId");

            try
            {
                return provider.GetMenuItems(menuId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定菜单是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="menuName">菜单名称。</param>
        /// <returns>是否存在。</returns>
        public bool IsMenuExist(int siteId, string menuName)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(menuName, "menuName");

            try
            {
                return provider.IsMenuExist(siteId, menuName);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定菜单项是否存在。
        /// </summary>
        /// <param name="menuId">菜单ID。</param>
        /// <param name="menuItemName">菜单项名称。</param>
        /// <returns>是否存在。</returns>
        public bool IsMenuItemExist(int menuId, string menuItemName)
        {
            Validator.CheckNotPositive(menuId, "menuId");
            Validator.CheckStringNull(menuItemName, "menuItemName");

            try
            {
                return provider.IsMenuItemExist(menuId, menuItemName);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 移除菜单项。
        /// </summary>
        /// <param name="itemId">菜单项ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool RemoveMenuItem(int itemId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return RemoveMenuItem(currentUser, itemId);
        }

        /// <summary>
        /// 移除菜单项。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="itemId">菜单项ID。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool RemoveMenuItem(UserInfo currentUser, int itemId)
        {
            return RemoveMenuItem(currentUser, itemId, false);
        }

        /// <summary>
        /// 移除菜单项。
        /// </summary>
        /// <remarks>参数<paramref name="isRemoveChildItems"/>暂不可用，当前行为为不处理。</remarks>
        /// <param name="itemId">菜单项ID。</param>
        /// <param name="isRemoveChildItems">是否同时移除其子菜单项。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool RemoveMenuItem(int itemId, bool isRemoveChildItems)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return RemoveMenuItem(currentUser, itemId, isRemoveChildItems);
        }

        /// <summary>
        /// 移除菜单项。
        /// </summary>
        /// <remarks>参数<paramref name="isRemoveChildItems"/>暂不可用，当前行为为不处理。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="itemId">菜单项ID。</param>
        /// <param name="isRemoveChildItems">是否同时移除其子菜单项。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool RemoveMenuItem(UserInfo currentUser, int itemId, bool isRemoveChildItems)
        {
            Validator.CheckNotPositive(itemId, "itemId");
            PermissionManager.CheckPermission(currentUser, "EditMenu", "编辑菜单");

            try
            {
                return provider.RemoveMenuItem(itemId, isRemoveChildItems);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 移除多个菜单项。
        /// </summary>
        /// <param name="itemIds">菜单项ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool RemoveMenuItems(int[] itemIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return RemoveMenuItems(currentUser, itemIds);
        }

        /// <summary>
        /// 移除多个菜单项。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="itemIds">菜单项ID的集合。</param>
        /// <returns>是否移除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool RemoveMenuItems(UserInfo currentUser, int[] itemIds)
        {
            foreach (int itemId in itemIds)
            {
                Validator.CheckNotPositive(itemId, "itemIds");
            }
            PermissionManager.CheckPermission(currentUser, "EditMenu", "编辑菜单");

            try
            {
                return provider.RemoveMenuItems(itemIds);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 更新菜单信息。
        /// </summary>
        /// <remarks>该方法仅更新菜单的名称及说明，不更新菜单项。</remarks>
        /// <param name="menu">菜单信息。</param>
        /// <param name="oldMenuName">原始菜单名。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool UpdateMenu(MenuInfo menu, string oldMenuName)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return UpdateMenu(currentUser, menu, oldMenuName);
        }

        /// <summary>
        /// 更新菜单信息。
        /// </summary>
        /// <remarks>该方法仅更新菜单的名称及说明，不更新菜单项。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="menu">菜单信息。</param>
        /// <param name="oldMenuName">原始菜单名。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool UpdateMenu(UserInfo currentUser, MenuInfo menu, string oldMenuName)
        {
            Validator.CheckNull(menu, "menu");
            Validator.CheckNotPositive(menu.MenuId, "menu.MenuId");
            Validator.CheckStringNull(menu.MenuName, "menu.MenuName");
            if (menu.Description == null)
            {
                menu.Description = string.Empty;
            }
            PermissionManager.CheckPermission(currentUser, "EditMenu", "编辑菜单");

            if (menu.MenuName != oldMenuName && IsMenuExist(menu.SiteId, menu.MenuName))
            {
                throw new Exception("该菜单名已经存在，请重新设置！");
            }

            try
            {
                return provider.UpdateMenu(menu);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 更新菜单项信息。
        /// </summary>
        /// <param name="item">菜单信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool UpdateMenuItem(MenuItemInfo item)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用MenuManager(UserInfo)实例化该类。");
            }
            return UpdateMenuItem(currentUser, item);
        }

        /// <summary>
        /// 更新菜单项信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="item">菜单信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑菜单的权限时抛出该异常。</exception>
        public bool UpdateMenuItem(UserInfo currentUser, MenuItemInfo item)
        {
            Validator.CheckNull(item, "item");
            Validator.CheckStringNull(item.ItemName, "item.ItemName");
            Validator.CheckNotPositive(item.Id, "item.Id");
            Validator.CheckNegative(item.ParentItemId, "item");
            if (item.Ordering < 0)
            {
                item.Ordering = 0;
            }
            if (item.Link == null)
            {
                item.Link = string.Empty;
            }
            if (item.Target == null)
            {
                item.Target = string.Empty;
            }

            PermissionManager.CheckPermission(currentUser, "EditMenu", "编辑菜单");

            //if (IsMenuItemExist(item.MenuId, item.ItemName))
            //{
            //    throw new Exception("该菜单项名称已经存在，请重新设置！");
            //}

            try
            {
                return provider.UpdateMenuItem(item);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回菜单树形结构。
        /// </summary>
        /// <remarks>该根节点为临时创建的节点。</remarks>
        /// <param name="menuItems">菜单项的集合。</param>
        /// <returns>菜单树形结构的根节点。</returns>
        public MenuItemInfo GetMenuTree(MenuItemCollection menuItems)
        {
            Validator.CheckNull(menuItems, "menuItems");

            Dictionary<int, int> dicIds = new Dictionary<int, int>();

            MenuItemInfo head = new MenuItemInfo();
            head.Depth = 0;
            head.Id = 0;
            menuItems.Insert(0, head);
            for (int i = 0; i < menuItems.Count; i++)
            {
                dicIds.Add(menuItems[i].Id, i);
                menuItems[i].ChildItems = new MenuItemCollection();
            }
            foreach (MenuItemInfo item in menuItems)
            {
                menuItems[dicIds[item.ParentItemId]].ChildItems.Add(item);
            }
            menuItems[0].ChildItems.Remove(menuItems[0]);
            return head;
        }

        /// <summary>
        /// 返回菜单html代码。
        /// </summary>
        /// <remarks>该方法将在后续版本中移除！</remarks>
        /// <param name="head">菜单树形结构的根节点。</param>
        /// <returns>菜单html代码。</returns>
        public string GetMenuHtml(MenuItemInfo head)
        {
            Validator.CheckNull(head, "head");

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
