using System;
using System.Web;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Menu;
using UniqueStudio.Core.PageVisit;

namespace UniqueStudio.CGE
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int siteId = (int)this.Session[GlobalConfig.SESSION_SITEID];
            MenuManager manager = new MenuManager();
            MenuInfo menu = manager.GetMenu(siteId);
            if (menu != null)
            {
                MenuItemInfo head = manager.GetMenuTree(menu.Items);
                navigationMenu.Text = manager.GetMenuHtml(head);
            }

            if (!IsPostBack)
            {
                try
                {
                    ltlPv.Text = PageVisitManager.GetPageVisitCount().ToString();
                }
                catch
                {
                    //不做处理
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                Response.Redirect("search.aspx?title=" + HttpUtility.UrlEncode(txtSearch.Text.Trim()));
            }
        }
    }
}
