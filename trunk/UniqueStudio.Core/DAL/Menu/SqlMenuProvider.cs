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
        private const string GET_MENU = "GetMenu";
        private const string GET_MENU_ITEMS = "GetMenuItems";

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
                using (SqlDataReader reader = SqlHelper.ExecuteReader(conn,CommandType.StoredProcedure, GET_MENU, parm))
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
        /// <returns>菜单的集合</returns>
        public MenuCollection GetAllMenus()
        {
            MenuCollection collection = new MenuCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_MENUS, null))
            {
                while (reader.Read())
                {
                    collection.Add(FillMenuInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 移除菜单项
        /// </summary>
        /// <param name="itemId">菜单项ID</param>
        /// <param name="isRemoveChildItems">是否同时移除其子菜单项</param>
        /// <returns>是否删除成功</returns>
        public bool RemoveMenuItem(int itemId, bool isRemoveChildItems)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
