using System;
using System.Collections;
using System.Collections.Generic;
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
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Admin.admin.background
{
    public partial class menulist : UniqueStudio.Controls.BasePage
    {
        private MenuManager manager = new MenuManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            try
            {
                rptList.DataSource = manager.GetAllMenus();
                rptList.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            MenuInfo menu = new MenuInfo();
            menu.MenuName = txtMenuName.Text.Trim();
            menu.Description = txtDescription.Text.Trim();

            try
            {
                menu = manager.CreateMenu((UserInfo)this.Session[GlobalConfig.SESSION_USER], menu);
                if (menu != null)
                {
                    message.SetSuccessMessage("菜单创建成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("菜单创建失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
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
                    if (manager.DeleteMenus(list.ToArray()))
                    {
                        message.SetSuccessMessage("所选菜单已删除！");
                    }
                    else
                    {
                        message.SetErrorMessage("所选菜单删除失败！");
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage(ex.Message);
                }
                GetData();
            }
        }
    }
}
