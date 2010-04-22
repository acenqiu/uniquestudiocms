using System;
using System.Text;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;
using UniqueStudio.Common.Config;
using UniqueStudio.Core.Menu;

namespace UniqueStudio.CGE.controls
{
    public partial class SubCategories : System.Web.UI.UserControl
    {
        private int categoryId = 0;

        public int CategoryId
        {
            set { categoryId = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && categoryId != 0)
            {

                int siteId = (int)this.Session[GlobalConfig.SESSION_SITEID];
                MenuManager manager = new MenuManager();
                MenuInfo menu = manager.GetMenu(siteId);
                MenuItemInfo menuCat = null;
                CategoryInfo root = (new CategoryManager()).GetCategoryChain(categoryId);
                if (menu != null)
                {
                    MenuItemInfo head = manager.GetMenuTree(menu.Items);
                    menuCat = GetMenuByCategoryId(head, categoryId);
                    //navigationMenu.Text = manager.GetMenuHtml(head);
                }

                if (menuCat != null)
                {
                    ltlCategoryName.Text = root.CategoryName;
                    ltlList.Text = manager.GetMenuHtml(menuCat).Replace("candy-menu","").Replace("hide(this)","").Replace("show(this)","");
                }
                else
                {

                   
                    if (root != null)
                    {
                        ltlCategoryName.Text = root.CategoryName;
                        ltlList.Text = GetCategoryHtml(root);
                    }
                }
            }
        }

        private MenuItemInfo GetMenuByCategoryId(MenuItemInfo menu,int id)
        {
            string sb=new StringBuilder("list.aspx?catId=").Append(id).ToString();
            MenuItemInfo info = null;
            if (menu.Link.Equals(sb))
            {
                return menu;
            }
            else
            {
                foreach (MenuItemInfo item in menu.ChildItems)
                {
                    if (info==null)
                    {
                       info=GetMenuByCategoryId(item, id);
                    }
                }
                return info;
            }
        }

        private string GetCategoryHtml(CategoryInfo root)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>").Append("\r\n");
            foreach (CategoryInfo item in root.ChildCategories)
            {
                sb.Append(GetHtml(item)).Append("\r\n");
            }
            sb.Append("</ul>").Append("\r\n");
            return sb.ToString();
        }

        private string GetHtml(CategoryInfo node)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<li>\r\n<a href='list.aspx?catId={0}' >{1}</a>\r\n", node.CategoryId, node.CategoryName));

            if (node.ChildCategories != null && node.ChildCategories.Count != 0)
            {
                sb.Append("<ul>\r\n");
                foreach (CategoryInfo child in node.ChildCategories)
                {
                    sb.Append(GetHtml(child)).Append("\r\n");
                }
                sb.Append("</ul>\r\n");
            }

            sb.Append("</li>\r\n");
            return sb.ToString();
        }
    }
}