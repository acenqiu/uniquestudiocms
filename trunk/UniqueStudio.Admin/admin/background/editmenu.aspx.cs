//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：编辑菜单页面。
// 完成日期：2010年03月20日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Menu;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin.background
{
    public partial class editmenu : UniqueStudio.Controls.AdminBasePage
    {
        private int menuId;
        private MenuManager manager = new MenuManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            menuId = Converter.IntParse(Request.QueryString["menuId"], 0);
            if (menuId == 0)
            {
                Response.Redirect("menulist.aspx?siteId=" + SiteId);
            }

            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            MenuInfo menu = null;
            try
            {
                menu = manager.GetMenu(menuId);
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }

            if (menu == null)
            {
                message.SetErrorMessage("数据读取失败：指定的菜单不存在！");
            }
            else
            {
                //菜单
                txtMenuName.Text = menu.MenuName;
                hfOldMenuName.Value = menu.MenuName;
                txtDescription.Text = menu.Description;

                //菜单项
                txtLink.Text = SiteManager.BaseAddress(SiteId);
                txtItemName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtTarget.Text = string.Empty;
                txtOrdering.Text = string.Empty;
                ddlItems.Items.Clear();
                ddlItems.Items.Add(new ListItem("无", "0"));
                foreach (MenuItemInfo item in menu.Items)
                {
                    ddlItems.Items.Add(new ListItem(item.ItemName, item.Id.ToString()));
                }

                rptList.DataSource = menu.Items;
                rptList.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MenuInfo menu = new MenuInfo();
            menu.MenuId = menuId;
            menu.SiteId = SiteId; //用于验证菜单名是否重复
            menu.MenuName = txtMenuName.Text.Trim();
            menu.Description = txtDescription.Text.Trim();

            try
            {
                if ((new MenuManager()).UpdateMenu(CurrentUser, menu, hfOldMenuName.Value))
                {
                    message.SetSuccessMessage("菜单修改成功！");
                }
                else
                {
                    message.SetErrorMessage("菜单修改失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("菜单修改失败：" + ex.Message);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            MenuItemInfo item = new MenuItemInfo();
            item.MenuId = menuId;
            item.ItemName = txtItemName.Text.Trim();
            item.Link = txtLink.Text.Trim();
            item.Target = txtTarget.Text.Trim();
            item.Ordering = Converter.IntParse(txtOrdering.Text, 0);
            item.ParentItemId = Converter.IntParse(ddlItems.SelectedValue, 0);

            try
            {
                item = manager.AddMenuItem(CurrentUser, item);
                if (item != null)
                {
                    message.SetSuccessMessage("菜单项添加成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("菜单项添加失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("菜单项添加失败：" + ex.Message);
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            MenuManager manager = new MenuManager(CurrentUser);
            List<int> list = new List<int>();
            if (Request.Form["chkSelected"] != null)
            {
                string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    list.Add(Converter.IntParse(ids[i], 0));
                }
            }
            else
            {
                return;
            }

            if (ddlOperation.SelectedValue == "delete")
            {
                try
                {
                    if (manager.RemoveMenuItems(list.ToArray()))
                    {
                        message.SetSuccessMessage("所选菜单项已删除！");
                    }
                    else
                    {
                        message.SetErrorMessage("所选菜单项删除失败！");
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("所选菜单项删除失败：" + ex.Message);
                }
                GetData();
            }
        }
    }
}
