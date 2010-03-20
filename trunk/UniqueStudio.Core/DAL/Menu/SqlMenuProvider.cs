using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.Menu
{
    /// <summary>
    /// 提供菜单管理在Sql Server上的实现方法
    /// </summary>
    internal class SqlMenuProvider : IMenu
    {
        private const string ADD_MENU_ITEM = "AddMenuItem";
        private const string CREATE_MENU = "CreateMenu";
        private const string DELETE_MENU = "DeleteMenu";   //没有完成
        private const string GET_ALL_MENUS = "GetAllMenus";
        private const string GET_MENU_CHAIN_BY_ITEMID = "GetMenuChainByItemId";
        private const string GET_MENU_CHAIN_BY_CHAINID = "GetMenuChainByChainId";
        private const string GET_MENU = "GetMenu";
        private const string GET_MENU_ITEMS = "GetMenuItems";
        private const string IS_MENU_EXIST = "IsMenuExist";
        private const string IS_MENU_ITEM_EXIST = "IsMenuItemExist";
        private const string REMOVE_MENU_ITEM = "RemoveMenuItem";
        private const string UPDATE_MENU = "UpdateMenu";
        private const string UPDATE_MENU_ITEM = "UpdateMenuItem";

        /// <summary>
        /// 初始化<see cref="SqlMenuProvider"/>类的实例
        /// </summary>
        public SqlMenuProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="item">菜单项信息</param>
        /// <returns>如果添加成功，返回该菜单项信息，否则返回空</returns>
        public MenuItemInfo AddMenuItem(MenuItemInfo item)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@MenuID",item.MenuId),
                                                        new SqlParameter("@ItemName",item.ItemName),
                                                        new SqlParameter("@Link",item.Link),
                                                        new SqlParameter("@Target",item.Target),
                                                        new SqlParameter("@Ordering",item.Ordering),
                                                        new SqlParameter("@SubOf",item.ParentItemId)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, ADD_MENU_ITEM, parms);
            if (o != null && o != DBNull.Value)
            {
                item.Id = Convert.ToInt32(o);
                return item;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menu">菜单信息</param>
        /// <returns>如果添加成功，返回该菜单信息，否则返回空</returns>
        public MenuInfo CreateMenu(MenuInfo menu)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@SiteID",menu.SiteId),
                                                        new SqlParameter("@MenuName",menu.MenuName),
                                                        new SqlParameter("@Description",menu.Description)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_MENU, parms);
            if (o != null && o != DBNull.Value)
            {
                menu.MenuId = Convert.ToInt32(o);
                return menu;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 删除指定菜单
        /// </summary>
        /// <param name="menuId">待删除菜单ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteMenu(int menuId)
        {
            SqlParameter parm = new SqlParameter("@MenuID", menuId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_MENU, parm) > 0;
        }

        /// <summary>
        /// 删除多个菜单
        /// </summary>
        /// <param name="menuIds">待删除菜单ID的集合</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteMenus(int[] menuIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_MENU, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (int menuId in menuIds)
                            {
                                cmd.Parameters[0].Value = menuId;
                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定菜单
        /// </summary>
        /// <remarks>包含所有菜单项</remarks>
        /// <param name="menuId">菜单ID</param>
        /// <returns>菜单信息</returns>
        public MenuInfo GetMenu(int menuId)
        {
            MenuInfo menu = null;
            SqlParameter parm = new SqlParameter("@MenuID", menuId);
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, GET_MENU, parm))
                {
                    if (reader.Read())
                    {
                        menu = FillMenuInfo(reader);
                    }
                    reader.Close();
                }
                if (menu != null)
                {
                    menu.Items = new MenuItemCollection();
                    using (SqlDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, GET_MENU_ITEMS, parm))
                    {
                        while (reader.Read())
                        {
                            menu.Items.Add(FillMenuItemInfo(reader));
                        }
                        reader.Close();
                    }
                }
                conn.Close();
            }
            return menu;
        }

        /// <summary>
        /// 返回菜单列表
        /// </summary>
        /// <remarks>不含菜单项</remarks>
        /// <param name="siteId">网站ID。</param>
        /// <returns>菜单的集合</returns>
        public MenuCollection GetAllMenus(int siteId)
        {
            MenuCollection collection = new MenuCollection();
            SqlParameter parm = new SqlParameter("@SiteID", siteId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_MENUS, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillMenuInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回菜单链
        /// </summary>
        /// <param name="MenuItemId">该菜单链中任一菜单项的ID</param>
        /// <returns>菜单链中各菜单项的集合</returns>
        public MenuItemCollection GetMenuChain(int menuItemId)
        {
            MenuItemCollection collection = new MenuItemCollection();
            SqlParameter parm = new SqlParameter("@ItemID", menuItemId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_MENU_CHAIN_BY_ITEMID, parm))
            {
                while (reader.Read())
                {
                    MenuItemInfo item = FillMenuItemInfo(reader);
                    collection.Add(item);
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回菜单链
        /// </summary>
        /// <param name="chainId">该菜单链的ID</param>
        /// <returns>菜单链中各菜单项的集合</returns>
        public MenuItemCollection GetMenuChain(Guid chainId)
        {
            MenuItemCollection collection = new MenuItemCollection();
            SqlParameter parm = new SqlParameter("@ChainID", chainId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_MENU_CHAIN_BY_CHAINID, parm))
            {
                while (reader.Read())
                {
                    MenuItemInfo item = FillMenuItemInfo(reader);
                    collection.Add(item);
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回指定菜单是否存在
        /// </summary>
        /// <param name="siteId">网站ID</param>
        /// <param name="menuName">菜单名称</param>
        /// <returns>是否存在</returns>
        public bool IsMenuExist(int siteId, string menuName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteID",siteId),
                                                    new SqlParameter("@MenuName",menuName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_MENU_EXIST, parms);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean((int)o);
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 返回指定菜单项是否存在
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="menuItemName">菜单项名称</param>
        /// <returns>是否存在</returns>
        public bool IsMenuItemExist(int menuId, string menuItemName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@MenuID",menuId),
                                                    new SqlParameter("@ItemName",menuItemName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_MENU_ITEM_EXIST, parms);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean((int)o);
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 移除菜单项
        /// </summary>
        /// <param name="itemId">菜单项ID</param>
        /// <param name="isRemoveChildItems">是否同时移除其子菜单项</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveMenuItem(int itemId, bool isRemoveChildItems)
        {
            SqlParameter parm = new SqlParameter("@ItemID", itemId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, REMOVE_MENU_ITEM, parm) > 0;
        }

        /// <summary>
        /// 移除多个菜单项
        /// </summary>
        /// <param name="itemIds">菜单项ID的集合</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveMenuItems(int[] itemIds)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(REMOVE_MENU_ITEM, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (int itemId in itemIds)
                            {
                                cmd.Parameters[0].Value = itemId;
                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <remarks>
        /// 如果Items不为空，则同时更新菜单项
        /// </remarks>
        /// <param name="menu">菜单信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateMenu(MenuInfo menu)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@MenuID",menu.MenuId),
                                                    new SqlParameter("@MenuName",menu.MenuName),
                                                    new SqlParameter("@Description",menu.Description)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_MENU, parms) > 0;
        }

        /// <summary>
        /// 更新菜单项信息
        /// </summary>
        /// <param name="item">菜单信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateMenuItem(MenuItemInfo item)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@ItemID",item.Id),
                                                        new SqlParameter("@ItemName",item.ItemName),
                                                        new SqlParameter("@Link",item.Link),
                                                        new SqlParameter("@Target",item.Target),
                                                        new SqlParameter("@Ordering",item.Ordering),
                                                        new SqlParameter("@SubOf",item.ParentItemId)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_MENU_ITEM, parms) > 0;
        }

        private MenuInfo FillMenuInfo(SqlDataReader reader)
        {
            MenuInfo menu = new MenuInfo();
            menu.MenuId = (int)reader["ID"];
            menu.MenuName = reader["MenuName"].ToString();
            menu.Description = reader["Description"].ToString();
            return menu;
        }

        private MenuItemInfo FillMenuItemInfo(SqlDataReader reader)
        {
            MenuItemInfo item = new MenuItemInfo();
            item.Id = (int)reader["ID"];
            item.ItemName = reader["ItemName"].ToString();
            item.MenuId = (int)reader["MenuID"];
            item.Link = reader["Link"].ToString();
            item.Target = reader["Target"].ToString();
            item.Depth = (int)reader["Depth"];
            item.Ordering = (int)reader["Ordering"];
            item.ParentItemId = (int)reader["SubOf"];
            if (reader["ParentItemName"] != DBNull.Value)
            {
                item.ParentItemName = reader["ParentItemName"].ToString();
            }
            return item;
        }
    }
}
