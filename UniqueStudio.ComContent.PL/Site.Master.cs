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
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Menu;
using UniqueStudio.Core.PageVisit;

namespace UniqueStudio.ComContent.PL
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MenuManager manager = new MenuManager();
                MenuInfo menu = manager.GetMenu(1);
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
