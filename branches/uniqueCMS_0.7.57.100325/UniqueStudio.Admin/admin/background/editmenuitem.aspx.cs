//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：编辑菜单项页面。
// 完成日期：2010年03月20日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Menu;

namespace UniqueStudio.Admin.admin.background
{
    public partial class editmenuitem : Controls.AdminBasePage
    {
        private int itemId;
        private int menuId;

        protected void Page_Load(object sender, EventArgs e)
        {
            itemId = Converter.IntParse(Request.QueryString["itemId"], 0);
            menuId = Converter.IntParse(Request.QueryString["menuId"], 0);

            if (itemId == 0)
            {
                Response.Redirect(string.Format("editmenu.aspx?siteId={0}&menuId={1}", SiteId, menuId));
            }

            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            try
            {
                MenuManager manager = new MenuManager();
                UniqueStudio.Common.Model.MenuItemCollection items = manager.GetMenuItems(menuId);
                if (items != null)
                {
                    ddlItems.Items.Clear();
                    ddlItems.Items.Add(new ListItem("无", "0"));
                    foreach (MenuItemInfo item in items)
                    {
                        ddlItems.Items.Add(new ListItem(item.ItemName, item.Id.ToString()));
                    }
                }
                else
                {
                    message.SetErrorMessage("数据读取失败：指定菜单不存在！");
                }
                
                MenuItemInfo menuItem = manager.GetMenuItem(itemId);
                if (menuItem != null)
                {
                    txtItemName.Text = menuItem.ItemName;
                    txtLink.Text = menuItem.Link;
                    txtTarget.Text = menuItem.Target;
                    txtOrdering.Text = menuItem.Ordering.ToString();
                    ddlItems.SelectedValue = menuItem.ParentItemId.ToString();
                }
                else
                {
                    message.SetErrorMessage("数据读取失败：指定菜单项不存在！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            MenuItemInfo item = new MenuItemInfo();
            item.Id = itemId;
            item.ItemName = txtItemName.Text;
            item.Link = txtLink.Text;
            item.Target = txtTarget.Text;
            item.Ordering = Converter.IntParse(txtOrdering.Text, 0);
            item.ParentItemId = Converter.IntParse(ddlItems.SelectedValue, -1);
            if (item.ParentItemId == -1)
            {
                message.SetErrorMessage("菜单项更新失败：父菜单项ID转换失败！");
                return;
            }

            try
            {
                if ((new MenuManager()).UpdateMenuItem(CurrentUser,item))
                {
                    Response.Redirect(string.Format("editmenu.aspx?siteId={0}&menuId={1}&msg={2}",SiteId
                                                                                                                                                    ,menuId     
                                                                                                                                                    ,HttpUtility.UrlEncode("菜单项更新成功！")));
                }
                else
                {
                    message.SetErrorMessage("菜单项更新失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("菜单项更新失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("editmenu.aspx?siteId={0}&menuId={1}", SiteId, menuId));
        }
    }
}
