using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.Menu;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class editmenu : System.Web.UI.Page
    {
        private int menuId;
        private MenuManager manager = new MenuManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["menuId"] != null)
            {
                if (!int.TryParse(Request.QueryString["menuId"], out menuId))
                {
                    message.SetErrorMessage("参数错误！");
                    return;
                }
            }
            else
            {
                Response.Redirect("menulist.aspx");
            }

            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            MenuInfo menu = manager.GetMenu(menuId);
            if (menu == null)
            {
                message.SetErrorMessage("指定的菜单不存在！");
            }
            else
            {
                txtMenuName.Text = menu.MenuName;
                txtDescription.Text = menu.Description;

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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            MenuItemInfo item = new MenuItemInfo();
            item.MenuId = menuId;
            item.ItemName = txtItemName.Text;

            //修改！！
            item.Link = txtLink.Text;
            item.Target = txtTarget.Text;
            int ordering;
            if (int.TryParse(txtOrdering.Text, out ordering))
            {
                item.Ordering = ordering;
            }
            else
            {
                item.Ordering = 0;
            }
            item.ParentItemId = Convert.ToInt32(ddlItems.SelectedValue);

            try
            {
                item = manager.AddMenuItem((UserInfo)this.Session[GlobalConfig.SESSION_USER], item);
                if (item != null)
                {
                    message.SetSuccessMessage("菜单项添加成功！");
                    GetData();
                    txtItemName.Text = "";
                    txtDescription.Text = "";
                    txtLink.Text = "";
                    txtTarget.Text = "";
                    txtOrdering.Text = "";
                }
                else
                {
                    message.SetErrorMessage("菜单项添加失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
