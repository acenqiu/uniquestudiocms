
using System;
using System.Web;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Menu;
using UniqueStudio.Core.PageVisit;
using UniqueStudio.Common.Config;

namespace UniqueStudio.CGE
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int siteId = (int)this.Session[GlobalConfig.SESSION_SITEID];
            if (!IsPostBack)
            {
                MenuManager manager = new MenuManager();
                MenuInfo menu = manager.GetMenu(siteId);
                if (menu != null)
                {
                    MenuItemInfo head = manager.GetMenuTree(menu.Items);
                    navigationMenu.Text = manager.GetMenuHtml(head);
                }

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
            Response.Redirect("search.aspx?title=" + HttpUtility.UrlEncode(txtSearch.Text.Trim()));
        }
    }
}
