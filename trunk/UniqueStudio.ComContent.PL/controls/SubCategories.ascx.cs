using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.Category;
using UniqueStudio.Core.Menu;
using UniqueStudio.Common;
using UniqueStudio.Common.Model;

namespace UniqueStudio.ComContent.PL.controls
{
    public partial class SubCategories : System.Web.UI.UserControl
    {
        private int categoryId;

        public int CategoryId
        {
            set { categoryId = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["menuPath"] != null)
            {
                string[] menuIds = Request.QueryString["menuPath"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (menuIds.Length > 0)
                {
                    int itemId = Utility.IntParse(menuIds[0], 0);
                    if (itemId != 0)
                    {
                        MenuManager manager = new MenuManager();
                        MenuItemInfo item = manager.GetMenuChain(itemId);
                        if (item != null)
                        {
                            ltlCategoryName.Text = item.ItemName;

                            ltlList.Text = GetMenuHtml(item);
                        }
                    }
                }
            }
        }

        private string GetMenuHtml(MenuItemInfo head)
        {
            StringBuilder sb = new StringBuilder();
            string menuPath = string.Format("menuPath={0},", head.Id);
            sb.Append("<ul>").Append("\r\n");
            foreach (MenuItemInfo item in head.ChildItems)
            {
                sb.Append(GetHtml(item, menuPath + item.Id.ToString())).Append("\r\n");
            }
            sb.Append("</ul>").Append("\r\n");
            return sb.ToString();
        }

        private string GetHtml(MenuItemInfo node, string menuPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<li>").Append("\r\n");

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

            if (node.ChildItems != null)
            {
                sb.Append("<ul>").Append("\r\n");
                foreach (MenuItemInfo child in node.ChildItems)
                {
                    sb.Append(GetHtml(child, menuPath + "," + child.Id.ToString())).Append("\r\n");
                }
                sb.Append("</ul>").Append("\r\n");
            }

            sb.Append("</li>").Append("\r\n");
            return sb.ToString();
        }
    }
}