using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 菜单管理提供类需实现的方法
    /// </summary>
    internal interface IMenu
    {
        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="item">菜单项信息</param>
        /// <returns>如果添加成功，返回该菜单项信息，否则返回空</returns>
        MenuItemInfo AddMenuItem(MenuItemInfo item);

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menu">菜单信息</param>
        /// <returns>如果添加成功，返回该菜单信息，否则返回空</returns>
        MenuInfo CreateMenu(MenuInfo menu);

        /// <summary>
        /// 删除指定菜单
        /// </summary>
        /// <param name="menuId">待删除菜单ID</param>
        /// <returns>是否删除成功</returns>
        bool DeleteMenu(int menuId);

        /// <summary>
        /// 获取指定菜单
        /// </summary>
        /// <remarks>包含所有菜单项</remarks>
        /// <param name="menuId">菜单ID</param>
        /// <returns>菜单信息</returns>
        MenuInfo GetMenu(int menuId);

        /// <summary>
        /// 返回菜单列表
        /// </summary>
        /// <remarks>不含菜单项</remarks>
        /// <returns>菜单的集合</returns>
        MenuCollection GetAllMenus();

        /// <summary>
        /// 返回菜单链
        /// </summary>
        /// <param name="MenuItemId">该菜单链中任一菜单项的ID</param>
        /// <returns>菜单链中各菜单项的集合</returns>
        MenuItemCollection GetMenuChain(int menuItemId);

        /// <summary>
        /// 返回菜单链
        /// </summary>
        /// <param name="chainId">该菜单链的ID</param>
        /// <returns>菜单链中各菜单项的集合</returns>
        MenuItemCollection GetMenuChain(Guid chainId);

        /// <summary>
        /// 移除菜单项
        /// </summary>
        /// <param name="itemId">菜单项ID</param>
        /// <param name="isRemoveChildItems">是否同时移除其子菜单项</param>
        /// <returns>是否删除成功</returns>
        bool RemoveMenuItem(int itemId, bool isRemoveChildItems);

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <remarks>
        /// 如果Items不为空，则同时更新菜单项
        /// </remarks>
        /// <param name="menu">菜单信息</param>
        /// <returns>是否更新成功</returns>
        bool UpdateMenu(MenuInfo menu);
    }
}
